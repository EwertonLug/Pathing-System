using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateWaypoints))]
public class CreateWaypointsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CreateWaypoints myScript = (CreateWaypoints)target;
        if(GUILayout.Button("Criate Root"))
        {
            myScript.CreateRoot();
        }
    }
}
