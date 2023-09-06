* LoadLevelAdditive(...) zerstört die alte GameObjects nicht wie LoadLevel(...), sondern ladet die neuen über die alten


* SerializedObject und SerializedProperty ermöglichen die Veränderung von Objekten auf einer generischen Ebene. SerializedObject ermöglicht den zugriff auf Propertes durch FindProperty ("xy") Methode

``` c#
private void InitLevel () {
	_mySerializedObject = new SerializedObject (_myTarget);
	_serializedTotalTime = _mySerializedObject.FindProperty ("_totalTime");
	if (_myTarget.Pieces == null || _myTarget.Pieces.Length == 0) 
	{
		Debug.Log("Initializing the Pieces array...");
		_myTarget.Pieces = new LevelPiece[ _myTarget.TotalColumns *_myTarget.TotalRows];
	}
}
```