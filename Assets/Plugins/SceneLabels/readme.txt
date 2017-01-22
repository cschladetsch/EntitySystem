SceneLabels package allows easy drawing of labels and buttons directly in scene view, 
using methods like OnDrawGizmos and OnDrawGizmosSelected. Generally, you can't just 
call GUI.Button from these methods, but SceneLabels is the next best thing. 
SceneLabels function somewhat like built-in object icon, but with advanced functionality:
you can draw dynamic text (and not just the object name), change color and icon 
programmatically, add tooltips and even run custom code when clicking the label.
Also, SceneLabels automatically move away from tool handles, never getting in the way 
of actually editing your scene.


Using SceneLabels.
----

Using SceneLabels is pretty easy. Implement OnDrawGizmos method in your script, and call
on of the methods of LabelsAccess class. The main method is called DrawLabel(), and it
accepts the following parameters:
 - position. World-space position of the label. You can use simply transform.position to
   draw label directly on the object, or label some other point.
 - text. The text to display. 
 - tooltip. Tooltip text. Tooltip is displayed when mouse is over the label.
 - icon. Label icon.
 - bgColor. Label background color. This works like GUI.backgroundColor - it does not affect
   text or icon, only the background.
 - style. GUI style to use. SceneView defaults to built-in "mini-button" style, but you
   can use anything you want.
 - gameObject. The game object associated with the label. If it's not null, clicking
   the label in scene view will select this object.
 - buttonHandler. The delegate to run when label is clicked. Pass something here if you
   want more funcationality that simply selecting the object.

The other methods in LabelsAccess class have less parameters. They all ultimately call
the main DrawLabel(), adding default values where applicable.

The example scene contains a couple of scripts that use these label. Check out 
ExampleBall.cs (it uses label to display current velocity) and ExampleTrigger.cs (it
counts balls inside trigger area and displays count in scene view.) Open SceneLabels Example 
Scene to see them in action, both in run-time and design-time.

The actual drawing is handled by LabelsDraw.cs. It is thoroughly commented, so you can customize
everything even more, should you need it.