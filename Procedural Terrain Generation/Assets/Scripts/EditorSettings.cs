using UnityEngine;
using System.Collections;
using UnityEditor;

// Will inherit from editor class
[CustomEditor (typeof (MapGenerator))]

public class EditorSettings : Editor {

    public override void OnInspectorGUI()
    {
        // We need here a reference to the Map Generator class
        // target here is object that this custom editor is inspecting 
        // and cast that object to a map generator
        MapGenerator mg = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (mg.updateNoiseMap)
            {
                mg.mapGen();
            }
        }

        if(GUILayout.Button("GenerateMap"))
        {
            mg.mapGen();
        }
    }
}
