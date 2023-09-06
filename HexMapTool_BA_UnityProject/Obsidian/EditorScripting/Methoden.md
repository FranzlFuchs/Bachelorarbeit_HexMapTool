``` c#
EditorUtility.DisplayDialog("Hello World","Do you really want to do this?","Create","Cancel");
```
Öffnet Dialog fenster

### Gizmos
* In Monobehaviour Klassen verwendbar

OnDrawGizmos() - Zeichnet Gizmo, das sich immer in Sceneview befindet, und mit Klick selektiert werden kann
OnDrawGizmosSelected() - Zeichnet Gizmo, wenn Objekt selektiert ist (nichtmehr "pickable")

Beispiele [[Beispiele#Gismo]]


### Inspector 
``` c#
EditorGUILayout.BeginHorizontal();
EditorGUILayout.BeginHorizontal("box");
EditorGUILayout.BeginVertical();

EditorGUILayout.EndVertical();
EditorGUILayout.EndHorizontal();
```
Formatiert Inspector

Scene View
```c#
Handles.BeginGUI();
	//GUI CODE FÜR SCENE VIEW
Handles.EndGUI();
```
- Definiert Gui Elemente
   Beispiel - [[Beispiele#Scene View mit custom GUI]]
   ```c#
   private void OnSceneGUI() 
{
	//Definitionen in diese Methode packen
}
```