﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PathSystem2D.Base
{
    public class WayPointFunction : MonoBehaviour
    {

        // Start is called before the first frame update

        public GameObject CreateNeighbor()
        {
            var wayBase = this.GetComponent<WayPoint>();
            GameObject way = new GameObject();
            way.name = "WayPoint";
            way.tag = "Waypoint";
            way.transform.SetParent(this.transform.parent);
            way.transform.position = new Vector3(transform.position.x + 2, transform.position.y, 0);
            way.AddComponent<WayPoint>();
            way.AddComponent<WayPointFunction>();
            //Adicionando Vizinho ao WayPoint Atual
            Debug.Log(wayBase.neighbors.Count);
            wayBase.neighbors.Add(way.GetComponent<WayPoint>()); 
            //Adicionando WayPoint atual ao Vizinho Criado
            way.GetComponent<WayPoint>().neighbors.Add(wayBase);

            return way;
        }
        /**
         * Alterar o estado do WayPoint para Ou passavél OU Não passável
         * caso o mesmo esteja selecionado e a Tecla de Espaco tenha sido precionada.
         * 
         */
        public void UpdateStateWayPoint()
        {
            var target = this.GetComponent<WayPoint>();
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                    {
                        if (Event.current.keyCode == (KeyCode.Space))
                        {
                            if (!target.isWalkable)
                            {
                                target.isWalkable = true;

                            }
                            else
                            {
                                target.isWalkable = false;

                            }
                        }
                        break;
                    }
            }
        }
        public void WalkableChange()
        {
            GetComponent<WayPoint>().isWalkable = !GetComponent<WayPoint>().isWalkable;
        }
        public string GetState()
        {
            var state = this.GetComponent<WayPoint>().isWalkable;
            return state == true ? "" : "Block";
        }



    }
}
