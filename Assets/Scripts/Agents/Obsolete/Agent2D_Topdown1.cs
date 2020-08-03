using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathSystem2D.Obsolete
{
    [Obsolete("Using Agent2D_Topdown")]
    public class Agent2D_Topdown1 : Agent2D1
    {
        public enum AgentStates_Topdown
        {
            WALK,
            IDLE
        }
        [Header("Agent2d Topdown Configs")]
        public AgentStates_Topdown STATE;

        protected override void UpdateStates()
        {

            if (NextWayPoint == CurrentWaypointPosition)
            {

                OnIdle();
            }
            else
            {
                OnMove();

            }
        }
        protected override void OnChangeStates()
        {
            switch (STATE)
            {
                case AgentStates_Topdown.WALK:
                    //Executar animação correspondente aqui
                    break;

                case AgentStates_Topdown.IDLE:
                    //Executar animação correspondente aqui
                    break;
            }

        }
        protected override void OnIdle()
        {
            STATE = AgentStates_Topdown.IDLE;
        }
        protected override void OnMove()
        {
            STATE = AgentStates_Topdown.WALK;
            transform.parent.right = NextWayPoint - transform.parent.position;
            transform.parent.position = Vector3.Lerp(CurrentWaypointPosition, CurrentPath.Peek(), MoveTimeCurrent / MoveTimeTotal);


            Debug.DrawLine(transform.parent.position, CurrentPath.Peek());//Direção
        }
        protected override void OnJump()
        {

        }
        protected override void OnDrop()
        {

        }

    }
}
