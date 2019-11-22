using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointFunction : MonoBehaviour
{
  
    // Start is called before the first frame update
 
    public void CreateNeighbor(){
        var wayBase = this.GetComponent<WayPoint>();
        GameObject way = new GameObject();
        way.name = "WayPoint";
        way.tag = "Waypoint";
        way.transform.SetParent(this.transform.parent);
        way.transform.position = new Vector3(transform.position.x+2, Random.Range(0,10), 0);
        way.AddComponent<WayPoint>();
        way.AddComponent<WayPointFunction>();
        //Adicionando Vizinho
        Debug.Log(wayBase.neighbors.Count);
        wayBase.neighbors.Add(way.GetComponent<WayPoint>());
        
    }
}
