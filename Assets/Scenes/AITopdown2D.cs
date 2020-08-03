using PathSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent2D_Topdown))]
public  class AITopdown2D : MonoBehaviour
{
    public enum AgentStates_Topdown
    {
        WALK,
        IDLE
    }
    public Agent2D_Topdown agent;
    public Transform target;
    public AgentStates_Topdown STATE = AgentStates_Topdown.IDLE;
    // Start is called before the first frame update
    void Start()
    {
        
        agent = gameObject.GetComponent<Agent2D_Topdown>();
        agent.Register(transform);
        agent.SetTarget(target);
        agent.StartSearch();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isFoundTarget())
        {

            OnIdle();
        }
        else
        {
            OnMove();

        }
    }
    protected  void OnMove()
    {

        STATE = AgentStates_Topdown.IDLE;
        transform.parent.right = agent.NextWayPoint - transform.parent.position;
        transform.parent.position = Vector3.Lerp(agent.CurrentWaypointPosition, agent.CurrentPath.Peek(), agent.MoveTimeCurrent / agent.MoveTimeTotal);


        Debug.DrawLine(transform.parent.position, agent.CurrentPath.Peek());//Direção
    }
    protected void OnIdle()
    {
        STATE = AgentStates_Topdown.IDLE;
    }

}
