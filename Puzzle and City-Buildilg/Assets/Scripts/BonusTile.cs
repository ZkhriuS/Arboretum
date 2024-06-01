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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

    public void SetBaseTile()
    {
        Tile baseTile = gameObject.GetComponent<BonusTile>();
        if (baseTile)
        {
            SetNewTile(baseTile.gameObject);
            Destroy(baseTile.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Tile bonusTile = other.GetComponentInChildren<BonusTile>();
        if (bonusTile != null)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            if (!CheckTiles(bonusTile, gameObject.GetComponent<BonusTile>()))
            {
                
            }
        }
    }

    private bool CheckTiles(Tile bonusTile, Tile triggeredTile)
    {
        for (int i = 0; i < 6; i++)
        {
            if (bonusTile.gameObject.GetComponent<NeighbourController>().neighboursFree[i])
            {
                NeighbourController triggeredNeighbourC = triggeredTile.gameObject.GetComponent<NeighbourController>();
                switch ((Neighbours)i)
                {
                    case Neighbours.UP_LEFT:
                        if (triggeredNeighbourC.neighboursFree[(int)Neighbours.DOWN_RIGHT])
                            return false;
                        break;
                    case Neighbours.UP_CENTER:
                        if (triggeredNeighbourC.neighboursFree[(int)Neighbours.DOWN_CENTER])
                            return false;
                        break;
                    case Neighbours.UP_RIGHT:
                        if (triggeredNeighbourC.neighboursFree[(int)Neighbours.DOWN_LEFT])
                            return false;
                        break;
                    case Neighbours.DOWN_LEFT:
                        if (triggeredNeighbourC.neighboursFree[(int)Neighbours.UP_RIGHT])
                            return false;
                        break;
                    case Neighbours.DOWN_CENTER:
                        if (triggeredNeighbourC.neighboursFree[(int)Neighbours.UP_CENTER])
                            return false;
                        break;
                    case Neighbours.DOWN_RIGHT:
                        if (triggeredNeighbourC.neighboursFree[(int)Neighbours.UP_LEFT])
                            return false;
                        break;
                }
                i = 6;
            }
        }
        return true;
    }
}
