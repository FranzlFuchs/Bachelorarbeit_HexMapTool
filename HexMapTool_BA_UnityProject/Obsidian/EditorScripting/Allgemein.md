* Makro \#UNITY_EDITOR  für if verwenden, damit Script in Editor Build includiert wird zum (muss dann nicht in ```Editor ``` Folder sein [[Beispiele#UNITY_EDITOR]]

 * Script sollte ansonsten in ````Editor```` Folder (egal wo im Projekt)

* UnityEditor Namespace verwenden 
```c#
using UnityEditor;
```

* Gizmo = visuelle Hilfe im Scene View, können für jede Szene in Gizmo Dropdown aktiviert werden, erscheint wenn Funktion im Projekt implementiert ist [[Beispiele#Gismo]]

* Gizmo Farbe sollte immer zurückgesetzt werden
```c#
private void OnDrawGizmos() 
{
	Color oldColor = Gizmos.color;
	//Something soemthing
	Gizmos.color = oldColor;
}
```
* damit gezeichnete Gizmos sich aktualisieren, muss die Gizmo Matrix aktualisiert werden!

* Analog zu Awake, Update, and OnDestroy Methoden in Monobehaviour,
 haben Inspektoren ähnliche Funktionen von Editor geerbt

```c#
private void OnEnable() // Wenn Objekt angeklickt

private void OnDisable() // Wenn Objekt Out of Scope

private void OnDestroy() // Wenn Objekt destroyed

```

Editor hat auch eine Variable _target_ = das inpizierte Objekt

Editor Windows können Customized werden


* Property Drawer - wie werden Variablen in Inspektor angezeigt (ohne ganzen Custom Inspektor)
```c#
public class DrawerExample : MonoBehaviour {
	[Range (0, 100)]
	public int intValue = 50;
}
```

Editor Window  kann customized werden
gleiche Funktionen wie Editor
```c#
private void OnEnable() // Wenn Objekt angeklickt

private void OnDisable() // Wenn Objekt Out of Scope

private void OnDestroy() // Wenn Objekt destroyed

```

Scene View kann customized werden