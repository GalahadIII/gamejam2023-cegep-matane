using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(UVCubes))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UVCubes myScript = (UVCubes)target;
        if (GUILayout.Button("Apply Texture"))
        {
            myScript.ApplyTexture();
        }
    }
}
