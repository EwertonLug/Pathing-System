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
    [DrawGizmo(GizmoType.NonSelected)]
    static void OnDrawGizmoNonSelected(WayPointFunction way, GizmoType gizmoType)
    {
        var neighbors = way.GetComponent<WayPoint>().neighbors;
        if (neighbors == null)
            return;
        Handles.color = Color.yellow;
        Handles.DrawSolidDisc(way.transform.position, way.transform.forward, 0.1f);
        foreach (var neighbor in neighbors)
        {
            if (neighbor != null && neighbor.gameObject != Selection.activeGameObject)
            {

                Handles.color = Color.yellow;

                Handles.DrawDottedLine(way.transform.position, neighbor.transform.position, 5);
                Handles.DrawSolidDisc(neighbor.transform.position, neighbor.transform.forward, 0.1f);
            }
        }



    }
    [DrawGizmo(GizmoType.Selected)]
    static void OnDrawGizmoSelected(WayPointFunction way, GizmoType gizmoType)
    {
        var neighbors = way.GetComponent<WayPoint>().neighbors;
        Handles.color = Color.green;
        Handles.DrawSolidDisc(way.transform.position, way.transform.forward, 0.2f);
        Handles.color = Color.blue;
        Handles.DrawSolidDisc(way.transform.position, way.transform.forward, 0.1f);

        foreach (var neighbor in neighbors)
        {
            if (neighbor != null)
            {
                Handles.color = Color.blue;

                Handles.DrawLine(way.transform.position, neighbor.transform.position);
                Handles.DrawSolidDisc(neighbor.transform.position, neighbor.transform.forward, 0.2f);
            }
        }


    }
   

}
