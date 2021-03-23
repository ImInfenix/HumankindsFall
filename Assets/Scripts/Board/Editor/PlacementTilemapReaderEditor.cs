using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlacementTilemapReader))]
public class PlacementTilemapReaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate scene data"))
        {
            (target as PlacementTilemapReader)?.GenerateData();
        }
    }
}
