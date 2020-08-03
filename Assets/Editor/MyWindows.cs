using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
namespace PathSystem2D.Base
{
    public class MyWindows : EditorWindow
    {
        string myString = "Hello World";
        bool groupEnabled;
        bool myBool = true;
        float myFloat = 1.23f;

        // Add menu named "My Window" to the Window menu
        [MenuItem("Window/Path System/Create Init Path")]
        private static void CreateInitPath()
        {
            GameObject newPath = new GameObject();
            newPath.name = "PATH_ROOT";

            GameObject way = new GameObject();
            way.name = "WayPoint";
            way.tag = "Waypoint";
            way.transform.SetParent(newPath.transform);
            way.AddComponent<WayPoint>();
            way.AddComponent<WayPointFunction>();
            newPath.transform.SetAsFirstSibling();
        }
        [MenuItem("Window/Path System/Create Grade Path")]
        static void CreateGridPath()
        {
            // Get existing open window or if none, make a new one:
            CreateGridPathWindowns window = (CreateGridPathWindowns)EditorWindow.GetWindow(typeof(CreateGridPathWindowns));
            window.Show();

        }
        [MenuItem("Window/Path System/Join WayPoints")]
        static void JoinWayPoints()
        {
            SelectionCreate ways = new SelectionCreate();
            ways.Join();
        }


    }

    public class CreateGridPathWindowns : EditorWindow
    {
        static string name = "GRADE_PATH";
        static bool groupEnabled;
        static float whith = 10;
        static float heigth = 10;
        static float offset = 1f;
        static bool horizontal = true;

        void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            name = EditorGUILayout.TextField("Name", name);

            EditorGUILayout.BeginToggleGroup("Settings", true);

            whith = EditorGUILayout.FloatField("Whith", whith);
            heigth = EditorGUILayout.FloatField("Heigth", heigth);
            offset = EditorGUILayout.Slider("Offset", offset, 0, 10);
            horizontal = EditorGUILayout.Toggle("Neighbors Horizontal ?", horizontal);
            if (GUILayout.Button("Build"))
            {
                CreateGridPath();
            }
            EditorGUILayout.EndToggleGroup();
        }
        static void CreateGridPath()
        {
            GameObject newGrade = new GameObject();
            newGrade.name = name;
            newGrade.tag = "GRID_PATH";
            newGrade.AddComponent<Grade>();
            Grade grade = newGrade.GetComponent<Grade>();
            grade.nodes = new List<Node>();
            for (int x = 0; x < whith; x++)
            {
                for (int y = 0; y < heigth; y++)
                {
                    //Apply the offsets and create the world object for each node
                    float posX = x * offset;
                    float posY = y * offset;
                    float posZ = 0;
                    grade.nodes.Add(CreateNode(new Vector3(posX, posY, posZ)));

                }
            }
            FillNeighborsNode(grade);
        }
        static Node CreateNode(Vector3 position)
        {

            Node node = new Node();
            node.position = position;
            return node;

        }
        static void CreateWayPoint(GameObject parent, Vector3 position)
        {

            GameObject way = new GameObject();
            way.name = "WayPoint";
            way.tag = "Waypoint";
            way.transform.SetParent(parent.transform);
            way.AddComponent<WayPoint>();
            way.AddComponent<WayPointFunction>();
            way.transform.position = position;
            way.transform.name = position.x.ToString() + " " + position.y.ToString();
        }
        static void FillNeighborsWayPoint(Transform path_root)
        {
            int count = path_root.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform transform_child = path_root.GetChild(i);
                WayPoint check = transform_child.GetComponent<WayPoint>();
                Vector2 currentPosition = transform_child.position;
                FindNeighbor(check, path_root, currentPosition.x + offset, currentPosition.y);
                FindNeighbor(check, path_root, currentPosition.x - offset, currentPosition.y);
                FindNeighbor(check, path_root, currentPosition.x, currentPosition.y + offset);
                FindNeighbor(check, path_root, currentPosition.x, currentPosition.y - offset);


                //Pega Vizinhos da Diagonal - {(x+1,y+1),(x+1, y-1),(x-1, y+1),(x-1, y-1)
                if (horizontal)
                {
                    FindNeighbor(check, path_root, currentPosition.x + offset, currentPosition.y + offset);
                    FindNeighbor(check, path_root, currentPosition.x + offset, currentPosition.y - offset);
                    FindNeighbor(check, path_root, currentPosition.x - offset, currentPosition.y + offset);
                    FindNeighbor(check, path_root, currentPosition.x - offset, currentPosition.y - offset);
                }
            }
        }
        static void FillNeighborsNode(Grade grade)
        {
            int count = grade.nodes.Count;
            for (int i = 0; i < count; i++)
            {
                Node check = grade.nodes[i];

                Vector2 currentPosition = check.position;
                FindNeighborNode(check, grade, currentPosition.x + offset, currentPosition.y);
                FindNeighborNode(check, grade, currentPosition.x - offset, currentPosition.y);
                FindNeighborNode(check, grade, currentPosition.x, currentPosition.y + offset);
                FindNeighborNode(check, grade, currentPosition.x, currentPosition.y - offset);


                //Pega Vizinhos da Diagonal - {(x+1,y+1),(x+1, y-1),(x-1, y+1),(x-1, y-1)
                if (horizontal)
                {
                    FindNeighborNode(check, grade, currentPosition.x + offset, currentPosition.y + offset);
                    FindNeighborNode(check, grade, currentPosition.x + offset, currentPosition.y - offset);
                    FindNeighborNode(check, grade, currentPosition.x - offset, currentPosition.y + offset);
                    FindNeighborNode(check, grade, currentPosition.x - offset, currentPosition.y - offset);
                }
            }
        }
        static void FindNeighbor(WayPoint check, Transform root, float x, float y)
        {
            WayPoint child = null;
            int count = root.childCount;
            for (int i = 0; i < count; i++)
            {

                if (root.GetChild(i).transform.position == new Vector3(x, y, 0))
                {
                    child = root.GetChild(i).GetComponent<WayPoint>();

                }

            }
            if (child != null)
            {
                check.neighbors.Add(child);
            }


        }
        static void FindNeighborNode(Node check, Grade grade, float x, float y)
        {
            Node child = null;
            int count = grade.nodes.Count;
            for (int i = 0; i < count; i++)
            {

                if (grade.nodes[i].position == new Vector3(x, y, 0))
                {
                    child = new Node();
                    child = grade.nodes[i];

                }

            }
            if (child != null)
            {
                check.neighbors.Add(child);
            }


        }
    }
}