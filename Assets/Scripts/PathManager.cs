using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//http://www.trickyfast.com/2017/09/21/building-a-waypoint-pathing-system-in-unity/
//https://gamedev.stackexchange.com/questions/118912/how-can-i-adapt-a-pathfinding-to-work-with-platformers
public class PathManager : MonoBehaviour
{
    public float walkSpeed = 5.0f;

    private Stack<Vector3> currentPath;
    private Vector3 currentWaypointPosition;
    private float moveTimeTotal;
    private float moveTimeCurrent;
    public Transform target;


    void Start()
    {
        InvokeRepeating("Search", 2, 5f);
    }
    void Search()
    {
        Debug.Log("Nova Busca ininicada!");
        Stop();
        NavigateTo(target.position);
    }
    public void NavigateTo(Vector3 destination)
    {
        currentPath = new Stack<Vector3>();
        var currentNode = FindClosestWaypoint(transform.position);
        var endNode = FindClosestWaypoint(destination);
        if (currentNode == null || endNode == null || currentNode == endNode)
            return;
        var openList = new SortedList<float, WayPoint>();
        var closedList = new List<WayPoint>();
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

                if (closedList.Contains(neighbor) || openList.ContainsValue(neighbor) )
                    continue;

			    if(neighbor == null){continue;}
                neighbor.previous = currentNode;
                neighbor.distance = dist + (neighbor.transform.position - currentNode.transform.position).magnitude;
                var distanceToTarget = (neighbor.transform.position - endNode.transform.position).magnitude;
                Debug.Log(neighbor.gameObject.name + " Distance: " + neighbor.distance + distanceToTarget);


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
                currentPath.Push(currentNode.transform.position);
                currentNode = currentNode.previous;
            }
            currentPath.Push(transform.position);
        }
    }

    public void Stop()
    {
        currentPath = null;
        moveTimeTotal = 0;
        moveTimeCurrent = 0;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Search();
        }
        if (currentPath != null && currentPath.Count > 0)
        {
            if (moveTimeCurrent < moveTimeTotal)
            {
                moveTimeCurrent += Time.deltaTime;
                if (moveTimeCurrent > moveTimeTotal)
                    moveTimeCurrent = moveTimeTotal;
                transform.position = Vector3.Lerp(currentWaypointPosition, currentPath.Peek(), moveTimeCurrent / moveTimeTotal);
            }
            else
            {
                currentWaypointPosition = currentPath.Pop();
                if (currentPath.Count == 0)
                    Stop();
                else
                {
                    moveTimeCurrent = 0;
                    moveTimeTotal = (currentWaypointPosition - currentPath.Peek()).magnitude / walkSpeed;
                }
            }
        }
    }

    private WayPoint FindClosestWaypoint(Vector3 target)
    {
        GameObject closest = null;
        float closestDist = Mathf.Infinity;
        foreach (var waypoint in GameObject.FindGameObjectsWithTag("Waypoint"))
        {
            var dist = (waypoint.transform.position - target).magnitude;
            if (dist < closestDist)
            {
                closest = waypoint;
                closestDist = dist;
            }
        }
        if (closest != null)
        {
            return closest.GetComponent<WayPoint>();
        }
        return null;
    }

}
