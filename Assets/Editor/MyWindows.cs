using UnityEngine;
using UnityEditor;

public class MyWindows : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Path System/Create Path")]
    static void Init()
    {

        MyWindows.CreateInitPath();
    }
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
    }


}