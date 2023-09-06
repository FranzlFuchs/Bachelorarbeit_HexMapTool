## UNITY_EDITOR  

 ````c#
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif 

public class Uebung
{
    #if UNITY_EDITOR

    [MenuItem("GameObject/Create HelloWorld")]
    private static void CreateHelloWorldGameObject()
    {
        if 
        (
            EditorUtility
                .DisplayDialog("Hello World",
                "Do you really want to do this?",
                "Create",
                "Cancel")
        )

        {
            new GameObject("HelloWorld");
        }
    }

#endif
    // Add your video game code here
}
````

## EditorApplication

``` c#
using System.Collections.Generic;
using UnityEditor;
using UnityEngine; 

namespace RunAndJump.LevelCreator
{
    public static class EditorUtils
    {
        // Creates a new scene
        public static void NewScene()
        {
            EditorApplication.SaveCurrentSceneIfUserWantsTo();
            EditorApplication.NewScene();
        } 

        // Remove all the elements of the scene
        public static void CleanScene()
        {
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
            foreach (GameObject go in allObjects)
            {
                GameObject.DestroyImmediate (go);
            }
        } 

        // Creates a new scene capable to be used as a level
        public static void NewLevel()
        {
            NewScene();
            CleanScene();
            GameObject levelGO = new GameObject("Level");
            levelGO.transform.position = Vector3.zero;
            levelGO.AddComponent<Level>();
        }
    }
}
```

## Gismo 

```c#
using UnityEngine; 

public class GizmoExample : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        //CODE
    }
}
```


```c#
public class GizmoExample : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
  

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
```

## Scene View mit custom GUI
```c#
Handles.BeginGUI();
	GUILayout.BeginArea(new Rect(10f, 10f, 360, 40f));
	_selectedMode = (Mode) GUILayout.Toolbar(
	(int) _currentMode,
	modeLabels.ToArray(),
	GUILayout.ExpandHeight(true));
	GUILayout.EndArea();
Handles.EndGUI();
```
