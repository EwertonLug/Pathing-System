using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathSystem2D.Base;
namespace PathSystem2D
{
    public class Agent2D_Platform : Agent2D
    {
        [Header("Agent2d Platform cofigs")]
        public float bizierMoveSpeed = 3f;
        public List<Vector3> bizierCurve;
        private GameObject[] waypoints;
        [Header("Limiter JUMP | DROP")]
        public float jumpLimiter = 2f;
        public float dropLimiter = 2f;
        private void Start()
        {
            waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
            Search();
        }

        new void FixedUpdate()
        {
            base.FixedUpdate();
            DebugCurve();
        }
        protected override void Search()
        {

            Stop();
            if (!pause)
            {
                NavigateToWaypoints(target.position);
                Debug.Log("Nova Busca ininicada!");
            }
        }

        private void NavigateToWaypoints(Vector3 destination)
        {
            CurrentPath = new Stack<Vector3>();


            var currentNode = FindClosestWaypoint(transformAgent.parent.position);
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
                CurrentPath.Push(transformAgent.parent.position);
            }

        }
        private WayPoint FindClosestWaypoint(Vector3 target)
        {
            GameObject closest = null;
            float closestDist = Mathf.Infinity;
            foreach (var waypoint in waypoints)
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
        protected Vector3 curretBizzerPosition;
        public IEnumerator WalkCurve()
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
        public void DebugCurve()
        {
            //Debug
            for (int i = 1; i < bizierCurve.Count; i++)
            {
                Debug.DrawLine(bizierCurve.ToArray()[i - 1], bizierCurve.ToArray()[i]);//Direção


            }
        }

    }
}
