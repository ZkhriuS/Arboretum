using System;
using System.Collections;
using System.Collections.Generic;
using Edited;
using UnityEngine;

public class BonusTile : Tile
{
    public int bonus;
    public char typeBonus;
    public Color error;
    public Color correct;
    [SerializeField] private int oldSort;
    [SerializeField] private int newSort;
    
    private void Update()
    {
        renderer.sortingOrder = (isGrounded) ? newSort : oldSort;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
        foreach (Neighbours i in Enum.GetValues(typeof(Neighbours)))
        {
            if (bonusTile.gameObject.GetComponent<NeighbourController>().neighboursFree[i])
            {
                NeighbourController triggeredNeighbourC = triggeredTile.gameObject.GetComponent<NeighbourController>();
                switch (i)
                {
                    case Neighbours.UP_LEFT:
                        if (triggeredNeighbourC.neighboursFree[Neighbours.DOWN_RIGHT])
                            return false;
                        break;
                    case Neighbours.UP_CENTER:
                        if (triggeredNeighbourC.neighboursFree[Neighbours.DOWN_CENTER])
                            return false;
                        break;
                    case Neighbours.UP_RIGHT:
                        if (triggeredNeighbourC.neighboursFree[Neighbours.DOWN_LEFT])
                            return false;
                        break;
                    case Neighbours.DOWN_LEFT:
                        if (triggeredNeighbourC.neighboursFree[Neighbours.UP_RIGHT])
                            return false;
                        break;
                    case Neighbours.DOWN_CENTER:
                        if (triggeredNeighbourC.neighboursFree[Neighbours.UP_CENTER])
                            return false;
                        break;
                    case Neighbours.DOWN_RIGHT:
                        if (triggeredNeighbourC.neighboursFree[(int)Neighbours.UP_LEFT])
                            return false;
                        break;
                }
                break;
            }
        }
        return true;
    }
}
