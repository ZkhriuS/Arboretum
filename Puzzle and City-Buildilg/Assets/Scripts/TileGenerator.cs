using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> tileList;
    [SerializeField] private List<GameObject> plusTileList;
    [SerializeField] private List<GameObject> mulTileList;
    [SerializeField] private GameObject tileHolder;
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject rotationIndicatorParent;
    [SerializeField] private GameObject rotationIndicatorChild;
    private GameObject newTile;
    private GameObject childTile;
    private Neighbours startAngleNeighbours;
    public static event Action<int> TilePeopleArrived;
    [SerializeField] private int minPeople;

    [SerializeField] private int maxPeople;

    // Start is called before the first frame update
    void Start()
    {
        newTile = null;
        childTile = null;
        rotationIndicatorParent.transform.position = grid.CellToWorld(new Vector3Int(0, 0));
        rotationIndicatorChild.transform.position = grid.CellToWorld(new Vector3Int(0, 1));
        rotationIndicatorChild.transform.parent = rotationIndicatorParent.transform;
        startAngleNeighbours = Neighbours.UP_LEFT;
    }

    // Update is called once per frame
    void Update()
    {
        if (newTile)
        {
            rotationIndicatorParent.transform.position = newTile.transform.position;
            var localPosition = rotationIndicatorChild.transform.localPosition;
            localPosition = new Vector3(localPosition.x, localPosition.y, 0);
            rotationIndicatorChild.transform.localPosition = localPosition;
            childTile.transform.position =
                grid.CellToWorld(grid.WorldToCell(rotationIndicatorChild.transform.position));
        }
    }

    public bool IsOtherActionBlocked()
    {
        return newTile;
    }

    public GameObject GetNewTile()
    {
        return newTile;
    }

    public void AddTile()
    {
        int variant = UnityEngine.Random.Range(0, tileList.Capacity);
        newTile = Instantiate(tileList[variant], tileHolder.transform);
        variant = UnityEngine.Random.Range(0, 10);
        if (variant < 4)
        {
            variant = UnityEngine.Random.Range(0, mulTileList.Capacity);
            childTile = Instantiate(mulTileList[variant], tileHolder.transform);
            childTile.transform.parent = newTile.transform;
        }
        else
        {
            variant = UnityEngine.Random.Range(0, plusTileList.Capacity);
            childTile = Instantiate(plusTileList[variant], tileHolder.transform);
            childTile.transform.parent = newTile.transform;
        }

        Tile bonusTile = childTile.gameObject.GetComponent<Tile>();
        bonusTile.gameObject.GetComponent<NeighbourController>().SetNeigbour(
            bonusTile.GetOppositeIndex(startAngleNeighbours),
            newTile.gameObject.GetComponent<ResourceTile>());
        newTile.gameObject.GetComponent<NeighbourController>().SetNeigbour((int) startAngleNeighbours, bonusTile);
        Debug.Log(startAngleNeighbours);
    }

    public void LeftRotate()
    {
        Tile bonusTile = childTile.gameObject.GetComponent<Tile>();
        NeighbourController bonusNeighbourC = bonusTile.gameObject.GetComponent<NeighbourController>();
        if (bonusTile)
        {
            rotationIndicatorParent.transform.Rotate(new Vector3(0, 0, 60));
            Vector3 localPosition = rotationIndicatorChild.transform.localPosition;
            Vector3Int cellPosition = grid.LocalToCell(localPosition);
            localPosition = grid.CellToLocal(new Vector3Int(cellPosition.x, cellPosition.y));
            rotationIndicatorChild.transform.localPosition = new Vector3(localPosition.x, localPosition.y,
                rotationIndicatorParent.transform.position.z);
            for (int i = 0; i < 6; i++)
            {
                newTile.gameObject.GetComponent<NeighbourController>().SetNeigbour(i, null);
                if (bonusNeighbourC.neighboursFree[i])
                {
                    bonusNeighbourC.neighboursFree[i] = null;
                    Neighbours index = (Neighbours) i;
                    RotateStartAngle(startAngleNeighbours, false);
                    switch (index)
                    {
                        case Neighbours.UP_LEFT:
                        {
                            newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeigbour((int) Neighbours.DOWN_RIGHT, null);
                            bonusNeighbourC.SetNeigbour((int) Neighbours.DOWN_LEFT,
                                newTile.gameObject.GetComponent<ResourceTile>());
                            newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeigbour((int) Neighbours.UP_RIGHT, bonusTile);
                        }
                            break;
                        case Neighbours.UP_CENTER:
                        {
                            newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeigbour((int) Neighbours.DOWN_CENTER, null);
                            bonusNeighbourC.SetNeigbour((int) Neighbours.UP_LEFT,
                                newTile.gameObject.GetComponent<ResourceTile>());
                            newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeigbour((int) Neighbours.DOWN_RIGHT, bonusTile);
                        }
                            break;
                        case Neighbours.UP_RIGHT:
                        {
                            newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeigbour((int) Neighbours.DOWN_LEFT, null);
                            bonusNeighbourC.SetNeigbour((int) Neighbours.UP_CENTER,
                                newTile.gameObject.GetComponent<ResourceTile>());
                            newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeigbour((int) Neighbours.DOWN_CENTER, bonusTile);
                        }
                            break;
                        case Neighbours.DOWN_LEFT:
                        {
                            newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeigbour((int) Neighbours.UP_RIGHT, null);
                            bonusNeighbourC.SetNeigbour((int) Neighbours.DOWN_CENTER,
                                newTile.gameObject.GetComponent<ResourceTile>());
                            newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeigbour((int) Neighbours.UP_CENTER, bonusTile);
                        }
                            break;
                        case Neighbours.DOWN_CENTER:
                        {
                            newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeigbour((int) Neighbours.UP_CENTER, null);
                            bonusNeighbourC.SetNeigbour((int) Neighbours.DOWN_RIGHT,
                                newTile.gameObject.GetComponent<ResourceTile>());
                            newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeigbour((int) Neighbours.UP_LEFT, bonusTile);
                        }
                            break;
                        case Neighbours.DOWN_RIGHT:
                        {
                            newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeigbour((int) Neighbours.UP_LEFT, null);
                            bonusNeighbourC.SetNeigbour((int) Neighbours.UP_RIGHT,
                                newTile.gameObject.GetComponent<ResourceTile>());
                            newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeigbour((int) Neighbours.DOWN_LEFT, bonusTile);
                        }
                            break;
                    }

                    i = 6;
                }
            }

        }

    }

    public void RightRotate()
    {
        Tile bonusTile = childTile.gameObject.GetComponent<Tile>();
        NeighbourController bonusNeighbourC = bonusTile.gameObject.GetComponent<NeighbourController>();
        if (bonusTile)
        {
            rotationIndicatorParent.transform.Rotate(new Vector3(0, 0, -60));
            Vector3 localPosition = rotationIndicatorChild.transform.localPosition;
            Vector3Int cellPosition = grid.LocalToCell(localPosition);
            localPosition = grid.CellToLocal(new Vector3Int(cellPosition.x, cellPosition.y));
            rotationIndicatorChild.transform.localPosition = new Vector3(localPosition.x, localPosition.y,
                rotationIndicatorParent.transform.position.z);
            for (int i = 0; i < 6; i++)
            {
                NeighbourController newNeighbourC = newTile.gameObject.GetComponent<NeighbourController>();
                newNeighbourC.SetNeigbour(i, null);
                if (bonusNeighbourC.neighboursFree[i])
                {
                    bonusNeighbourC.SetNeigbour(i, null);
                    Neighbours index = (Neighbours) i;
                    RotateStartAngle(startAngleNeighbours, true);
                    switch (index)
                    {
                        case Neighbours.UP_LEFT:
                        {
                            newNeighbourC.SetNeigbour((int) Neighbours.DOWN_RIGHT, null);
                            bonusNeighbourC.SetNeigbour((int) Neighbours.UP_CENTER,
                                newTile.gameObject.GetComponent<ResourceTile>());
                            newNeighbourC.SetNeigbour((int) Neighbours.DOWN_CENTER, bonusTile);
                        }
                            break;
                        case Neighbours.UP_CENTER:
                        {
                            newNeighbourC.SetNeigbour((int) Neighbours.DOWN_CENTER, null);
                            bonusNeighbourC.SetNeigbour((int) Neighbours.UP_RIGHT,
                                newTile.gameObject.GetComponent<ResourceTile>());
                            newNeighbourC.SetNeigbour((int) Neighbours.DOWN_LEFT, bonusTile);
                        }
                            break;
                        case Neighbours.UP_RIGHT:
                        {
                            newNeighbourC.SetNeigbour((int) Neighbours.DOWN_LEFT, null);
                            bonusNeighbourC.SetNeigbour((int) Neighbours.DOWN_RIGHT,
                                newTile.gameObject.GetComponent<ResourceTile>());
                            newNeighbourC.SetNeigbour((int) Neighbours.UP_LEFT, bonusTile);
                        }
                            break;
                        case Neighbours.DOWN_LEFT:
                        {
                            newNeighbourC.SetNeigbour((int) Neighbours.UP_RIGHT, null);
                            bonusNeighbourC.SetNeigbour((int) Neighbours.UP_LEFT,
                                newTile.gameObject.GetComponent<ResourceTile>());
                            newNeighbourC.SetNeigbour((int) Neighbours.DOWN_RIGHT, bonusTile);
                        }
                            break;
                        case Neighbours.DOWN_CENTER:
                        {
                            newNeighbourC.SetNeigbour((int) Neighbours.UP_CENTER, null);
                            bonusNeighbourC.SetNeigbour((int) Neighbours.DOWN_LEFT,
                                newTile.gameObject.GetComponent<ResourceTile>());
                            newNeighbourC.SetNeigbour((int) Neighbours.UP_RIGHT, bonusTile);
                        }
                            break;
                        case Neighbours.DOWN_RIGHT:
                        {
                            newNeighbourC.SetNeigbour((int) Neighbours.UP_LEFT, null);
                            bonusNeighbourC.SetNeigbour((int) Neighbours.DOWN_CENTER,
                                newTile.gameObject.GetComponent<ResourceTile>());
                            newNeighbourC.SetNeigbour((int) Neighbours.UP_CENTER, bonusTile);
                        }
                            break;
                    }

                    i = 6;
                }
            }
        }
    }

    private void RotateStartAngle(Neighbours prev, bool clockwise)
    {
        switch (prev)
        {
            case Neighbours.UP_LEFT:
            {
                startAngleNeighbours = (clockwise) ? Neighbours.DOWN_LEFT : Neighbours.UP_CENTER;
                break;
            }
            case Neighbours.UP_CENTER:
            {
                startAngleNeighbours = (clockwise) ? Neighbours.UP_RIGHT : Neighbours.UP_LEFT;
                break;
            }
            case Neighbours.UP_RIGHT:
            {
                startAngleNeighbours = (clockwise) ? Neighbours.DOWN_RIGHT : Neighbours.UP_CENTER;
                break;
            }
            case Neighbours.DOWN_LEFT:
            {
                startAngleNeighbours = (clockwise) ? Neighbours.UP_LEFT : Neighbours.DOWN_CENTER;
                break;
            }
            case Neighbours.DOWN_CENTER:
            {
                startAngleNeighbours = (clockwise) ? Neighbours.DOWN_LEFT : Neighbours.DOWN_RIGHT;
                break;
            }
            case Neighbours.DOWN_RIGHT:
            {
                startAngleNeighbours = (clockwise) ? Neighbours.DOWN_CENTER : Neighbours.UP_RIGHT;
                break;
            }
        }
    }


public GameObject SetNewTile(Vector3 position)
    {
        GameObject tile = Instantiate(newTile, tileHolder.transform);
        tile.transform.position = position;
        TilePeopleArrived?.Invoke((int)UnityEngine.Random.Range(minPeople, maxPeople));
        Destroy(newTile.gameObject);
        return tile;
    }
}
