using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HexMap3DCreator
{
    public static class Utilities
    {
        public static void NewMap()
        {
            NewScene();
            RemoveGameObjects();
            InitMap();
        }


        public static string RemoveFileExtension(string file)
        {
            string fileWithoutExtension = "";
            int i = file.LastIndexOf(".");
            if (i > 0)
            {
                fileWithoutExtension = file.Remove(i);
            }

            return fileWithoutExtension;
        }

        private static void InitMap()
        {
            GameObject mapGO = new GameObject("HexMap3D");
            mapGO.transform.position = Vector3.zero;
            Selection.activeGameObject = mapGO;
            SceneView.FrameLastActiveSceneView();
            mapGO.AddComponent<HexMap3D>();

        }

        public static void NewScene()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
            newScene.name = "HexMap3D Creator Scene";

            RenderSettings.skybox = GetMaterialFromAssets("Assets/Fundamentals/Prefabs", "SkyBox.mat");
        }

        public static void RemoveGameObjects()
        {
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
            foreach (GameObject go in allObjects)
            {
                if (go.GetComponent<Light>() == null)
                {
                    Object.DestroyImmediate(go);
                }
                else
                {
                    go.hideFlags = HideFlags.HideInHierarchy;
                }
            }
        }

        public static List<T> GetAssetsWithScript<T>(string path)
            where T : MonoBehaviour
        {
            T tmp;
            string assetPath;
            GameObject asset;
            List<T> assetList = new List<T>();
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { path });
            for (int i = 0; i < guids.Length; i++)
            {
                assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
                tmp = asset.GetComponent<T>();
                if (tmp != null)
                {
                    assetList.Add(tmp);
                }
            }
            return assetList;
        }

        public static Material
        GetMaterialFromAssets(string path, string materialName)
        {
            string materialFileExtension = ".mat";
            Material material = (Material)AssetDatabase.LoadAssetAtPath<Material>(BuildAssetPath(path, materialName, materialFileExtension)) as Material;
            return material;
        }

        public static GameObject
        GetGameObjectFromAssets(string path, string prefabName)
        {
            string prefabFileExtension = ".prefab";
            GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath<GameObject>(BuildAssetPath(path, prefabName, prefabFileExtension)) as GameObject;
            return gameObject;
        }

        public static string
        BuildAssetPath(string path, string assetName, string fileExtension)
        {
            string fullpath;

            //Add Slash if not existing
            if (!path.EndsWith("/"))
            {
                path += "/";
            }

            //Add Extension if not existing
            if (!assetName.EndsWith(fileExtension))
            {
                assetName += fileExtension;
            }

            //Connect strings to path
            fullpath = path + assetName;

            return fullpath;
        }

        public static List<T> GetListFromEnum<T>()
        {
            List<T> enumList = new List<T>();
            System.Array enums = System.Enum.GetValues(typeof(T));
            foreach (T e in enums)
            {
                enumList.Add(e);
            }
            return enumList;
        }

        public static void MakePrefabFromList(List<GameObject> gameObjects)
        {
            //GameObject obj = new GameObject("HexMap3D");
            //PrefabUtility.CreatePrefab("Assets/Prefabs/Maps/Map.prefab", .gameObject);
        }
    }
}
