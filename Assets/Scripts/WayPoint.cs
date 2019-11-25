using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public List<WayPoint> neighbors = new List<WayPoint>();
    public bool isWalkable = true;
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
       
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.15f);
         foreach (var neighbor in neighbors)
        {
            if (neighbor != null)
            {
               
               Gizmos.DrawLine(transform.position, neighbor.transform.position);
               Gizmos.DrawSphere(neighbor.transform.position, 0.15f);
            }
        }
      
      
    }
       void OnDrawGizmos()
    {   /*/
        if (neighbors == null )
            return;
        Gizmos.color = Color.green;
        foreach (var neighbor in neighbors)
        {
            if (neighbor != null)
            {
                Gizmos.DrawLine(transform.position, neighbor.transform.position);

                Gizmos.DrawSphere(neighbor.transform.position, 0.1f);
            }
        }
     

        Gizmos.DrawSphere(transform.position, 0.1f);*/
    }
}
