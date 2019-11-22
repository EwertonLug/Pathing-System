using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public List<WayPoint> neighbors;

    public WayPoint previous
    {
        get;
        set;
    }

    public float distance
    {
        get;
        set;
    }


    void OnDrawGizmosSelected()
    {
        if (neighbors == null)
            return;
        Gizmos.color = Color.red;
        foreach (var neighbor in neighbors)
        {
            if (neighbor != null)
            {
                Gizmos.DrawLine(transform.position, neighbor.transform.position);

                Gizmos.DrawSphere(neighbor.transform.position, 0.1f);
            }
        }

        Gizmos.DrawSphere(transform.position, 0.1f);
    }
    void OnDrawGizmos()
    {
       
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
