using System;
using Edited;
using UnityEngine;

namespace Tiles
{
    public class GroundTile : Tile
    {
        private bool isFree;
        private bool isSetMode;
        public static Action<int> OnEnterGround;
        public static Action<int> OnExitGround;
        
        private void Start()
        {
            isFree = true;
            isSetMode = false;
        }

        public void SetNeighbours(ResourceTile prev)
        {
            foreach (Neighbour i in Enum.GetValues(typeof(Neighbour)))
            {
                if (gameObject.GetComponent<NeighbourController>().neighbours[i])
                {
                    gameObject.GetComponent<NeighbourController>().neighbours[i].gameObject.GetComponent<NeighbourController>().neighbours[GetOppositeIndex((Neighbour)i)] = this;
                    if (gameObject.GetComponent<NeighbourController>().neighbours[i].transform.IsChildOf(prev.transform))
                    {
                        prev.transform.DetachChildren();
                        gameObject.GetComponent<NeighbourController>().neighbours[i].transform.SetParent(transform);
                    }
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Tile tile = other.GetComponentInChildren<BuildingTile>();
            if (tile != null)
            {
                tile.GetComponentInParent<TileHolder>().SetTriggeredBonusTile(gameObject);
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            Tile tile = other.GetComponentInChildren<BuildingTile>();
            if (tile != null)
            {
                tile.GetComponentInParent<TileHolder>().SetTriggeredBonusTile(null);
            }
        }
        
        // private void OnTriggerEnter(Collider other)
        // {
        //     OnEnterGround?.Invoke(-1);
        // }
        // private void OnTriggerExit(Collider other)
        // {
        //     OnExitGround?.Invoke(+1);
        // }

        public void InitSetMode()
        {
            isSetMode = true;
        }

        public void ExitSetMode()
        {
            isSetMode = false;
        }
    }
}
