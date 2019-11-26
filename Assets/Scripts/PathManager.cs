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
    public float jumpForce = 5f;

    private Stack<Vector3> currentPath;
    public List<Vector3> bizierCurve;
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

    void Start()
    {
        //InvokeRepeating("Search", 2, 5f);
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

    void FixedUpdate()
    {
        DebugCurve();
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

        if (nextWayPoint.y > currentWaypointPosition.y + 0.5f)
        {
            OnJump();
        }
        else if (nextWayPoint.y < currentWaypointPosition.y - 0.5f)
        {
            OnDrop();
        }
        else if (nextWayPoint == currentWaypointPosition)
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
                GetComponent<Animator>().Play("enemy-walk");
                jump = 0;
                drop = 0;
                break;
            case IAState.JUMP:
                GetComponent<Animator>().Play("enemy-jump-in");
                drop = 0;

                break;
            case IAState.DROP:
                GetComponent<Animator>().Play("enemy-jump-out");
                jump = 0;

                break;
            case IAState.IDLE:
                GetComponent<Animator>().Play("enemy-idle");
                jump = 0;
                drop = 0;
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
        Debug.DrawLine(transform.parent.position, currentPath.Peek());//Direção

    }
    int jump = 0;

    public void OnJump()
    {

        STATE = IAState.JUMP;

        if (jump == 0)
        {
            bizierCurve.Clear();

            bizierCurve = BizierCurve(currentWaypointPosition, new Vector3(currentWaypointPosition.x, currentWaypointPosition.y + 10, 0), nextWayPoint, 20);

            pause = true;
            jump++;
            StartCoroutine(WalkCurve());

        }


    }


    int drop = 0;
    public void OnDrop()
    {
        STATE = IAState.DROP;

        if (drop == 0)
        {
            bizierCurve.Clear();
            float distance = Vector3.Distance(currentWaypointPosition, nextWayPoint);
            bizierCurve = BizierCurve(currentWaypointPosition, new Vector3(currentWaypointPosition.x, nextWayPoint.y + distance*2, 0), nextWayPoint, 20);

            drop++;
            pause = true;
            StartCoroutine(WalkCurve());

        }
    }

    public Vector3 curretBizzerPosition;
    IEnumerator WalkCurve()
    {

        while (true)
        {


            for (int i = 0; i < bizierCurve.Count; i++)
            {

                yield return new WaitForSeconds(0.03f);
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
                    currentWaypointPosition = curretBizzerPosition;
                    moveTimeTotal = 0;
                    moveTimeCurrent = 0;
                    StopCoroutine(WalkCurve());

                }
            }
            yield return null;
        }

    }
    public void OnIdle()
    {
        STATE = IAState.IDLE;
    }
    public List<Vector3> BizierCurve(Vector3 current, Vector3 middle, Vector3 next, float numberOfPoints)
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
    void DebugCurve()
    {
        //Debug
        for (int i = 1; i < bizierCurve.Count; i++)
        {
            Debug.DrawLine(bizierCurve.ToArray()[i - 1], bizierCurve.ToArray()[i]);//Direção


        }
    }

}



