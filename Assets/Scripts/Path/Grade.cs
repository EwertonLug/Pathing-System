using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathSystem2D.Base
{
    public class Grade : MonoBehaviour
    {

        [HideInInspector] public List<Node> nodes;
        public bool draw = true;
        public Tilemap TileColliderMap;

        void Awake()
        {
            Tilemap tilemap = TileColliderMap;
            BoundsInt bounds = tilemap.cellBounds;
            TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    if (tile != null)
                    {
                        //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);

                        foreach (var node in nodes)
                        {
                            if (node.position == new Vector3(x, y, 0))
                            {
                                node.isWalkable = false;

                            }
                        }
                    }
                }
            }
        }
        void OnDrawGizmos()
        {

            if (draw)
            {
                Gizmos.color = Color.yellow;


                foreach (var node in nodes)
                {
                    if (node != null)
                    {

                        if (node.isWalkable == true)
                        {
                            Gizmos.DrawSphere(node.position, 0.2f);
                        }

                        foreach (var neighbor in node.neighbors)
                        {
                            if (neighbor != null && neighbor.isWalkable == true && node.isWalkable == true)
                            {
                                Gizmos.DrawLine(node.position, neighbor.position);

                            }


                        }

                    }
                }
            }



        }

    }
}
