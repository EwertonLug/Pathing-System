using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace PathSystem2D.Base
{
    public class WayPoint : MonoBehaviour
    {
        public List<WayPoint> neighbors = new List<WayPoint>();
        public bool isWalkable = true;
        public bool isSkipped = true;
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


    }
}
