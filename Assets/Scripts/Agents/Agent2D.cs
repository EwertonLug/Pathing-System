using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//http://www.trickyfast.com/2017/09/21/building-a-waypoint-pathing-system-in-unity/
//https://gamedev.stackexchange.com/questions/118912/how-can-i-adapt-a-pathfinding-to-work-with-platformers

namespace PathSystem2D
{
    public abstract class Agent2D : MonoBehaviour
    {

        [Header("Pathfinder Configs")]

        public float walkSpeed = 10.0f;


        protected Stack<Vector3> currentPath;


        [SerializeField] private Vector3 currentWaypointPosition;
        private Vector3 nextWayPoint;
        private float moveTimeTotal;
        private float moveTimeCurrent;
        protected Transform target;

        public int frequencyPerWayPoints = 2;
        int currentFrequency = 0;

        public bool pause = false;

        protected Transform transformAgent;

        public void Register(Transform transformAgent)
        {
            this.transformAgent = transformAgent;
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }
        public void StartSearch()
        {
            pause = false;
        }
        public void StopSearch()
        {
            pause = true;
        }


        protected void Stop()
        {
            CurrentPath = null;
            MoveTimeTotal = 0;
            MoveTimeCurrent = 0;
            Debug.Log("Parado");
        }

        protected void FixedUpdate()
        {

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

        }
        public bool isFoundTarget()
        {
            if (NextWayPoint == CurrentWaypointPosition)
                return true;
            else
                return false;
        }



        protected abstract void Search();

        public Vector3 CurrentWaypointPosition { get => currentWaypointPosition; set => currentWaypointPosition = value; }
        public Stack<Vector3> CurrentPath { get => currentPath; set => currentPath = value; }
        public float MoveTimeTotal { get => moveTimeTotal; set => moveTimeTotal = value; }
        public float MoveTimeCurrent { get => moveTimeCurrent; set => moveTimeCurrent = value; }
        public Vector3 NextWayPoint { get => nextWayPoint; set => nextWayPoint = value; }
    }
}



