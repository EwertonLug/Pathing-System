using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
namespace PathSystem2D.Base
{
    [CustomEditor(typeof(WayPointFunction))]
    [CanEditMultipleObjects]
    public class WayPointFunctionEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            WayPointFunction myScript = (WayPointFunction)target;
            if (GUILayout.Button("Create Neighbor "))
            {
                var way = myScript.CreateNeighbor();
                Selection.SetActiveObjectWithContext(way, null);
                AssignLabel(way);
            }
        }

        /**
            sv_label_0
            sv_label_1
            sv_label_2
            sv_label_3
            sv_label_5
            sv_label_6
            sv_label_7
            sv_icon_none
            sv_icon_dot14_pix16_gizmo
        */
        public static void AssignLabel(GameObject g)
        {
            Texture2D tex = EditorGUIUtility.IconContent("sv_icon_dot12_pix16_gizmo").image as Texture2D;
            Type editorGUIUtilityType = typeof(EditorGUIUtility);
            BindingFlags bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
            object[] args = new object[] { g, tex };
            editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args);
        }
        void OnSceneGUI()
        {
            WayPointFunction myScript = (WayPointFunction)target;
            myScript.UpdateStateWayPoint();
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontStyle = FontStyle.Bold;

            Handles.Label(myScript.transform.position, "Selected", style);

            GUILayout.BeginArea(new Rect(10, 10, 150, 100), "Waypoints Functions");
            GUILayout.Label(" Waypoints Functions", style);
            if (GUILayout.Button("Create Neighbor", GUILayout.Width(120), GUILayout.Height(20)))
            {
                var way = myScript.CreateNeighbor();
                Selection.SetActiveObjectWithContext(way, null);
                AssignLabel(way);
            }
            
            if (GUILayout.Button("Is Walkable", GUILayout.Width(120), GUILayout.Height(20)))
            {
                myScript.WalkableChange();

            }

            GUILayout.EndArea();



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
            var wayPoint = way.GetComponent<WayPoint>();

            if (!wayPoint.isWalkable)
            {
                Handles.color = new Color(1, 0, 0, 0.1f);
                Handles.DrawSolidDisc(way.transform.position, way.transform.forward, 0.6f);
            }
        }
        [DrawGizmo(GizmoType.Selected)]
        static void OnDrawGizmoSelected(WayPointFunction way, GizmoType gizmoType)
        {
            var neighbors = way.GetComponent<WayPoint>().neighbors;


            foreach (var neighbor in neighbors)
            {
                if (neighbor != null)
                {
                    Handles.color = Color.blue;

                    Handles.DrawLine(way.transform.position, neighbor.transform.position);
                    Handles.DrawSolidDisc(neighbor.transform.position, neighbor.transform.forward, 0.2f);
                }
            }
            var wayPoint = way.GetComponent<WayPoint>();
            if (wayPoint.isWalkable)
            {
                Handles.color = Color.green;
                Handles.DrawSolidDisc(way.transform.position, way.transform.forward, 0.2f);
                Handles.color = Color.blue;
                Handles.DrawSolidDisc(way.transform.position, way.transform.forward, 0.1f);
            }
            else
            {

                Handles.color = new Color(1, 0, 0, 0.1f);
                Handles.DrawSolidDisc(way.transform.position, way.transform.forward, 0.6f);
            }


        }


    }
}
