using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HexMap3DCreator
{
    [CustomEditor(typeof(HexMap3D))]
    public class MapInspector : Editor
    {
        private HexMap3D _target;

        private float _tileHeight = 0.0f;

        private GameObject _mouseOverObject;

        private PaletteItem _itemSelected;

        private Texture2D _itemSelectedTex;

        private Texture2D _helpIcon;

        private HexMapTile _pieceSelected;

        private List<GameObject> _mapTiles = new List<GameObject>();


        public void FocusOnMap()
        {
            Selection.activeGameObject = _target.gameObject;
        }

        private void DrawPreviewGUI()
        {
            GUIStyle previewStyle = new GUIStyle(GUI.skin.box);

            Handles.BeginGUI();

            GUILayout.BeginArea(new Rect(10f, 110f, 150f, 400f));

            if (_itemSelected == null)
            {
                GUILayout
                    .Label("No Item selected.\nOpen the palette (Tools/MapCreator/Open HexMap3D Palette) and choose a tile.",
                    previewStyle);
            }
            else
            {
                GUILayout
                    .Label("selected Tile: " + _itemSelected.ItemName,
                    previewStyle,
                    GUILayout.Width(135f));

                GUILayout.Label(_itemSelectedTex, previewStyle);
            }
            GUILayout.EndArea();
            Handles.EndGUI();
        }
        private void DrawTooltipGUI()
        {

            Handles.BeginGUI();

            GUILayout.BeginArea(new Rect(10f, 285f, 100f, 100f));

            if (_itemSelected != null)
            {
                GUILayout
                .Label(new GUIContent(_helpIcon, "Left Click:   Place Tile\nRight Click: Delete Tile\n\nThe transparent tiles are possible locations. To extend the map use the menu above"));
            }

            GUILayout.EndArea();
            Handles.EndGUI();
        }

        private void DrawGUI()
        {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.box);
            guiStyle.normal.textColor = Color.white;
            Handles.BeginGUI();

            GUILayout.BeginArea(new Rect(10f, 10f, 70f, 400f));

            GUILayout
                .Label("Rows",
                guiStyle,
                GUILayout.Height(EditorGUIUtility.singleLineHeight),
                GUILayout.Width(60f));
            string returnLength;
            int returnLengthnum;

            returnLength =
            GUILayout
            .TextArea(_target.GridLength.ToString(),
            guiStyle,
            GUILayout.Height(EditorGUIUtility.singleLineHeight),
            GUILayout.Width(60f));

            returnLengthnum = System.Int32.Parse(returnLength);


            if (returnLengthnum != _target.GridLength)
            {
                _target.GridLength = returnLengthnum;
                _target.RedrawGridTransparentTiles();
            }


            GUILayout
                .Label("Columns",
                guiStyle,
                GUILayout.Height(EditorGUIUtility.singleLineHeight),
                GUILayout.Width(60f));

            string returnWidth;
            int returnWidthnum;

            returnWidth =
            GUILayout
       .TextArea(_target.GridWidth.ToString(),
       guiStyle,
       GUILayout.Height(EditorGUIUtility.singleLineHeight),
       GUILayout.Width(60f));

            returnWidthnum = System.Int32.Parse(returnWidth);


            if (returnWidthnum != _target.GridWidth)
            {
                _target.GridWidth = returnWidthnum;
                _target.RedrawGridTransparentTiles();
            }

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(80f, 10f, 30f, 400f));

            bool buttonAddRow =
                GUILayout
                    .Button("+",
                    GUILayout.Height(EditorGUIUtility.singleLineHeight * 2 + 5f));
            if (buttonAddRow)
            {
                {
                    _target.AddLine(HexMap3D.LineType.row);
                }
            }

            bool buttonAddColumn =
                GUILayout
                    .Button("+",
                    GUILayout.Height(EditorGUIUtility.singleLineHeight * 2 + 5f));

            if (buttonAddColumn)
            {
                {
                    _target.AddLine(HexMap3D.LineType.column);
                }
            }

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(120f, 10f, 30f, 400f));

            bool buttonRemoveRow =
                GUILayout
                    .Button("-",
                    GUILayout.Height(EditorGUIUtility.singleLineHeight * 2 + 5f));
            if (buttonRemoveRow)
            {
                {
                    _target.RemoveLine(HexMap3D.LineType.row);
                }
            }

            bool buttonRemoveColumn =
                GUILayout
                    .Button("-",
                    GUILayout.Height(EditorGUIUtility.singleLineHeight * 2 + 5f));

            if (buttonRemoveColumn)
            {
                {
                    _target.RemoveLine(HexMap3D.LineType.column);
                }
            }
            GUILayout.EndArea();

            Handles.EndGUI();
        }

        private void DrawExportButtonGUI()
        {

            int width = Screen.width;
            int height = Screen.height;

            GUILayout.BeginArea(new Rect(width - 240f, height - 70f, 110f, 50f));
            _target.MapName = GUILayout.TextArea(_target.MapName, GUILayout.Height(EditorGUIUtility.singleLineHeight));

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(width - 120f, height - 70f, 110f, 50f));


            bool buttonExport = GUILayout.Button("Export Map", GUILayout.Height(EditorGUIUtility.singleLineHeight));

            if (buttonExport)
            {
                {
                    MakeMapPrefab();
                }
            }
            GUILayout.EndArea();
            Handles.EndGUI();

        }

        private void EventHandler()
        {
            HandleUtility
                .AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Vector3 mousePosition = Event.current.mousePosition;

            Ray worldRay =
                HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;

            // Shoot this ray. check in a distance of 10000.
            if (Physics.Raycast(worldRay, out hitInfo, 10000))
            {
                _mouseOverObject = hitInfo.collider.gameObject;
            }
            else
            {
                _mouseOverObject = null;
            }



            if (Event.current.type == EventType.MouseUp)
            {
                //left Mouse
                if (Event.current.button == 0)
                {
                    StackSelectedTile();
                }

                //right Mouse
                if (Event.current.button == 1)
                {
                    DeleteMouseOverTile();
                }
            }
        }

        private void OnSceneGUI()
        {
            DrawGUI();
            DrawPreviewGUI();
            DrawTooltipGUI();
            DrawExportButtonGUI();
            EventHandler();
        }

        private void OnEnable()
        {
            _target = (HexMap3D)target;
            string path = "Assets/Fundamentals/Icons/Help.png";
            _helpIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));

            _target.InitResources();
            SubscribeEvents();
        }

        private void OnDisable()
        {

            UnsubscribeEvents();
            //FocusOnMap();
        }

        private void SubscribeEvents()
        {
            PaletteWindow.ItemSelectedEvent +=
                new PaletteWindow.itemSelectedDelegate(UpdateCurrentPieceInstance);
        }

        private void UnsubscribeEvents()
        {
            PaletteWindow.ItemSelectedEvent -=
                new PaletteWindow.itemSelectedDelegate(UpdateCurrentPieceInstance);
        }

        public override void OnInspectorGUI()
        {
            DrawRawTileGUI();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(_target);
            }
        }

        private void DrawRawTileGUI()
        {
            _target.RawTileGO =
                (GameObject)
                EditorGUILayout
                    .ObjectField("Raw Tile Prefab / Game Object",
                    _target.RawTileGO,
                    typeof(GameObject),
                    false);

            if (_target.RawTileGO == null)
            {
                EditorGUILayout
                    .HelpBox("No raw Tile attached!", MessageType.Info);
            }
            else
            {
                Texture2D preview =
                    AssetPreview.GetAssetPreview(_target.RawTileGO.gameObject);
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout
                    .LabelField(new GUIContent(preview), GUILayout.Height(40));
                EditorGUILayout.LabelField("Raw Tile");
                EditorGUILayout.EndVertical();
            }
        }

        private void GetTileHeight()
        {
            if (_tileHeight == 0.0f)
            {
                Renderer renderer = _mouseOverObject.GetComponent<Renderer>();
                Vector3 tilesize = renderer.bounds.size;
                _tileHeight = tilesize.y;
            }
        }

        GameObject
        InstantiateHexMapTile(Vector3 position, List<HexMapTile.TileType> types)
        {
            GameObject obj =
                PrefabUtility.InstantiatePrefab(_pieceSelected.gameObject) as
                GameObject;
            _mapTiles.Add(_mouseOverObject.gameObject);
            obj.transform.SetParent(_target.gameObject.transform);

            obj.gameObject.AddComponent<HexMapTile>();
            obj.gameObject.GetComponent<HexMapTile>().TileTypes.AddRange(types);

            obj.transform.position = position;
            obj.name = _itemSelected.ItemName;

            obj.hideFlags = HideFlags.HideInHierarchy;
            obj.gameObject.AddComponent<MeshCollider>();
            obj.gameObject.GetComponent<MeshCollider>().convex = true;

            if (obj.gameObject.GetComponent<Rigidbody>() == null)
            {
                obj.gameObject.AddComponent<Rigidbody>();
            }

            return obj;
        }

        void StackOnPlaceHolder()
        {
            Vector3 position = _mouseOverObject.transform.position;
            InstantiateHexMapTile(position,
            new List<HexMapTile.TileType> {
                HexMapTile.TileType.Bottom,
                HexMapTile.TileType.Top
            });
            DestroyImmediate(_mouseOverObject);
        }

        void StackOnTop()
        {
            Vector3 position =
                new Vector3(_mouseOverObject.transform.position.x,
                    _mouseOverObject.transform.position.y + _tileHeight,
                    _mouseOverObject.transform.position.z);

            GameObject obj =
                InstantiateHexMapTile(position,
                new List<HexMapTile.TileType> { HexMapTile.TileType.Top });


            HexMapTile mouseOverTile =
                    _mouseOverObject.gameObject.GetComponent<HexMapTile>();
            mouseOverTile.TileTypes.Remove(HexMapTile.TileType.Top);

            HexMapTile placedTile = obj.gameObject.GetComponent<HexMapTile>();
            placedTile.TileBeneath = mouseOverTile;
        }

        public void StackSelectedTile()
        {
            if (_mouseOverObject == null || _pieceSelected == null)
            {
                return;
            }
            GetTileHeight();

            HexMapTile mouseOverTile =
                _mouseOverObject.gameObject.GetComponent<HexMapTile>();

            if (
                mouseOverTile != null &&
                mouseOverTile.TileTypes.Contains(HexMapTile.TileType.Top)
            )
            {
                StackOnTop();
            }

            if (IsPlaceholer(_mouseOverObject.gameObject))
            {
                StackOnPlaceHolder();
            }
        }

        public void DeleteMouseOverTile()
        {
            if (_mouseOverObject == null || _pieceSelected == null)
            {
                return;
            }

            if (IsPlaceholer(_mouseOverObject.gameObject))
            {
                return;
            }

            HexMapTile tile =
                _mouseOverObject.gameObject.GetComponent<HexMapTile>();

            if (tile == null)
            {
                return;
            }

            if (tile.TileTypes.Contains(HexMapTile.TileType.Bottom))
            {
                _target
                    .PlaceTransparentTile(_mouseOverObject
                        .gameObject
                        .transform
                        .position);
                DestroyImmediate(_mouseOverObject.gameObject);
                _mapTiles.Remove(_mouseOverObject.gameObject);
            }

            if (tile.TileTypes.Contains(HexMapTile.TileType.Top))
            {
                if (tile.TileBeneath != null)
                {
                    tile.TileBeneath.TileTypes.Add(HexMapTile.TileType.Top);
                }
                DestroyImmediate(_mouseOverObject.gameObject);
                _mapTiles.Remove(_mouseOverObject.gameObject);

            }
        }

        void MakeMapPrefab()
        {
            bool success;
            PrefabUtility.SaveAsPrefabAsset(_target.gameObject, "Assets/Prefabs/Maps/" + _target.MapName + ".prefab", out success);

        }

        private void UpdateCurrentPieceInstance(
            PaletteItem item,
            Texture2D preview
        )
        {
            _itemSelected = item;
            _itemSelectedTex = preview;

            _pieceSelected = (HexMapTile)item.GetComponent<HexMapTile>();
            SceneView.RepaintAll();
        }

        private bool IsPlaceholer(GameObject go)
        {
            HexMapTile tile = go.GetComponent<HexMapTile>();
            if (tile != null)
            {
                if (tile.TileTypes.Contains(HexMapTile.TileType.Placeholder))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
