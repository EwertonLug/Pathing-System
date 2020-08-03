using PathSystem2D.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PathSystem2D
{
    public class Agent2D_Topdown : Agent2D
    {
        [Header("Agent2d TopDown cofigs")]
        protected Grade gradePath;
        private void Start()
        {
            gradePath = GameObject.FindGameObjectWithTag("GRID_PATH").GetComponent<Grade>();
            Search();
        }
        protected override void Search()
        {
            Debug.Log("Nova Busca ininicada!");
            Stop();
            if (!pause)
            {
                NavigateToNode(target.position);
            }
        }

        private void NavigateToNode(Vector3 destination)
        {
            currentPath = new Stack<Vector3>();
            var currentNode = FindClosestNode(transformAgent.parent.position);
            var endNode = FindClosestNode(destination);
            if (currentNode == null || endNode == null || currentNode == endNode)
                return;
            var openList = new SortedList<float, Node>();
            var closedList = new List<Node>();
            openList.Add(0, currentNode);
            currentNode.previous = null;
            currentNode.distance = 0f;
            while (openList.Count > 0)
            {
                currentNode = openList.Values[0];
                openList.RemoveAt(0);
                var dist = currentNode.distance;
                closedList.Add(currentNode);
                if (currentNode == endNode)
                {
                    break;
                }
                foreach (var neighbor in currentNode.neighbors)
                {

                    if (closedList.Contains(neighbor) || openList.ContainsValue(neighbor))
                        continue;

                    if (neighbor == null || neighbor.isWalkable == false) { continue; }
                    neighbor.previous = currentNode;
                    neighbor.distance = dist + (neighbor.position - currentNode.position).magnitude;
                    var distanceToTarget = (neighbor.position - endNode.position).magnitude;



                    if (!openList.ContainsKey(neighbor.distance + distanceToTarget))
                    {
                        openList.Add(neighbor.distance + distanceToTarget, neighbor);
                    }


                }
            }
            if (currentNode == endNode)
            {
                while (currentNode.previous != null)
                {
                    currentPath.Push(currentNode.position);
                    currentNode = currentNode.previous;
                }
                currentPath.Push(transformAgent.parent.position);
            }

        }
        private Node FindClosestNode(Vector3 target)
        {
            Node closest = null;
            float closestDist = Mathf.Infinity;
            foreach (var node in gradePath.nodes)
            {
                if (node.isWalkable == true)
                {
                    var dist = (node.position - target).magnitude;
                    if (dist < closestDist)
                    {
                        closest = node;
                        closestDist = dist;
                    }
                }

            }
            if (closest != null)
            {
                return closest;
            }
            return null;
        }

    }
}
