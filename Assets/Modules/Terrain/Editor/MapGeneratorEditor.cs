using System.Collections;
using System.Collections.Generic;
using FGWorms.Terrain;
using UnityEngine;
using UnityEditor;
using UnityEditor.TerrainTools;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var mapGen = (MapGenerator)target;
        DrawDefaultInspector();
        // if (GUILayout.Button("Generate"))
        // {
        //     mapGen.GenerateMap();
        // }
    }
}
