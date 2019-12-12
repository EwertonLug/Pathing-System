using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node : ScriptableObject
{
    public Vector3 position;
    public List<Node> neighbors = new List<Node>();
    public bool isWalkable = true;
    
       public Node previous
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

