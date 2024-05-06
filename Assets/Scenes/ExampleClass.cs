using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleClass : MonoBehaviour
{
    public Rect windowRect0 = new Rect(20, 20, 120, 50);
    public Rect windowRect1 = new Rect(20, 100, 120, 50);

    void OnGUI()
    {
        // Register the window. We create two windows that use the same function
        // Notice that their IDs differ
        windowRect0 = GUI.Window(0, windowRect0, DoMyWindow, "My Window");
        windowRect1 = GUI.Window(1, windowRect1, DoMyWindow, "My Window");
    }

    // Make the contents of the window
    void DoMyWindow(int windowID)
    {
        if (GUI.Button(new Rect(10, 20, 100, 20), "Hello World"))
        {
            print("Got a click in window " + windowID);
        }

        // Make the windows be draggable.
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }
}
