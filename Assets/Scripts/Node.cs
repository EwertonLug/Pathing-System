using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node
{
    public Vector3 position;
    public List<Node> neighbors = new List<Node>();
    public bool isWalkable = true;
    public Node previous;
}

