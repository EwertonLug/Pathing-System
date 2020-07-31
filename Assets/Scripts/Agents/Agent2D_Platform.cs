using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent2D_Platform : Agent2D
{
    public enum AgentStates_Platform
    {
        JUMP,
        DROP,
        WALK,
        IDLE
    }
    [Header("Agent2d Platform cofigs")]
    public AgentStates_Platform STATE;
    // Start is called before the first frame update
    void Start()
    {

    }

    protected override void UpdateStates()
    {
        if (NextWayPoint.y > CurrentWaypointPosition.y + 0.5f)
        {
            OnJump();
        }
        else if (NextWayPoint.y < CurrentWaypointPosition.y - 0.5f)
        {
            OnDrop();
        }
        else if (NextWayPoint == CurrentWaypointPosition)
        {

            OnIdle();
        }
        else
        {
            OnMove();

        }
        //Girando Agente
        if (NextWayPoint.x > CurrentWaypointPosition.x + 0.5f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (NextWayPoint.x < CurrentWaypointPosition.x - 0.5f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    protected override void OnChangeStates()
    {

        switch (STATE)
        {
            case AgentStates_Platform.WALK:
                GetComponent<Animator>().Play("enemy-walk");
                jump = 0;
                drop = 0;
                break;
            case AgentStates_Platform.JUMP:
                GetComponent<Animator>().Play("enemy-jump-in");
                drop = 0;

                break;
            case AgentStates_Platform.DROP:
                GetComponent<Animator>().Play("enemy-jump-out");
                jump = 0;

                break;
            case AgentStates_Platform.IDLE:
                GetComponent<Animator>().Play("enemy-idle");
                jump = 0;
                drop = 0;
                break;
        }
    }
    protected override void OnIdle()
    {
        STATE = AgentStates_Platform.IDLE;
    }
    protected override void OnMove()
    {
        STATE = AgentStates_Platform.WALK;
        transform.parent.position = Vector3.Lerp(CurrentWaypointPosition, CurrentPath.Peek(), MoveTimeCurrent / MoveTimeTotal);
        Debug.DrawLine(transform.parent.position, CurrentPath.Peek());//Direção
    }
    int jump = 0;
    protected override void OnJump()
    {
        STATE = AgentStates_Platform.JUMP;

        if (jump == 0)
        {
            bizierCurve.Clear();

            bizierCurve = BizierCurve(CurrentWaypointPosition, new Vector3(CurrentWaypointPosition.x, CurrentWaypointPosition.y + 10, 0), NextWayPoint, 20);

            pause = true;
            jump++;
            StartCoroutine(WalkCurve());

        }
    }
    int drop = 0;
    protected override void OnDrop()
    {
        STATE = AgentStates_Platform.DROP;

        if (drop == 0)
        {
            bizierCurve.Clear();
            float distance = Vector3.Distance(CurrentWaypointPosition, NextWayPoint);
            bizierCurve = BizierCurve(CurrentWaypointPosition, new Vector3(CurrentWaypointPosition.x, NextWayPoint.y + distance * 2, 0), NextWayPoint, 20);

            drop++;
            pause = true;
            StartCoroutine(WalkCurve());

        }
    }

}
