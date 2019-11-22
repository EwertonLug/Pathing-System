using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WayPointFunction))]
public class WayPointFunctionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WayPointFunction myScript = (WayPointFunction)target;
        if(GUILayout.Button("Create Neighbor "))
        {
            myScript.CreateNeighbor();
        }
    }
}
