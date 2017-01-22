using System;
using ChaosCult.SceneLabels;
using UnityEngine;

namespace ChaosCult.SceneLabels
{
    public interface IDrawLabels
    {
        /// <summary>
        /// Draws (actually, queues drawing of) a GUI label in scene view. Call this method from OnDrawGizmos or OnDrawGizmosSelected
        /// </summary>
        /// <param name="position">Position of label in world space</param>
        /// <param name="text">Label text</param>
        /// <param name="tooltip">Label tooltip</param>
        /// <param name="icon">Label icon</param>
        /// <param name="bgColor">Label background color</param>
        /// <param name="style">Label GUI style</param>
        /// <param name="gameObject">GameObject to select when user clicks on label</param>
        /// <param name="buttonHandler">Method to call when user clicks on label</param>
        void DrawLabel(Vector3 position, string text, string tooltip, Texture2D icon, Color bgColor, GUIStyle style,
                       GameObject gameObject, Action buttonHandler);
    }


    public static class LabelsAccess
    {
        /// <summary>
        /// Instance of actual drawing implementation. Don't use directly.
        /// </summary>
        public static IDrawLabels Instance;

        /// <summary>
        /// Draws (actually, queues drawing of) a GUI label in scene view. Call this method from OnDrawGizmos or OnDrawGizmosSelected
        /// </summary>
        /// <param name="position">Position of label in world space</param>
        /// <param name="text">Label text</param>
        /// <param name="tooltip">Label tooltip</param>
        /// <param name="icon">Label icon</param>
        /// <param name="bgColor">Label background color</param>
        /// <param name="style">Label GUI style</param>
        /// <param name="gameObject">GameObject to select when user clicks on label</param>
        /// <param name="buttonHandler">Method to call when user clicks on label</param>
        public static void DrawLabel(Vector3 position, string text, string tooltip, Texture2D icon, Color bgColor,
                                     GUIStyle style,
                                     GameObject gameObject, Action buttonHandler)
        {
            Instance.DrawLabel(position, text, tooltip, icon, bgColor, style, gameObject, buttonHandler);
        }

        /// <summary>
        /// Draws (actually, queues drawing of) a GUI label in scene view. Call this method from OnDrawGizmos or OnDrawGizmosSelected
        /// </summary>
        /// <param name="text">Label text</param>
        /// <param name="tooltip">Label tooltip</param>
        /// <param name="icon">Label icon</param>
        /// <param name="bgColor">Label background color</param>
        /// <param name="style">Label GUI style</param>
        /// <param name="gameObject">GameObject to select when user clicks on label</param>
        public static void DrawLabel(GameObject gameObject, string text, string tooltip, Texture2D icon, Color bgColor,
                                     GUIStyle style)
        {
            DrawLabel(gameObject.transform.position, text, tooltip, icon, bgColor, style, gameObject, null);
        }

        /// <summary>
        /// Draws (actually, queues drawing of) a GUI label in scene view. Call this method from OnDrawGizmos or OnDrawGizmosSelected
        /// </summary>
        /// <param name="text">Label text</param>
        /// <param name="icon">Label icon</param>
        /// <param name="gameObject">GameObject to select when user clicks on label</param>
        public static void DrawLabel(GameObject gameObject, string text, Texture2D icon)
        {
            DrawLabel(gameObject.transform.position, text, null, icon, Color.white, null, gameObject, null);
        }

        /// <summary>
        /// Draws (actually, queues drawing of) a GUI label in scene view. Call this method from OnDrawGizmos or OnDrawGizmosSelected
        /// </summary>
        /// <param name="position">Position of label in world space</param>
        /// <param name="text">Label text</param>
        /// <param name="tooltip">Label tooltip</param>
        /// <param name="icon">Label icon</param>
        /// <param name="bgColor">Label background color</param>
        /// <param name="style">Label GUI style</param>
        public static void DrawLabel(Vector3 position, string text, string tooltip, Texture2D icon, Color bgColor,
                                     GUIStyle style)
        {
            DrawLabel(position, text, tooltip, icon, bgColor, style, null, null);
        }

        /// <summary>
        /// Draws (actually, queues drawing of) a GUI label in scene view. Call this method from OnDrawGizmos or OnDrawGizmosSelected
        /// </summary>
        /// <param name="position">Position of label in world space</param>
        /// <param name="text">Label text</param>
        /// <param name="icon">Label icon</param>
        public static void DrawLabel(Vector3 position, string text, Texture2D icon)
        {
            DrawLabel(position, text, null, icon, Color.white, null, null, null);
        }

        /// <summary>
        /// Draws (actually, queues drawing of) a GUI label in scene view. Call this method from OnDrawGizmos or OnDrawGizmosSelected
        /// </summary>
        /// <param name="position">Position of label in world space</param>
        /// <param name="text">Label text</param>
        /// <param name="tooltip">Label tooltip</param>
        /// <param name="icon">Label icon</param>
        /// <param name="bgColor">Label background color</param>
        /// <param name="style">Label GUI style</param>
        /// <param name="buttonHandler">Method to call when user clicks on label</param>
        public static void DrawButton(Vector3 position, string text, string tooltip, Texture2D icon, Color bgColor,
                                      GUIStyle style, Action buttonHandler)
        {
            Instance.DrawLabel(position, text, tooltip, icon, bgColor, style, null, buttonHandler);
        }

        /// <summary>
        /// Draws (actually, queues drawing of) a GUI label in scene view. Call this method from OnDrawGizmos or OnDrawGizmosSelected
        /// </summary>
        /// <param name="position">Position of label in world space</param>
        /// <param name="text">Label text</param>
        /// <param name="icon">Label icon</param>
        /// <param name="buttonHandler">Method to call when user clicks on label</param>
        public static void DrawButton(Vector3 position, string text, Texture2D icon, Action buttonHandler)
        {
            DrawLabel(position, text, null, icon, Color.white, null, null, buttonHandler);
        }
    }
}