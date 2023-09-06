using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapTile : MonoBehaviour
{

    public List<TileType> TileTypes = new List<TileType>();    
    public HexMapTile TileBeneath = null;    
    public enum TileType
    {
        Placeholder,
        Bottom,
        Top
    }


}
