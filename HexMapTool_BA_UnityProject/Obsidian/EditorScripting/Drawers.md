## Property Drawers

### Range
Slider für int und float mit Min und Max Werten

```c#
// Method signatures
// public RangeAttribute(float min, float max);
// public RangeAttribute(int min, int max);
[Range (0, 1)]
public float floatRange = 0.5f;
[Range (0, 100)]
public int intRange = 50;
```

### Multiline
Mehrzeiliger Textfeld für strings, Zeilananzahl ist definierbar
Textfeld wrapped nicht um Eingabe

```c#
// Method signatures
// public MultilineAttribute();
// public MultilineAttribute(int lines);
[Multiline (2)]
public string stringMultiline = "This text is using a multiline
property drawer";
```

### TextArea
Textfeld höhenverstellbar und mit Scrollbar für strings, min und max Zeilenanzahl konfigurierbar

``` c#
// Method signatures
// public TextAreaAttribute();
// public TextAreaAttribute(int minLines, int maxLines);
[TextArea (2, 4)]
public string stringTextArea = "This text \nis using \na text area \
nproperty \ndrawer";
```

### ContextMenu
Macht eine Methode sichtbar, wird mit auswählen im Komponeten Menü (kleines Zahnrad rechts oben) ausgeführt.

```c#
// Method signature
// public ContextMenuAttribute(string name);
[ContextMenu ("Do Something")]
public void DoSomething() {
Debug.Log ("DoSomething was called...");
}
```

### ContextMenuItem
Macht eine Methode sichtbar, wird mit auswählen im Komponeten Menü eines Property (RK auf Property) ausgeführt.

```c#
// Method signature
// public ContextMenuItemAttribute(string name, string function);
[ContextMenuItem("Reset this value", "Reset")]
public int intReset = 100;
public void Reset() {
intReset = 0;
```