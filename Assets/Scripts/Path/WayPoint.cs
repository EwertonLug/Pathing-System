using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class WayPoint : MonoBehaviour
{
    public List<WayPoint> neighbors = new List<WayPoint>();
    public bool isWalkable = true;
    public WayPoint previous
    {
        get;
        set;
    }

    public float distance
    {
        get;
        set;
    }

    
}
