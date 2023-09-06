using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexMap3DCreator
{
    public static class MenuItems
    {
        [MenuItem("Tools/Map Creator/new HexMap3D")]
        private static void NewMap()
        {
            Utilities.NewMap();
        }

        [MenuItem("Tools/Map Creator/HexMap3D Palette")]
        private static void OpenPalette()
        {
            PaletteWindow.ShowPalette();
        }
    }
}
