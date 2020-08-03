using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace PathSystem2D.Base
{
    public class SelectionCreate
    {


        public void Join()
        {

            List<WayPoint> currentSelections;
            if (UnityEditor.Selection.transforms.Length == 2)
            {
                currentSelections = new List<WayPoint>();
                foreach (Transform transform in UnityEditor.Selection.transforms)
                {
                    //Undo.RegisterUndo(transform, transform.name + " Looks at Main Camera");
                    if (transform.gameObject.GetComponent<WayPoint>())
                    {
                        currentSelections.Add(transform.gameObject.GetComponent<WayPoint>());
                    }

                }
                //Joined
                if (currentSelections != null && currentSelections.Count == 2)
                {
                    var way1 = currentSelections[0];
                    var way2 = currentSelections[1];

                    if (!way1.neighbors.Contains(way2))
                        way1.neighbors.Add(way2);
                    if (!way2.neighbors.Contains(way1))
                        way2.neighbors.Add(way1);

                    Debug.Log("Unição Criada");
                }
                else
                {
                    Debug.LogWarning("[INFO] Não é possivel unir objetos selecionados");
                }


            }
            else
            {

                Debug.LogWarning("[INFO] Não é possivel unir mais ou menos de 2 WayPoints");
            }
        }

    }
}
