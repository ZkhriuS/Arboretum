using System;
using Edited;
using UnityEngine;

namespace Tiles
{
    public class BonusTile : Tile
    {
        public int bonus;
        public char typeBonus;
        public Color error;
        public Color correct;

        private void OnTriggerStay(Collider other)
        {
            Tile bonusTile = other.GetComponentInChildren<BonusTile>();
            if (bonusTile != null)
            {
                if (other.tag.Equals("Tile") && !bonusTile.isGrounded && !other.isTrigger)
                {
                    if (!CheckTiles(bonusTile, gameObject.GetComponent<Tile>()))
                    {
                        gameObject.GetComponent<SpriteRenderer>().color = error;
                        TileHolder holder = bonusTile.GetComponentInParent<TileHolder>();
                        if (holder)
                        {
                            holder.SetTriggeredBonusTile(null);
                        }
                    }
                    else
                    {
                        gameObject.GetComponent<SpriteRenderer>().color = correct;
                        TileHolder holder = bonusTile.GetComponentInParent<TileHolder>();
                        if (holder)
                        {
                            holder.SetTriggeredBonusTile(gameObject);
                        }
                    }
                }
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            Tile bonusTile = other.GetComponentInChildren<BonusTile>();
            if (bonusTile != null)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                var holder = bonusTile.GetComponentInParent<TileHolder>();
                if (holder)
                {
                    holder.SetTriggeredBonusTile(null);
                }
                if (!CheckTiles(bonusTile, gameObject.GetComponent<BonusTile>()))
                {
                
                }
            }
        }

        private bool CheckTiles(Tile bonusTile, Tile triggeredTile)
        {
            foreach (Neighbour i in Enum.GetValues(typeof(Neighbour)))
            {
                if (bonusTile.gameObject.GetComponent<NeighbourController>().neighbours[i])
                {
                    NeighbourController triggeredNeighbourC = triggeredTile.gameObject.GetComponent<NeighbourController>();
                    switch (i)
                    {
                        case Neighbour.UP_LEFT:
                            if (triggeredNeighbourC.neighbours[Neighbour.DOWN_RIGHT])
                                return false;
                            break;
                        case Neighbour.UP_CENTER:
                            if (triggeredNeighbourC.neighbours[Neighbour.DOWN_CENTER])
                                return false;
                            break;
                        case Neighbour.UP_RIGHT:
                            if (triggeredNeighbourC.neighbours[Neighbour.DOWN_LEFT])
                                return false;
                            break;
                        case Neighbour.DOWN_LEFT:
                            if (triggeredNeighbourC.neighbours[Neighbour.UP_RIGHT])
                                return false;
                            break;
                        case Neighbour.DOWN_CENTER:
                            if (triggeredNeighbourC.neighbours[Neighbour.UP_CENTER])
                                return false;
                            break;
                        case Neighbour.DOWN_RIGHT:
                            if (triggeredNeighbourC.neighbours[(int)Neighbour.UP_LEFT])
                                return false;
                            break;
                    }
                    break;
                }
            }
            return true;
        }
    }
}
