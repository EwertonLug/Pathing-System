using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//http://www.trickyfast.com/2017/09/21/building-a-waypoint-pathing-system-in-unity/
//https://gamedev.stackexchange.com/questions/118912/how-can-i-adapt-a-pathfinding-to-work-with-platformers



public class Agent2D_Topdown : MonoBehaviour
{
    public enum IAState
    {

        WALK,
        IDLE
    }
    public float walkSpeed = 5.0f;


    private Stack<Vector3> currentPath;

    public List<Vector3> path;
    public Vector3 currentWaypointPosition;
    public Vector3 nextWayPoint;
    private float moveTimeTotal;
    private float moveTimeCurrent;
    public Transform target;

    public int frequencyPerWayPoints;
    int currentFrequency = 0;
    public IAState STATE;
    public bool pause = false;
    private Grade gradePath;
    void Start()
    {
        gradePath = GameObject.FindGameObjectWithTag("GRID_PATH").GetComponent<Grade>();
        Search();
    }
    void Search()
    {
        Debug.Log("Nova Busca ininicada!");
        Stop();
        if (!pause)
        {
            NavigateTo(target.position);
        }

    }
    public void NavigateTo(Vector3 destination)
    {
        currentPath = new Stack<Vector3>();
        var currentNode = FindClosestNode(transform.parent.position);
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
            currentPath.Push(transform.parent.position);
        }
    }

    public void Stop()
    {
        currentPath = null;
        moveTimeTotal = 0;
        moveTimeCurrent = 0;
    }

    void FixedUpdate()
    {

        if (!pause)
        {
            if (currentPath != null && currentPath.Count > 0)
            {
                nextWayPoint = currentPath.Peek();
                if (moveTimeCurrent < moveTimeTotal)
                {
                    moveTimeCurrent += Time.deltaTime;

                    if (moveTimeCurrent > moveTimeTotal)
                        moveTimeCurrent = moveTimeTotal;

                    OnMove();
                }
                else
                {
                    currentWaypointPosition = currentPath.Pop();
                    if (currentPath.Count == 0)
                    {
                        Stop();

                    }
                    else
                    {
                        moveTimeCurrent = 0;
                        currentFrequency++;
                        moveTimeTotal = (currentWaypointPosition - currentPath.Peek()).magnitude / walkSpeed;

                    }
                }
            }
            else
            {
                currentFrequency = 3;

            }
        }//Fim PAUSE


        if (nextWayPoint == currentWaypointPosition)
        {

            OnIdle();
        }
        else
        {

            STATE = IAState.WALK;
        }
        //Girando Agente
        if (nextWayPoint.x > currentWaypointPosition.x + 0.5f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (nextWayPoint.x < currentWaypointPosition.x - 0.5f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (currentFrequency > frequencyPerWayPoints)
        {
            Search();
            currentFrequency = 0;
        }
        switch (STATE)
        {
            case IAState.WALK:


                break;

            case IAState.IDLE:


                break;
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
    public void OnMove()
    {

         transform.parent.right = nextWayPoint - transform.parent.position;
        transform.parent.position = Vector3.Lerp(currentWaypointPosition, currentPath.Peek(), moveTimeCurrent / moveTimeTotal);
        
      
        Debug.DrawLine(transform.parent.position, currentPath.Peek());//Direção

    }
    public void OnIdle()
    {
        STATE = IAState.IDLE;
    }



}



