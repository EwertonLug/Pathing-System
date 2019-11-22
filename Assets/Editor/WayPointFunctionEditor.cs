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
        if (GUILayout.Button("Create Neighbor "))
        {
            myScript.CreateNeighbor();
        }
    }
    void OnSceneGUI()
    {
        WayPointFunction myScript = (WayPointFunction)target;
        myScript.UpdateStateWayPoint();
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontStyle = FontStyle.Bold;
        Handles.Label(myScript.transform.position, "Selected", style);
      
    }
   

}
