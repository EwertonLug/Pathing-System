using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//http://www.trickyfast.com/2017/09/21/building-a-waypoint-pathing-system-in-unity/
//https://gamedev.stackexchange.com/questions/118912/how-can-i-adapt-a-pathfinding-to-work-with-platformers
public enum IAState
{
    JUMP,
    DROP,
    WALK,
    IDLE
}


public class PathManager : MonoBehaviour
{
    public float walkSpeed = 5.0f;

    private Stack<Vector3> currentPath;
    public Vector3 currentWaypointPosition;
    public Vector3 nextWayPoint;
    private float moveTimeTotal;
    private float moveTimeCurrent;
    public Transform target;

    public int frequencyPerWayPoints;
    int currentFrequency = 0;
    public IAState STATE;
    void Start()
    {
        //InvokeRepeating("Search", 2, 5f);
        Search();
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
        var currentNode = FindClosestWaypoint(transform.parent.position);
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

                if (closedList.Contains(neighbor) || openList.ContainsValue(neighbor))
                    continue;

                if (neighbor == null) { continue; }
                neighbor.previous = currentNode;
                neighbor.distance = dist + (neighbor.transform.position - currentNode.transform.position).magnitude;
                var distanceToTarget = (neighbor.transform.position - endNode.transform.position).magnitude;



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
            currentPath.Push(transform.parent.position);
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
        if (nextWayPoint.y > currentWaypointPosition.y + 0.5f)
        {
            OnJump();
        }
        else if (nextWayPoint.y < currentWaypointPosition.y - 0.5f)
        {
            OnDrop();
        }
        else if (nextWayPoint == currentWaypointPosition){
           
            OnIdle();
            
           

        }else{
           
            STATE = IAState.WALK;
        }
        //
         if (nextWayPoint.x > currentWaypointPosition.x + 0.5f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (nextWayPoint.x<currentWaypointPosition.x - 0.5f)
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
             GetComponent<Animator>().Play("enemy-walk");
            break;
            case IAState.JUMP:
             GetComponent<Animator>().Play("enemy-jump-in");
            break;
            case IAState.DROP:
             GetComponent<Animator>().Play("enemy-jump-out");
            break;
             case IAState.IDLE:
             GetComponent<Animator>().Play("enemy-idle");
            break;
           
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
public void OnMove()
{

    transform.parent.position = Vector3.Lerp(currentWaypointPosition, currentPath.Peek(), moveTimeCurrent / moveTimeTotal);
}
public void OnJump()
{
    STATE = IAState.JUMP;


}
public void OnDrop()
{
    STATE = IAState.DROP;

}
public void OnIdle()
{
    STATE = IAState.IDLE;
}

}
