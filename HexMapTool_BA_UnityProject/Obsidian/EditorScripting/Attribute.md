````c#
 [MenuItem("GameObject/Create HelloWorld")]
````
Fügt Menüpunkte zu Unity Editor Menü (+ in Hierachy) hinzu
-> können mit Hotkey ausgestattet werden
```c#
[MenuItem ("Tools/Level Creator/Show Palette _p")]
```


```c#
[DrawGizmo(GizmoType.NotInSelectionHierarchy |
GizmoType.InSelectionHierarchy |
GizmoType.Selected |
GizmoType.Active |
GizmoType.Pickable)]

private static void MyCustomOnDrawGizmos(

        Target targetExample,

        GizmoType gizmoType

    )

    {

        Gizmos.color = Color.white;

        Gizmos.DrawCube(targetExample.transform.position, Vector3.one);

    }
```
Fügt Gizmo an Monobehaviour Klassen an - Methode danach MUSS static sein

Gizmotyp muss gesetzt werden
* InSelectionHierarchy: wird gezeichnet wenn selektier, oder Kind von selektierten
* NotInSelectionHierarchy: wird gezeichnet wenn nicht selektiert, oder nicht von eltern selektiert
* Selected: gezeichnet wenn selektiert
* Active: gezeichnet wenn aktiv (im Inspektor aktiviert)
* Pickable: Gizmo kann im Editor gepickt werden

Alle Types können für unterschiedliche Resultate unterschiedliche zusammengesetzt werden

```c#
[ExecuteInEditMode]
```
Skripte werden nicht im Play, sondern in Edit Mode ausgeführt _Achtung_ manch Funktionen zB Update verhalten sich anders!
Update - Callback wenn sich etwas in der Szene verändert

```c#
[CustomEditor(typeof(Level))]
```
Zur Erstellung eines Custom Editor für alle Objekte des Typen in den ()
- Wenn man mehrere Objekte auf einmal bearbeiten will, gibt es spezielle Attribute zb _CanEditMultipleObjects_

```c#
[Range (0, 100)]
public int intValue = 50;
```
Property drawer für int Slider