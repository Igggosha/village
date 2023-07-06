using System;
using MapGeneration;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            MapGenerator mapGenerator = (MapGenerator)target;
            
            if( DrawDefaultInspector())
            {
                if (mapGenerator.autoUpdate)
                {
                    mapGenerator.GenerateNoiseMap();
                }
            }
            
            if (GUILayout.Button("Generate"))
            {
                mapGenerator.GenerateNoiseMap();
            }
        }
    }
}
