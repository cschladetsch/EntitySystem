using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ChaosCult.SceneLabels
{
    [InitializeOnLoad]
    public class LabelsDraw : IDrawLabels
    {
        static LabelsDraw()
        {
            // initialize label drawing and subscribe to scene-view-drawing event
            var instance = new LabelsDraw();
            SceneView.onSceneGUIDelegate += instance.DrawGui;
            LabelsAccess.Instance = instance;
        }

        private readonly List<LabelData> m_LabelsToDraw = new List<LabelData>();
        private readonly List<LabelData> m_LabelsPool = new List<LabelData>();
        private bool m_RepaintHandled;
        private bool m_DrawCalled;
        private Rect m_HandlesRect;
        private static Texture2D s_LineTex;
        private Vector2 m_MouseDownCoords;
        private LabelData m_ActiveObjectLabel;
        private readonly GUIContent m_TempContent = new GUIContent();
        private int m_HotButton;
        private LabelData m_TopLabelWithMouse;

        private void DrawGui(SceneView sceneview)
        {
            // skip layout. We don't use it anyway 
            if (Event.current.type == EventType.layout)
                return;

            // m_DrawCalled would be false if there are NO labels on-screen (the m_LabelsToDraw list
            // won't be cleared in this case
            if (Event.current.type == EventType.repaint && !m_DrawCalled)
            {
                m_LabelsToDraw.Clear();
                m_ActiveObjectLabel = null;
            }

            Profiler.BeginSample("LabelsDraw.DrawGui");

            Handles.BeginGUI();
            // reset colors. Some extensions have a habit of messing with gui colors 
            GUI.color = Color.white;
            GUI.backgroundColor = Color.white;
            GUI.contentColor = Color.white;

            // find where current tool handles are on screen. p0-p4 are the center and arrow ends
            var p0 = GetHandlePoint(Vector3.zero);
            var p1 = GetHandlePoint(Vector3.forward);
            var p2 = GetHandlePoint(Vector3.up);
            var p3 = GetHandlePoint(Vector3.right);

            switch (Tools.current)
            {
                case Tool.None:
                case Tool.View:
                    m_HandlesRect = new Rect();
                    break;
                case Tool.Move:
                case Tool.Scale:
                    // move and scale tool draws arrows - we need to find a rect big enough to contain all three
                    m_HandlesRect = new Rect(Mathf.Min(p0.x, p1.x, p2.x, p3.x), Mathf.Min(p0.y, p1.y, p2.y, p3.y),
                                             0, 0)
                                        {
                                            xMax = Mathf.Max(p0.x, p1.x, p2.x, p3.x),
                                            yMax = Mathf.Max(p0.y, p1.y, p2.y, p3.y)
                                        };
                    break;
                case Tool.Rotate:
                    // rotate tool draws circles. We find its radius by taking longest arrow from origin (this is
                    // not 100% correct, but good enough)
                    var dmax = Mathf.Max(Vector2.Distance(p1, p0), Vector2.Distance(p2, p0), Vector3.Distance(p3, p0));
                    m_HandlesRect = new Rect(p0.x - dmax, p0.y - dmax, dmax * 2, dmax * 2 - 1);
                    break;
            }

            // GUI MAGIC I. Unity uses immediate GUI, that processes all controls sequentially. This means that if a 
            // control is drawn first, it would respond to mouse input first. But with many potentially-overlapping buttons 
            // we want an opposite of that - the button that was drawn last (an is visually on top) should be pressed
            // first. This is why we have to have different code for Repaint event (where drawing happens) and all other
            // events (where input is handled). 
            // We also do all layout calculations before actual drawing - we need it to find topmost button with mouse 
            // during repaint (it's used later in Button method)
            LayoutAllLabels();
            if (Event.current.type == EventType.repaint)
            {
                for (int ii = m_LabelsToDraw.Count - 1; ii >= 0; ii--)
                {
                    Draw(m_LabelsToDraw[ii]);
                }
                // ActiveObjectLabel is special - it's currently selected object, and we want to put it on top of all 
                // others, so it's always visible
                if (m_ActiveObjectLabel != null)
                    Draw(m_ActiveObjectLabel);
                // we draw tooltip last, above all buttons
                DrawTooltip();
            }
            else
            {
                if (m_ActiveObjectLabel != null)
                    Draw(m_ActiveObjectLabel);
                foreach (var labelData in m_LabelsToDraw)
                {
                    Draw(labelData);
                }
            }

            GUI.backgroundColor = Color.white; // return it back, just in case

            Handles.EndGUI();

            Profiler.EndSample();

            if (Event.current.type == EventType.repaint)
            {
                // set flag to inform DrawLabel that repaint has finished
                m_RepaintHandled = true;
                m_DrawCalled = false;
            }
            else if (Event.current.type == EventType.mouseUp)
            {
                // release hot control in case user pressed mouse over a button, but then moved it outside
                if (m_HotButton != 0)
                {
                    m_HotButton = GUIUtility.hotControl = 0;
                }
            }
        }

        private void LayoutAllLabels()
        {
            // calculate rects for all labels, and find topmost label that contains mouse pointer. We need the latter to
            // correctly draw pressed state on buttons
            m_TopLabelWithMouse = null;
            // when m_HotButton is non-zero, we have a pressed button. Thus, the topmost one is the one we pressed mouse over,
            // not the one where the pointer is currently
            var pos = m_HotButton == 0 ? Event.current.mousePosition : m_MouseDownCoords;
            if (m_ActiveObjectLabel != null)
            {
                m_ActiveObjectLabel.Rect = CalculateRect(m_ActiveObjectLabel);
                if (m_TopLabelWithMouse == null && m_ActiveObjectLabel.Rect.Contains(pos))
                    m_TopLabelWithMouse = m_ActiveObjectLabel;
            }
            foreach (var labelData in m_LabelsToDraw)
            {
                labelData.Rect = CalculateRect(labelData);
                if (m_TopLabelWithMouse == null && labelData.Rect.Contains(pos))
                    m_TopLabelWithMouse = labelData;
            }
        }

        private void DrawTooltip()
        {
            // builtin tooltip functionality is extremely wonky in scene view. Which is why we draw it ourselves
            // after all buttons are finished. This can still go wrong if some other extension gets initialized later
            // and draws something over the tooltip, but generally tooltips work.

            if (m_TopLabelWithMouse == null || string.IsNullOrEmpty(m_TopLabelWithMouse.Tooltip))
            {
                return;
            }

            var style = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).box;
            var content = FillGUIContent(m_TopLabelWithMouse.Tooltip, null);
            var pos = Event.current.mousePosition;
            float minWidth;
            float maxWidth;
            style.CalcMinMaxWidth(content, out minWidth, out maxWidth);
            // todo: max width should be limited in case of overlong tooltips
            var height = style.CalcHeight(content, maxWidth);
            var size = style.CalcScreenSize(new Vector2(maxWidth, height));
            var rect = new Rect(pos.x, pos.y - size.y, size.x, size.y);
            GUI.Box(rect, content, style);
        }

        private GUIContent FillGUIContent(string text, Texture2D icon)
        {
            // reuse one GUIContent var to prevent constant memory allocations
            m_TempContent.text = text;
            m_TempContent.image = icon;
            return m_TempContent;
        }

        private Vector2 GetHandlePoint(Vector3 v)
        {
            // calculates position of tool arrow in screen space, given its local 3d coordinates
            var hp = Tools.handlePosition;
            var hr = Tools.handleRotation;
            var p = hp + hr * (v * HandleUtility.GetHandleSize(hp));
            var sp = HandleUtility.WorldToGUIPoint(p);
            return sp;
        }

        private void Draw(LabelData ld)
        {
            // draw actual label
            GUI.backgroundColor = ld.Background;
            if (ld.GameObject == null && ld.ButtonHandler == null)
            {
                Box(ld);
            }
            else
            {
                if (Button(ld))
                {
                    if (ld.ButtonHandler != null)
                    {
                        try
                        {
                            ld.ButtonHandler();
                        }
                        catch (Exception ex)
                        {
                            Debug.LogException(ex, ld.GameObject);
                        }
                    }
                    else if (ld.GameObject)
                        Selection.activeGameObject = ld.GameObject;
                }
            }
        }

        private Rect CalculateRect(LabelData ld)
        {
            // calculate GUI position for label
            var style = ld.Style ?? EditorStyles.miniButton;
            var content = FillGUIContent(ld.Text, ld.Icon);
            var pos = HandleUtility.WorldToGUIPoint(ld.Position);
            var size = style.CalcSize(content);
            size.x += 6;
            var rect = new Rect(pos.x - size.x / 2, pos.y - size.y / 2, size.x, size.y);

            // don't obstruct tool handles
            MoveRectAwayFromHandles(ref rect, pos);
            return rect;
        }

        private bool Button(LabelData ld)
        {
            // GUI MAGIC II. We can't simply call GUI.Button, because any time GUIUtility.GetControlId is called,
            // the rest of scene GUI breaks (specifically, the choose-perspective buttons in the top-right corner 
            // stop working. This is why we have to jump through some hoops and make a button without using GetControlId
            // (Except in one case - see lower)

            var style = ld.Style ?? EditorStyles.miniButton;
            var content = FillGUIContent(ld.Text, ld.Icon);
            var rect = ld.Rect;

            var pos = Event.current.mousePosition;
            // isActive is the button that is (or might be) pressed right now. It's the topmost
            // button that contained mouse pointer at the time of press, and still contains it now
            bool isActive = ld == m_TopLabelWithMouse && rect.Contains(pos);

            if (Event.current.type == EventType.repaint)
            {
                style.Draw(rect, content, rect.Contains(pos), m_HotButton != 0 && isActive, false, false);
            }
            if (Event.current.type == EventType.mouseDown && Event.current.modifiers == 0)
            {
                if (isActive)
                {
                    // if we detect a mouse press over button, use the event up, and declare hot control ID. This is the only time
                    // GetControlID is actually required, because we want to prevent any other controls getting input until
                    // mouse is released
                    m_HotButton = GUIUtility.hotControl = GUIUtility.GetControlID(456345, FocusType.Native, rect);
                    m_MouseDownCoords = pos;
                    Event.current.Use();
                }
            }
            if (Event.current.type == EventType.mouseUp && m_HotButton != 0)
            {
                if (isActive)
                {
                    // if mouse is released over the same button that it was pressed - register press
                    m_HotButton = GUIUtility.hotControl = 0;
                    Event.current.Use();
                    return true;
                }
            }
            return false;
        }

        private void Box(LabelData ld)
        {
            // draw a non-interactive box. We can't use GUI.Box here (see GUI MAGIC II)
            if (Event.current.type != EventType.Repaint)
                return;

            var style = ld.Style ?? EditorStyles.miniButton;
            var content = FillGUIContent(ld.Text, ld.Icon);
            var rect = ld.Rect;

            style.Draw(rect, content, false, false, false, false);
        }

        private void MoveRectAwayFromHandles(ref Rect rect, Vector2 objPos)
        {
            // Try to move label rect so that it does not obstruct tool handles (move/rotate/scale)
            // In addition to being polite, this is necessary because handles are processed before
            // labels, and you simply can't click on a label if there's a handle under it.

            // don't do anything if rects don't intersect
            if (rect.xMin > m_HandlesRect.xMax || rect.xMax < m_HandlesRect.xMin)
            {
                return;
            }
            if (rect.yMin > m_HandlesRect.yMax || rect.yMax < m_HandlesRect.yMin)
            {
                return;
            }

            // find displacements for all four sides
            Vector2 attach = Vector2.zero;
            var moveUp = rect.center.y - m_HandlesRect.yMin;
            var moveDown = m_HandlesRect.yMax - rect.center.y;
            var moveLeft = rect.center.x - m_HandlesRect.xMin;
            var moveRight = m_HandlesRect.xMax - rect.center.x;

            // select smallest displacement
            if (moveUp <= moveDown && moveUp <= moveLeft && moveUp <= moveRight)
            {
                rect.y = m_HandlesRect.yMin - rect.height;
                attach = new Vector2(rect.center.x, rect.yMax);
            }
            if (moveDown <= moveUp && moveDown <= moveLeft && moveDown <= moveRight)
            {
                rect.y = m_HandlesRect.yMax;
                attach = new Vector2(rect.center.x, rect.yMin);
            }
            if (moveLeft <= moveDown && moveLeft <= moveUp && moveLeft <= moveRight)
            {
                rect.x = m_HandlesRect.xMin - rect.width;
                attach = new Vector2(rect.xMax, rect.center.y);
            }
            if (moveRight <= moveDown && moveRight <= moveLeft && moveRight <= moveUp)
            {
                rect.x = m_HandlesRect.xMax;
                attach = new Vector2(rect.xMin, rect.center.y);
            }

            // draw a line from actual object position to displaced label
            Debug.DrawLine(objPos, attach, Color.white, 2, true);
        }

        private class LabelData
        {
            public string Text;
            public string Tooltip;
            public Texture2D Icon;
            public Vector3 Position;
            public GameObject GameObject;
            public Color Background = Color.white;
            public GUIStyle Style;
            public Action ButtonHandler;
            public Rect Rect;
        }

        public void DrawLabel(Vector3 position, string text, string tooltip, Texture2D icon, Color bgColor, GUIStyle style, GameObject gameObject, Action buttonHandler)
        {
            // Draw label for a gameobject. This method is meant to be called from OnDrawGizmos (or OnDrawGizmosSelected)
            // handler. Due to some Unity magic, we can't put actual GUI code here (it works, but some events appear to
            // skip OnDrawGizmos stage entirely, and e.g. buttons won't display pressed state if draw here). So we
            // use a clever (or maybe ugly) hack: this class subscribes to onSceneGUI event and handles all GUI there.
            // This method only stores a reference to the label in a list, for DrawGui method to draw it later.

            // We're only interested in repaint event - it's called for all labels in scene, so we can be sure to catch them all
            if (Event.current.type == EventType.repaint)
            {
                // if m_RepaintHandled flag is set, it means that we've already repainted all labels once, and this call
                // is the first for next frame. In this case, we need to clear the list of labels;
                if (m_RepaintHandled)
                {
                    // we store all cleared LabelDatas in a pool to reuse. This is done to reduce memory allocations
                    // in GUI
                    m_LabelsPool.AddRange(m_LabelsToDraw);
                    if (m_ActiveObjectLabel != null)
                        m_LabelsPool.Add(m_ActiveObjectLabel);
                    m_LabelsToDraw.Clear();
                    m_ActiveObjectLabel = null;
                    m_DrawCalled = true;
                }
                m_RepaintHandled = false;

                var labelData = CreateNewLabel(position, text, tooltip, icon, bgColor, style, gameObject, buttonHandler);
                // label for currently selected gameobject gets special treatment, because we need to always draw it on top.
                if (gameObject != null && gameObject == Selection.activeGameObject)
                {
                    m_ActiveObjectLabel = labelData;
                }
                else
                {
                    m_LabelsToDraw.Add(labelData);
                }
            }
        }

        private LabelData CreateNewLabel(Vector3 position, string text, string tooltip, Texture2D icon, Color bgColor, GUIStyle style, GameObject gameObject, Action buttonHandler)
        {
            // creates new LabelData object or gets one from the pool
            LabelData ld;
            if (m_LabelsPool.Count > 0)
            {
                ld = m_LabelsPool[m_LabelsPool.Count - 1];
                m_LabelsPool.RemoveAt(m_LabelsPool.Count - 1);
            }
            else
            {
                ld = new LabelData();
            }
            ld.Position = position;
            ld.Text = text;
            ld.Tooltip = tooltip;
            ld.Icon = icon;
            ld.Background = bgColor;
            ld.Style = style;
            ld.GameObject = gameObject;
            ld.ButtonHandler = buttonHandler;
            return ld;
        }
    }
}
