using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//http://www.trickyfast.com/2017/09/21/building-a-waypoint-pathing-system-in-unity/
//https://gamedev.stackexchange.com/questions/118912/how-can-i-adapt-a-pathfinding-to-work-with-platformers


public abstract class Agent2D : MonoBehaviour
{
    public enum PathType { NODE, WAYPOINT }
    [Header("Pathfinder Configs")]
    public PathType pathType;
    public float walkSpeed = 10.0f;
    public float bizierMoveSpeed = 3f;

    private Stack<Vector3> currentPath;
    public List<Vector3> bizierCurve;

    [SerializeField] private Vector3 currentWaypointPosition;
    private Vector3 nextWayPoint;
    private float moveTimeTotal;
    private float moveTimeCurrent;
    public Transform target;

    public int frequencyPerWayPoints = 2;
    int currentFrequency = 0;

    public bool pause = false;
    private Grade gradePath;
    void Start()
    {
        if (pathType == PathType.NODE)
            gradePath = GameObject.FindGameObjectWithTag("GRID_PATH").GetComponent<Grade>();

        Search();
    }
    private void Search()
    {
        Debug.Log("Nova Busca ininicada!");
        Stop();
        if (!pause)
        {
          
            if (pathType == PathType.WAYPOINT)
                NavigateToWaypoints(target.position);
            if (pathType == PathType.NODE)
                NavigateToNode(target.position);
        }

    }
    private void NavigateToNode(Vector3 destination)
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


    private void NavigateToWaypoints(Vector3 destination)
    {
        CurrentPath = new Stack<Vector3>();


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

                if (neighbor == null || neighbor.isWalkable == false) { continue; }
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
                CurrentPath.Push(currentNode.transform.position);
                currentNode = currentNode.previous;
            }
            CurrentPath.Push(transform.parent.position);
        }

    }

    protected void Stop()
    {
        CurrentPath = null;
        MoveTimeTotal = 0;
        MoveTimeCurrent = 0;
    }

    void FixedUpdate()
    {
        DebugCurve();
        if (!pause)
        {
            if (CurrentPath != null && CurrentPath.Count > 0)
            {
                NextWayPoint = CurrentPath.Peek();
                if (MoveTimeCurrent < MoveTimeTotal)
                {
                    MoveTimeCurrent += Time.deltaTime;

                    if (MoveTimeCurrent > MoveTimeTotal)
                        MoveTimeCurrent = MoveTimeTotal;

                    OnMove();
                }
                else
                {
                    CurrentWaypointPosition = CurrentPath.Pop();
                    if (CurrentPath.Count == 0)
                    {
                        Stop();

                    }
                    else
                    {
                        MoveTimeCurrent = 0;
                        currentFrequency++;
                        MoveTimeTotal = (CurrentWaypointPosition - CurrentPath.Peek()).magnitude / walkSpeed;

                    }
                }
            }
            else
            {
                currentFrequency = 3;

            }
        }//Fim PAUSE

        if (currentFrequency > frequencyPerWayPoints)
        {
            Search();
            currentFrequency = 0;
        }
        UpdateStates();
        OnChangeStates();

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
    protected abstract void UpdateStates();
    protected abstract void OnChangeStates();
    protected abstract void OnMove();
    protected abstract void OnJump();

    protected abstract void OnDrop();
    protected abstract void OnIdle();
    protected Vector3 curretBizzerPosition;

    protected IEnumerator WalkCurve()
    {

        while (true)
        {


            for (int i = 0; i < bizierCurve.Count; i++)
            {

                yield return new WaitForSeconds(bizierMoveSpeed / 100);
                if (bizierCurve.Count > 0)
                {
                    transform.parent.position = bizierCurve[i];
                    curretBizzerPosition = bizierCurve[i];
                }

            }
            if (bizierCurve.Count > 0)
            {
                if (curretBizzerPosition == bizierCurve[bizierCurve.Count - 1])
                {
                    bizierCurve.Clear();
                    pause = false;
                    CurrentWaypointPosition = curretBizzerPosition;
                    MoveTimeTotal = 0;
                    MoveTimeCurrent = 0;
                    StopCoroutine(WalkCurve());

                }
            }
            yield return null;
        }

    }
    protected List<Vector3> BizierCurve(Vector3 current, Vector3 middle, Vector3 next, float numberOfPoints)
    {
        List<Vector3> path = new List<Vector3>();

        // set points of quadratic Bezier curve
        Vector3 p0 = current;
        Vector3 p1 = middle;
        Vector3 p2 = next;
        float t;
        Vector3 position;
        for (int i = 0; i < numberOfPoints; i++)
        {
            t = i / (numberOfPoints - 1.0f);
            position = (1.0f - t) * (1.0f - t) * p0
            + 2.0f * (1.0f - t) * t * p1 + t * t * p2;

            path.Add(position);
        }
        return path;
    }
    private void DebugCurve()
    {
        //Debug
        for (int i = 1; i < bizierCurve.Count; i++)
        {
            Debug.DrawLine(bizierCurve.ToArray()[i - 1], bizierCurve.ToArray()[i]);//Direção


        }
    }

    public Vector3 CurrentWaypointPosition { get => currentWaypointPosition; set => currentWaypointPosition = value; }
    public Stack<Vector3> CurrentPath { get => currentPath; set => currentPath = value; }
    protected float MoveTimeTotal { get => moveTimeTotal; set => moveTimeTotal = value; }
    protected float MoveTimeCurrent { get => moveTimeCurrent; set => moveTimeCurrent = value; }
    public Vector3 NextWayPoint { get => nextWayPoint; set => nextWayPoint = value; }
}



