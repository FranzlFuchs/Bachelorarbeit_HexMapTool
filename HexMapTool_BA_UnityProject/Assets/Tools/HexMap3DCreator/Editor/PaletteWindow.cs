using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HexMap3DCreator
{
    public class PaletteWindow : EditorWindow
    {
        public static PaletteWindow instance;

        private string _pathPrefabs;


        private string _pathMaterials = "Assets/Materials/Tiles";

        private string _pathTileRaw = "Assets/Fundamentals/Prefabs/TileRaw.prefab";
        private string _warning;

        private List<PaletteItem> _items = new List<PaletteItem>();

        public delegate void itemSelectedDelegate(PaletteItem item, Texture2D preview);

        public static event itemSelectedDelegate ItemSelectedEvent;

        private Dictionary<PaletteItem, Texture2D> _previews;

        public PaletteItem _selectedItem = null;

        private Vector2 _scrollPosition;

        private const float ButtonWidth = 80;

        private const float ButtonHeight = 90;

        public static void ShowPalette()
        {
            instance =
                (PaletteWindow)EditorWindow.GetWindow(typeof(PaletteWindow));
            instance.titleContent = new GUIContent("HexMap3D Palette");

        }

        private void OnEnable()
        {

            InitContent();

        }

        private GUIContent[] GetGUIContentsFromItems()
        {
            if (_previews == null)
            {
                _previews = new Dictionary<PaletteItem, Texture2D>();
            }

            List<GUIContent> guiContents = new List<GUIContent>();
            if (_previews.Count == _items.Count)
            {
                int totalItems = _items.Count;
                for (int i = 0; i < totalItems; i++)
                {
                    GUIContent guiContent = new GUIContent();
                    guiContent.text = _items[i].ItemName;
                    guiContent.image = _previews[_items[i]];
                    guiContents.Add(guiContent);
                }
            }
            return guiContents.ToArray();
        }

        private void GetSelectedItem(int index)
        {
            if (index != -1)
            {
                PaletteItem selectedItem = _items[index];
                if (ItemSelectedEvent != null)
                {
                    ItemSelectedEvent(selectedItem, _previews[selectedItem]);
                }

                _selectedItem = selectedItem;
            }
        }

        private GUIStyle GetGUIStyleSelectionGrid()
        {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
            guiStyle.alignment = TextAnchor.LowerCenter;
            guiStyle.imagePosition = ImagePosition.ImageAbove;
            guiStyle.fixedWidth = ButtonWidth;
            guiStyle.fixedHeight = ButtonHeight;
            return guiStyle;
        }

        private GUIStyle GetGUIStyleLabel()
        {
            GUIStyle guiStyleLabel = new GUIStyle(GUI.skin.label);

            guiStyleLabel.fontSize = 22;
            guiStyleLabel.fontStyle = FontStyle.Bold;
            guiStyleLabel.alignment = TextAnchor.UpperLeft;

            return guiStyleLabel;
        }

        private GUIStyle GetGUIStyleText()
        {
            GUIStyle guiStyleText = new GUIStyle(GUI.skin.label);

            guiStyleText.fontSize = 14;
            guiStyleText.alignment = TextAnchor.UpperLeft;
            guiStyleText.wordWrap = true;

            return guiStyleText;
        }

        private void InitContent()
        {
            _pathPrefabs = "Assets/Prefabs/Tiles";
            _pathMaterials = "Assets/Materials/Tiles";
            _pathTileRaw = "Assets/Fundamentals/Prefabs";
            try
            {





                GameObject tileRaw =
                    Utilities
                        .GetGameObjectFromAssets(_pathTileRaw,
                        "TileRaw.prefab");


                DirectoryInfo dirMats = new DirectoryInfo(_pathMaterials);
                DirectoryInfo dirPref = new DirectoryInfo(_pathPrefabs);

                FileInfo[] infoMats = dirMats.GetFiles("*.*");
                FileInfo[] infoPrefs = dirPref.GetFiles("*.*");



                foreach (FileInfo f in infoMats)
                {
                    string newPrefabName = Utilities.RemoveFileExtension(f.Name);


                    //string newPrefabName = f.Name.Substring(0, f.Name.Length - 4);

                    foreach (FileInfo pref in infoPrefs)
                    {
                        //Check ob prefab bereits existiert
                        //string prefName = pref.Name.Substring(0, f.Name.Length - 8);
                        string prefName = Utilities.RemoveFileExtension(pref.Name);
                        if (prefName == newPrefabName)
                        {
                            continue;
                        }
                    }

                    if (f.Extension == ".mat")
                    {
                        Material mat = Utilities.GetMaterialFromAssets(_pathMaterials, f.Name);

                        GameObject tileRawInst = Instantiate(tileRaw);
                        tileRawInst.hideFlags = HideFlags.HideInHierarchy;

                        GameObject prefab =
                            PrefabUtility
                                .SaveAsPrefabAsset(tileRawInst,
                                _pathPrefabs + "/" +
                                newPrefabName +
                                ".prefab");
                        prefab.gameObject.GetComponent<Renderer>().material = mat;
                        prefab.gameObject.GetComponent<PaletteItem>().ItemName =
                            newPrefabName;

                        DestroyImmediate(tileRawInst);
                    }
                }

                _items = Utilities.GetAssetsWithScript<PaletteItem>(_pathPrefabs);

                _previews = new Dictionary<PaletteItem, Texture2D>();
                _warning = "";
            }
            catch (Exception ex)
            {
                _warning = "Something is wrong with the materials in " + _pathMaterials + "\n" +
                            "Check for\n" +
                            "- Length of material name\n" +
                            "- Special characters in of material name\n\n" +
                            "Error Message:\n" +
                            ex;
                return;

            }

        }


        private void DrawWarning()
        {
            if (_warning == "" || _warning == null)
            {
                return;
            }

            EditorGUILayout.HelpBox(_warning, MessageType.Warning);




        }

        private void DrawRefreshButton()
        {
            EditorGUILayout.BeginVertical("button");

            bool refreshButton = GUILayout.Button("Refresh");
            if (refreshButton)
            {
                RefreshPaletteItems();
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawAddTiles()
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout
                .LabelField("Adding Tiles",
                GetGUIStyleLabel(),
                GUILayout.Height(45f));

            EditorGUILayout
                .TextArea("To add tiles put materials in the " +
                _pathMaterials +
                " folder and hit the refresh button above \n \n",
                GetGUIStyleText());
            EditorGUILayout.EndVertical();
        }

        private void DrawScroll()
        {
            if (_items.Count == 0)
            {
                EditorGUILayout
                    .HelpBox("There is no raw Tile! Please add a Prefabs in the " +
                    _pathTileRaw +
                    " folder to use as raw tile.",
                    MessageType.Info);

                return;
            }
            int rowCapacity = Mathf.FloorToInt(position.width / (ButtonWidth));
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            int selectionGridIndex = -1;
            selectionGridIndex =
                GUILayout
                    .SelectionGrid(selectionGridIndex,
                    GetGUIContentsFromItems(),
                    rowCapacity,
                    GetGUIStyleSelectionGrid());
            GetSelectedItem(selectionGridIndex);
            GUILayout.EndScrollView();
        }

        void RefreshPaletteItems()
        {
            InitContent();
            ResetPreviews();
            GeneratePreviews();
            Repaint();
        }
        private void ResetPreviews()
        {
            if (_previews != null)
            {
                _previews.Clear();

            }
        }
        private void GeneratePreviews()
        {
            foreach (PaletteItem item in _items)
            {
                if (!_previews.ContainsKey(item))
                {
                    Texture2D preview =
                        AssetPreview.GetAssetPreview(item.gameObject);
                    if (preview != null)
                    {
                        _previews.Add(item, preview);
                    }
                }
            }
        }

        private void AddComponentsToItems()
        {
            foreach (PaletteItem item in _items)
            {
                if (!_previews.ContainsKey(item))
                {
                    item.gameObject.AddComponent<HexMapTile>();
                }
            }
        }

        private void OnDisable()
        {
        }

        private void OnDestroy()
        {
        }

        private void OnGUI()
        {
            DrawScroll();
            DrawWarning();
            DrawRefreshButton();
            DrawAddTiles();
        }

        private void Update()
        {
            if (_previews != null && _items != null)
            {
                if (_previews.Count != _items.Count)
                {
                    GeneratePreviews();
                    AddComponentsToItems();
                }
            }
        }
    }
}
