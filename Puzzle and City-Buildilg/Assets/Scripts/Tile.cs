using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public bool isGrounded;
    public string type;
    public void SetNewTile(GameObject prev)
    {
        GameObject nextTile = prev.GetComponentInParent<TileGenerator>().SetNewTile(prev.transform.position);
        NeighbourController prevNeighbourController = prev.GetComponent<NeighbourController>();
        if (prevNeighbourController)
        {
            nextTile.GetComponent<NeighbourController>().UnionMask(prevNeighbourController);
            Tile childTile = nextTile.GetComponentInChildren<BonusTile>();
            NeighbourController childNeighbourController = childTile.gameObject.GetComponent<NeighbourController>();
            Tile parentTile = nextTile.GetComponent<ResourceTile>();
            NeighbourController parentNeighbourController = parentTile.gameObject.GetComponent<NeighbourController>();
            if (childNeighbourController)
            {
                GameObject temp = new GameObject();
                temp.transform.position = childTile.gameObject.transform.position;
                GameObject prevChild = Instantiate(temp, prev.transform.position, Quaternion.identity);
                prevChild.transform.SetParent(temp.transform);
                Vector3 startRotationPosition = prevChild.transform.position;
                
                for (int angle = 0; angle < 360; angle += 60)
                {
                    Vector3 rotationDirection = startRotationPosition - temp.transform.position;
                    Ray ray = new Ray(temp.transform.position, rotationDirection);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, rotationDirection.magnitude, 1<<0 , QueryTriggerInteraction.Collide))
                    {
                        Tile hittedTile = hit.collider.gameObject.GetComponent<Tile>();
                        if (hittedTile)
                        {
                            NeighbourController hittedNeighbourController = hittedTile.gameObject.GetComponent<NeighbourController>();
                            switch (((int)temp.transform.eulerAngles.y) % 360)
                            {
                                case 0:
                                    {
                                        childNeighbourController.SetNeigbour((int)Neighbours.UP_LEFT, hittedTile);
                                        hittedNeighbourController.SetNeigbour(GetOppositeIndex(Neighbours.UP_LEFT), childTile);
                                        break;
                                    }
                                case 60:
                                    {
                                        childNeighbourController.SetNeigbour((int)Neighbours.UP_CENTER, hittedTile);
                                        hittedNeighbourController.SetNeigbour(GetOppositeIndex(Neighbours.UP_CENTER), childTile);
                                        break;
                                    }
                                case 120:
                                    {
                                        childNeighbourController.SetNeigbour((int)Neighbours.UP_RIGHT, hittedTile);
                                        hittedNeighbourController.SetNeigbour(GetOppositeIndex(Neighbours.UP_RIGHT), childTile);
                                        break;
                                    }
                                case 180:
                                    {
                                        childNeighbourController.SetNeigbour((int)Neighbours.DOWN_RIGHT, hittedTile);
                                        hittedNeighbourController.SetNeigbour(GetOppositeIndex(Neighbours.DOWN_RIGHT), childTile);
                                        break;
                                    }
                                case 240:
                                    {
                                        childNeighbourController.SetNeigbour((int)Neighbours.DOWN_CENTER, hittedTile);
                                        hittedNeighbourController.SetNeigbour(GetOppositeIndex(Neighbours.DOWN_CENTER), childTile);
                                        break;
                                    }
                                case 300:
                                    {
                                        childNeighbourController.SetNeigbour((int)Neighbours.DOWN_LEFT, hittedTile);
                                        hittedNeighbourController.SetNeigbour(GetOppositeIndex(Neighbours.DOWN_LEFT), childTile);
                                        break;
                                    }
                                }
                            Debug.Log("Hitted!");
                        }
                    }
                    temp.transform.Rotate(0, 60, 0);
                }
                Destroy(temp);
                Destroy(prevChild);
            }
            for(int i=0; i<parentNeighbourController.GetLength(); i++)
            {
                if (!parentNeighbourController.neighboursFree[i])
                {
                    parentNeighbourController.SetNeigbour(i, prev.GetComponent<BonusTile>().gameObject.GetComponent<NeighbourController>().neighboursFree[i]);
                    if (parentNeighbourController.neighboursFree[i])
                        i = 0;
                }
                if (parentNeighbourController.neighboursFree[i])
                {
                    parentNeighbourController.neighboursFree[i].gameObject.GetComponent<NeighbourController>().SetNeigbour(GetOppositeIndex((Neighbours)i), parentTile);
                }
            }
            parentTile.isGrounded = true;
            for (int i = 0; i < childNeighbourController.GetLength(); i++)
            {
                if (childNeighbourController.neighboursFree[i])
                {
                    childNeighbourController.neighboursFree[i].gameObject.GetComponent<NeighbourController>().SetNeigbour(GetOppositeIndex((Neighbours)i), childTile);
                }
            }
            childTile.isGrounded = true;
            parentTile.GetComponent<ChainController>().IncreaseScore(prev.GetComponent<BonusTile>());
        }
        
        /*Tile childTile = nextTile.GetComponentInChildren<BonusTile>();
        NeighbourController childNeighbourController = childTile.gameObject.GetComponent<NeighbourController>();
        temp.transform.eulerAngles = new Vector3 (0, -nextTile.transform.eulerAngles.y, 0);
        
        temp.transform.eulerAngles = new Vector3(0, 0, 0);
        GameObject child = Instantiate(temp, startRotationPosition, Quaternion.identity);
        child.transform.SetParent(temp.transform);
        f*/
        
    }

    public int GetOppositeIndex(Neighbours side)
    {
        switch (side)
        {
            case Neighbours.UP_LEFT: return (int)Neighbours.DOWN_RIGHT;
            case Neighbours.UP_CENTER: return (int)Neighbours.DOWN_CENTER;
            case Neighbours.UP_RIGHT: return (int)Neighbours.DOWN_LEFT;
            case Neighbours.DOWN_LEFT: return (int)Neighbours.UP_RIGHT;
            case Neighbours.DOWN_CENTER: return (int)Neighbours.UP_CENTER;
            case Neighbours.DOWN_RIGHT: return (int)Neighbours.UP_LEFT;
        }
        return -1;
    }

    public static Vector3 GetNeighbourRelativeCoord(Vector3 coord, Neighbours neighbour)
    {
        
        
        return Vector3.zero;
    }
}



public enum Neighbours
{
    UP_LEFT,
    UP_CENTER,
    UP_RIGHT,
    DOWN_LEFT,
    DOWN_CENTER,
    DOWN_RIGHT
}