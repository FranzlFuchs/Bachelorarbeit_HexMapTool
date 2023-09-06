using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HexMap3DCreator
{
    public class HexMap3D : MonoBehaviour
    {
        /// <summary>
        /// COLUMS
        /// </summary>
        private int _gridWidth = 3;

        /// <summary>
        /// COLUMS
        /// </summary>
        public int GridWidth
        {
            get
            {
                return _gridWidth;
            }
            set
            {
                _gridWidth = value;

            }
        }

        /// <summary>
        /// ROWS
        /// </summary>
        private int _gridLength = 3;

        /// <summary>
        /// ROWS
        /// </summary>
        public int GridLength
        {
            get
            {
                return _gridLength;
            }
            set
            {
                _gridLength = value;
            }
        }


        private string _mapName;

        public string MapName
        {
            get
            {
                return _mapName;
            }
            set
            {
                _mapName = value;
            }
        }


        [SerializeField]
        private float _tileOuterRadius = 0.0f;

        private readonly Color _lineColor = new Color(249, 245, 245, 0.05f);

        [SerializeField]
        public GameObject _rawTileGO;

        public GameObject RawTileGO
        {
            get
            {
                return _rawTileGO;
            }
            set
            {
                _rawTileGO = value;
            }
        }

        private Material _transparentMaterial;

        public GameObject _rawTileGOTransparent;

        private List<GameObject> _mapTilePlaceholders;

        public void InitResources()
        {
            InitRawTile();
            InitTransparentTile();
            DrawGridTransparentTiles(_gridLength, _gridWidth);
        }
        public void RedrawGridTransparentTiles()
        {

            foreach (GameObject go in _mapTilePlaceholders)
            {
                DestroyImmediate(go);
            }
            _mapTilePlaceholders.Clear();

            DrawGridTransparentTiles(_gridLength, _gridWidth);
        }



        void InitTransparentTile()
        {
            if (_rawTileGOTransparent != null)
            {
                return;
            }

            _rawTileGOTransparent = RawTileGO;
            _transparentMaterial =
                AssetDatabase
                    .LoadAssetAtPath("Assets/Fundamentals/Materials/Transparent.mat",
                    typeof(Material)) as
                Material;
            _rawTileGOTransparent.GetComponent<Renderer>().material =
                _transparentMaterial;
        }

        void InitRawTile()
        {
            _rawTileGO =
                AssetDatabase
                    .LoadAssetAtPath("Assets/Fundamentals/Prefabs/TileRaw.prefab",
                    typeof(GameObject)) as
                GameObject;
        }

        public void PlaceTransparentTile(Vector3 position)
        {
            GameObject newTile =
                Instantiate(_rawTileGOTransparent,
                position,
                _rawTileGOTransparent.transform.rotation);
            newTile.name = "Placeholder";
            newTile.tag = "Placeholder";
            newTile.AddComponent<HexMapTile>();
            newTile
                .GetComponent<HexMapTile>()
                .TileTypes
                .Add(HexMapTile.TileType.Placeholder);

            newTile.hideFlags = HideFlags.HideInHierarchy;


            if (_mapTilePlaceholders == null)
            {
                _mapTilePlaceholders = new List<GameObject>();
            }
            _mapTilePlaceholders.Add(newTile);
        }

        void DrawGizmoMesh()
        {
            foreach (GameObject go in _mapTilePlaceholders)
            {
                MeshFilter meshfilter = go.GetComponent<MeshFilter>();
                Mesh mesh = meshfilter.mesh;
                mesh.RecalculateNormals();
                Gizmos
                    .DrawWireMesh(mesh,
                    -1,
                    go.transform.position,
                    go.transform.rotation);
            }
        }

        void GetTileDimensions()
        {
            Renderer renderer = _rawTileGO.GetComponent<Renderer>();
            Vector3 tilesize = renderer.bounds.size;
            _tileOuterRadius = Mathf.Max(tilesize.x / 2, tilesize.z / 2);
        }

        //GIZMOS
        private void OnDrawGizmos()
        {
            Color oldColor = Gizmos.color;
            Matrix4x4 oldMatrix = Gizmos.matrix;

            Gizmos.color = _lineColor;

            if (_rawTileGO != null)
            {
                GetTileDimensions();
                DrawGridGizmos();
                //DrawGizmoMesh();
            }
            Gizmos.matrix = oldMatrix;
            Gizmos.color = oldColor;
        }

        public Vector3 RotateVectorY(Vector3 vector, float angle)
        {
            return Quaternion.AngleAxis(angle, Vector3.up) * vector;
        }

        public void DrawHexagon(Vector3 center, Vector3 offset)
        {
            Vector3 v1 = center + new Vector3(0, 0, _tileOuterRadius);

            //v1 = RotateVectorY(v1, 30);
            Vector3 v2;
            Vector3 v3;

            for (int i = 0; i < 6; i++)
            {
                v3 = v1;
                v2 = RotateVectorY(v1, -60);
                v2 = v2 + offset;
                v3 = v3 + offset;
                Gizmos.DrawLine(v2, v3);
                v1 = v2 - offset;
            }
        }

        public void DrawGridGizmos()
        {
            float lineZOffset = 0;
            float tileInnerRadius = _tileOuterRadius * (Mathf.Sqrt(3f) / 2f);

            for (int i = 0; i < _gridWidth; i++)
            {
                lineZOffset = 0;
                if (i % 2 == 0)
                {
                    lineZOffset = tileInnerRadius;
                }

                //bei ungeraden versetzen
                for (int j = 0; j < _gridLength; j++)
                {
                    DrawHexagon(new Vector3(0, 0, 0),
                    new Vector3(j * 2 * tileInnerRadius + lineZOffset,
                        0,
                        (_tileOuterRadius / 2 + _tileOuterRadius) * i));
                }
            }
        }

        public enum LineType
        {
            row,
            column
        }

        public void AddLine(LineType line)
        {
            PlaceLineTransparentTiles(line);

            if (line == LineType.row)
            {
                _gridLength++;
            }

            if (line == LineType.column)
            {
                _gridWidth++;
            }
        }

        public void RemoveLine(LineType line)
        {
            if (line == LineType.row)
            {
                if (_gridLength <= 0)
                {
                    return;
                }

                _gridLength--;
            }

            if (line == LineType.column)
            {
                if (_gridWidth <= 0)
                {
                    return;
                }

                _gridWidth--;
            }

            RemoveLineTransparentTiles(line);
        }

        public bool HasTile(Vector3 position)
        {
            if (
                Physics.Raycast(position, transform.TransformDirection(Vector3.up), out RaycastHit hit, 101)
            )
            {
                if (hit.transform.gameObject.GetComponent<HexMapTile>() != null)
                {
                    return true;
                }
            }

            return false;
        }

        public void DrawGridTransparentTiles(int rows, int columns)
        {
            if (_rawTileGOTransparent == null)
            {
                InitTransparentTile();
                if (_rawTileGOTransparent == null)
                {
                    return;
                }
            }
            GetTileDimensions();

            float tileInnerRadius = _tileOuterRadius * (Mathf.Sqrt(3f) / 2f);

            for (int i = 0; i < columns; i++)
            {
                float lineZOffset = 0;
                if (i % 2 == 0)
                {
                    lineZOffset = tileInnerRadius;
                }

                //bei ungeraden versetzen
                for (int j = 0; j < rows; j++)
                {
                    Vector3 position = new Vector3(j * 2 * tileInnerRadius + lineZOffset, 0, (_tileOuterRadius / 2 + _tileOuterRadius) * i);

                    if (!HasTile(position))
                    {
                        PlaceTransparentTile(position);

                    }
                }
            }
        }

        public void PlaceLineTransparentTiles(LineType lineType)
        {
            if (_rawTileGOTransparent == null)
            {
                InitTransparentTile();
                if (_rawTileGOTransparent == null)
                {
                    return;
                }
            }


            float lineZOffset = 0;
            float sideLength = _tileOuterRadius / (Mathf.Sqrt(3f) / 2f);
            float tileInnerRadius =
                _tileOuterRadius * (Mathf.Sqrt(3f) / 2f);

            float numberOfTiles;
            float offsetOtherLine;
            Vector3 position;

            if (lineType == LineType.column)
            {
                numberOfTiles = _gridLength;
                offsetOtherLine = _gridWidth;

                lineZOffset = 0;
                if (offsetOtherLine % 2 == 0)
                {
                    lineZOffset = tileInnerRadius;
                }

                for (int j = 0; j < numberOfTiles; j++)
                {
                    position =
                        new Vector3(j * 2 * tileInnerRadius + lineZOffset,
                            0,
                            (_tileOuterRadius / 2 + _tileOuterRadius) *
                            offsetOtherLine);

                    if (!HasTile(position))
                    {
                        PlaceTransparentTile(position);
                    }
                }
            }
            else
            {
                numberOfTiles = _gridWidth;
                offsetOtherLine = _gridLength;

                for (int j = 0; j < numberOfTiles; j++)
                {
                    lineZOffset = 0;
                    if (j % 2 == 0)
                    {
                        lineZOffset = tileInnerRadius;
                    }

                    position =
                        new Vector3((
                            (2 * tileInnerRadius) * offsetOtherLine
                            ) +
                            lineZOffset,
                            0,
                            (_tileOuterRadius + (_tileOuterRadius / 2)) *
                            j);

                    if (!HasTile(position))
                    {
                        PlaceTransparentTile(position);
                    }
                }
            }

        }

        public void RemoveLineTransparentTiles(LineType lineType)
        {
            float lineZOffset = 0;
            float sideLength = _tileOuterRadius / (Mathf.Sqrt(3f) / 2f);
            float tileInnerRadius = _tileOuterRadius * (Mathf.Sqrt(3f) / 2f);

            float numberOfTiles;
            float offsetOtherLine;
            Vector3 position;
            GameObject tile;
            if (lineType == LineType.column)
            {
                numberOfTiles = _gridLength;
                offsetOtherLine = _gridWidth;

                lineZOffset = 0;
                if (offsetOtherLine % 2 == 0)
                {
                    lineZOffset = tileInnerRadius;
                }

                for (int j = 0; j < numberOfTiles; j++)
                {
                    position =
                        new Vector3(j * 2 * tileInnerRadius + lineZOffset,
                            0,
                            (_tileOuterRadius / 2 + _tileOuterRadius) *
                            offsetOtherLine);

                    tile = GetPlaceHolder(position);
                    if (tile != null)
                    {
                        DestroyImmediate(tile);
                    }
                }
            }
            else
            {
                numberOfTiles = _gridWidth;
                offsetOtherLine = _gridLength;

                for (int j = 0; j < numberOfTiles; j++)
                {
                    lineZOffset = 0;
                    if (j % 2 == 0)
                    {
                        lineZOffset = tileInnerRadius;
                    }

                    position =
                        new Vector3((2 * tileInnerRadius * offsetOtherLine) +
                            lineZOffset,
                            0,
                            (_tileOuterRadius + (_tileOuterRadius / 2)) * j);

                    tile = GetPlaceHolder(position);
                    if (tile != null)
                    {
                        DestroyImmediate(tile);
                    }
                }
            }
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

        GameObject GetPlaceHolder(Vector3 position)
        {
            if (Physics.Raycast(position, transform.TransformDirection(Vector3.up), out RaycastHit hit, 101))
            {
                if (IsPlaceholer(hit.transform.gameObject))
                {
                    return hit.transform.gameObject;
                }
            }

            return null;
        }
    }
}
