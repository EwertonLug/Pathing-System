using PathSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent2D_Platform))]
public class AIPlatform2D : MonoBehaviour
{
    public enum AgentStates_Platform
    {
        JUMP,
        DROP,
        WALK,
        IDLE
    }
    private Agent2D_Platform agent;
    public Transform target;
    public AgentStates_Platform STATE;
   
    // Start is called before the first frame update
    void Start()
    {

        agent = gameObject.GetComponent<Agent2D_Platform>();
        agent.Register(transform);
        agent.SetTarget(target);
        agent.StartSearch();
       
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStates();
        OnChangeStates();
    }
    protected void UpdateStates()
    {
        
        
        if (agent.NextWayPoint.y > agent.CurrentWaypointPosition.y + 0.5f)
        {
            OnJump();
        }
        else if (agent.NextWayPoint.y < agent.CurrentWaypointPosition.y - 0.5f)
        {
            OnDrop();
        }
        else if (agent.NextWayPoint == agent.CurrentWaypointPosition)
        {

            OnIdle();
        }
        else
        {
            OnMove();

        }
        //Girando Agente
        if (agent.NextWayPoint.x > agent.CurrentWaypointPosition.x + 0.5f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (agent.NextWayPoint.x < agent.CurrentWaypointPosition.x - 0.5f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
       
           

    }
    protected void OnChangeStates()
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
    protected void OnIdle()
    {
        STATE = AgentStates_Platform.IDLE;
    }
    protected void OnMove()
    {
        STATE = AgentStates_Platform.WALK;
        transform.parent.position = Vector3.Lerp(agent.CurrentWaypointPosition, agent.CurrentPath.Peek(), agent.MoveTimeCurrent / agent.MoveTimeTotal);
        Debug.DrawLine(transform.parent.position, agent.CurrentPath.Peek());//Direção
    }
    int jump = 0;
    protected void OnJump()
    {
        STATE = AgentStates_Platform.JUMP;

        if (jump == 0)
        {
            agent.bizierCurve.Clear();

            agent.bizierCurve = agent.BizierCurve(agent.CurrentWaypointPosition, new Vector3(agent.CurrentWaypointPosition.x, agent.CurrentWaypointPosition.y + 10, 0), agent.NextWayPoint, 20);

            agent.pause = true;
            jump++;
            StartCoroutine(agent.WalkCurve());

        }
    }
    int drop = 0;
    protected void OnDrop()
    {
        STATE = AgentStates_Platform.DROP;

        if (drop == 0)
        {
            agent.bizierCurve.Clear();
            float distance = Vector3.Distance(agent.CurrentWaypointPosition, agent.NextWayPoint);
            agent.bizierCurve = agent.BizierCurve(agent.CurrentWaypointPosition, new Vector3(agent.CurrentWaypointPosition.x, agent.NextWayPoint.y + distance * 2, 0), agent.NextWayPoint, 20);

            drop++;
            agent.pause = true;
            StartCoroutine(agent.WalkCurve());

        }
    }

}
