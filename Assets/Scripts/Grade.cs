using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grade : MonoBehaviour
{

    public List<Node> nodes = new List<Node>();



    void OnDrawGizmosSelected()
    {
       
        Gizmos.color = Color.blue;
      
        
         foreach (var node in nodes)
        {
            if (node != null)
            {
               
              
               Gizmos.DrawSphere(node.position, 0.2f);
             
            }
        }
      
      
    }
    
}
