using System.Collections.Generic;
using UnityEngine;
using System;
using Tiles;

public class TileGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> tileList;
    [SerializeField]
    private List<GameObject> plusTileList;
    [SerializeField]
    private List<GameObject> mulTileList;
    [SerializeField]
    private List<GameObject> buildingTileList;
    [SerializeField]
    private GameObject tileHolder;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private GameObject rotationIndicatorParent;
    [SerializeField]
    private GameObject rotationIndicatorChild;

    private GameObject _newTile;
    private GameObject _childTile;
    private Neighbour _startAngleNeighbour;
    public static event Action<int> TilePeopleArrived;

    [SerializeField]
    private int minPeople;
    [SerializeField]
    private int maxPeople;

    void Start()
    {
        _newTile = null;
        _childTile = null;
        rotationIndicatorParent.transform.position = grid.CellToWorld(new Vector3Int(0, 0));
        rotationIndicatorChild.transform.position = grid.CellToWorld(new Vector3Int(0, -1));
        rotationIndicatorChild.transform.parent = rotationIndicatorParent.transform;
        _startAngleNeighbour = Neighbour.UP_LEFT;
    }

    void Update()
    {
        if (_newTile)
        {
            rotationIndicatorParent.transform.position = _newTile.transform.position;
            var localPosition = rotationIndicatorChild.transform.localPosition;
            localPosition = new Vector3(localPosition.x, localPosition.y, 0);
            rotationIndicatorChild.transform.localPosition = localPosition;
            if (!_childTile) return;
            _childTile.transform.position =
                grid.CellToWorld(grid.WorldToCell(rotationIndicatorChild.transform.position));
        }
    }

    public bool IsOtherActionBlocked()
    {
        return _newTile && _newTile.gameObject.activeSelf;
    }

    public GameObject GetNewTile()
    {
        return _newTile;
    }

    public void AddTile()
    {
        if (_newTile && _childTile)
        {
            ReleaseTile();
            return;
        }
        
        int variant = UnityEngine.Random.Range(0, tileList.Capacity);
        _newTile = Instantiate(tileList[variant], tileHolder.transform);
        variant = UnityEngine.Random.Range(0, 10);
        if (variant < 4)
        {
            variant = UnityEngine.Random.Range(0, mulTileList.Capacity);
            _childTile = Instantiate(mulTileList[variant], tileHolder.transform);
            _childTile.transform.parent = _newTile.transform;
        }
        else
        {
            variant = UnityEngine.Random.Range(0, plusTileList.Capacity);
            _childTile = Instantiate(plusTileList[variant], tileHolder.transform);
            _childTile.transform.parent = _newTile.transform;
        }

        Tile bonusTile = _childTile.gameObject.GetComponent<Tile>();
        bonusTile.gameObject.GetComponent<NeighbourController>().neighbours[Tile.GetOppositeIndex(_startAngleNeighbour)] = _newTile.GetComponent<ResourceTile>();
        _newTile.gameObject.GetComponent<NeighbourController>().neighbours[_startAngleNeighbour] = bonusTile;
    }

    public void AddBuildingTile(int i)
    {
        _newTile = Instantiate(buildingTileList[i], tileHolder.transform);
    }

    public void ReleaseTile()
    {
        _newTile.gameObject.SetActive(true);
        if (!_childTile) return;
        _childTile.gameObject.SetActive(true);
    }
    
    public void HideTile()
    {
        if (_newTile.TryGetComponent(out BuildingTile _))
        {
            Destroy(_newTile);
            _newTile = null;
            return;
        }
        
        _newTile.gameObject.SetActive(false);
        if (!_childTile) return;
        _childTile.gameObject.SetActive(false);
    }
    
    public void DeleteTile()
    {
        _newTile = null;
        _childTile = null;
    }

    public void LeftRotate()
    {
        Tile bonusTile = _childTile.gameObject.GetComponent<Tile>();
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
                _newTile.gameObject.GetComponent<NeighbourController>().SetNeighbour((Neighbour)i, null);
                if (bonusNeighbourC.neighbours[(Neighbour)i])
                {
                    bonusNeighbourC.neighbours[(Neighbour)i] = null;
                    Neighbour index = (Neighbour)i;
                    RotateStartAngle(_startAngleNeighbour, false);
                    switch (index)
                    {
                        case Neighbour.UP_LEFT:
                        {
                            _newTile.gameObject.GetComponent<NeighbourController>().neighbours[Neighbour.DOWN_RIGHT] = null;
                            bonusNeighbourC.neighbours[Neighbour.DOWN_LEFT] = _newTile.gameObject.GetComponent<ResourceTile>();
                            _newTile.gameObject.GetComponent<NeighbourController>().neighbours[Neighbour.UP_RIGHT] = bonusTile;
                        }
                            break;
                        case Neighbour.UP_CENTER:
                        {
                            _newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeighbour(Neighbour.DOWN_CENTER, null);
                            bonusNeighbourC.SetNeighbour(Neighbour.UP_LEFT,
                                _newTile.gameObject.GetComponent<ResourceTile>());
                            _newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeighbour(Neighbour.DOWN_RIGHT, bonusTile);
                        }
                            break;
                        case Neighbour.UP_RIGHT:
                        {
                            _newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeighbour(Neighbour.DOWN_LEFT, null);
                            bonusNeighbourC.SetNeighbour(Neighbour.UP_CENTER,
                                _newTile.gameObject.GetComponent<ResourceTile>());
                            _newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeighbour(Neighbour.DOWN_CENTER, bonusTile);
                        }
                            break;
                        case Neighbour.DOWN_LEFT:
                        {
                            _newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeighbour(Neighbour.UP_RIGHT, null);
                            bonusNeighbourC.SetNeighbour(Neighbour.DOWN_CENTER,
                                _newTile.gameObject.GetComponent<ResourceTile>());
                            _newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeighbour(Neighbour.UP_CENTER, bonusTile);
                        }
                            break;
                        case Neighbour.DOWN_CENTER:
                        {
                            _newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeighbour(Neighbour.UP_CENTER, null);
                            bonusNeighbourC.SetNeighbour(Neighbour.DOWN_RIGHT,
                                _newTile.gameObject.GetComponent<ResourceTile>());
                            _newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeighbour(Neighbour.UP_LEFT, bonusTile);
                        }
                            break;
                        case Neighbour.DOWN_RIGHT:
                        {
                            _newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeighbour(Neighbour.UP_LEFT, null);
                            bonusNeighbourC.SetNeighbour(Neighbour.UP_RIGHT,
                                _newTile.gameObject.GetComponent<ResourceTile>());
                            _newTile.gameObject.GetComponent<NeighbourController>()
                                .SetNeighbour(Neighbour.DOWN_LEFT, bonusTile);
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
        Tile bonusTile = _childTile.gameObject.GetComponent<Tile>();
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
                NeighbourController newNeighbourC = _newTile.gameObject.GetComponent<NeighbourController>();
                newNeighbourC.SetNeighbour((Neighbour)i, null);
                if (bonusNeighbourC.neighbours[(Neighbour)i])
                {
                    bonusNeighbourC.SetNeighbour((Neighbour)i, null);
                    Neighbour index = (Neighbour)i;
                    RotateStartAngle(_startAngleNeighbour, true);
                    switch (index)
                    {
                        case Neighbour.UP_LEFT:
                        {
                            newNeighbourC.SetNeighbour(Neighbour.DOWN_RIGHT, null);
                            bonusNeighbourC.SetNeighbour(Neighbour.UP_CENTER,
                                _newTile.gameObject.GetComponent<ResourceTile>());
                            newNeighbourC.SetNeighbour(Neighbour.DOWN_CENTER, bonusTile);
                        }
                            break;
                        case Neighbour.UP_CENTER:
                        {
                            newNeighbourC.SetNeighbour(Neighbour.DOWN_CENTER, null);
                            bonusNeighbourC.SetNeighbour(Neighbour.UP_RIGHT,
                                _newTile.gameObject.GetComponent<ResourceTile>());
                            newNeighbourC.SetNeighbour(Neighbour.DOWN_LEFT, bonusTile);
                        }
                            break;
                        case Neighbour.UP_RIGHT:
                        {
                            newNeighbourC.SetNeighbour(Neighbour.DOWN_LEFT, null);
                            bonusNeighbourC.SetNeighbour(Neighbour.DOWN_RIGHT,
                                _newTile.gameObject.GetComponent<ResourceTile>());
                            newNeighbourC.SetNeighbour(Neighbour.UP_LEFT, bonusTile);
                        }
                            break;
                        case Neighbour.DOWN_LEFT:
                        {
                            newNeighbourC.SetNeighbour(Neighbour.UP_RIGHT, null);
                            bonusNeighbourC.SetNeighbour((int)Neighbour.UP_LEFT,
                                _newTile.gameObject.GetComponent<ResourceTile>());
                            newNeighbourC.SetNeighbour(Neighbour.DOWN_RIGHT, bonusTile);
                        }
                            break;
                        case Neighbour.DOWN_CENTER:
                        {
                            newNeighbourC.SetNeighbour(Neighbour.UP_CENTER, null);
                            bonusNeighbourC.SetNeighbour(Neighbour.DOWN_LEFT,
                                _newTile.gameObject.GetComponent<ResourceTile>());
                            newNeighbourC.SetNeighbour(Neighbour.UP_RIGHT, bonusTile);
                        }
                            break;
                        case Neighbour.DOWN_RIGHT:
                        {
                            newNeighbourC.SetNeighbour((int)Neighbour.UP_LEFT, null);
                            bonusNeighbourC.SetNeighbour(Neighbour.DOWN_CENTER,
                                _newTile.gameObject.GetComponent<ResourceTile>());
                            newNeighbourC.SetNeighbour(Neighbour.UP_CENTER, bonusTile);
                        }
                            break;
                    }

                    i = 6;
                }
            }
        }
    }

    private void RotateStartAngle(Neighbour prev, bool clockwise)
    {
        switch (prev)
        {
            case Neighbour.UP_LEFT:
            {
                _startAngleNeighbour = (clockwise) ? Neighbour.UP_CENTER : Neighbour.DOWN_LEFT;
                break;
            }
            case Neighbour.UP_CENTER:
            {
                _startAngleNeighbour = (clockwise) ? Neighbour.UP_RIGHT : Neighbour.UP_LEFT;
                break;
            }
            case Neighbour.UP_RIGHT:
            {
                _startAngleNeighbour = (clockwise) ? Neighbour.DOWN_RIGHT : Neighbour.UP_CENTER;
                break;
            }
            case Neighbour.DOWN_LEFT:
            {
                _startAngleNeighbour = (clockwise) ? Neighbour.UP_LEFT : Neighbour.DOWN_CENTER;
                break;
            }
            case Neighbour.DOWN_CENTER:
            {
                _startAngleNeighbour = (clockwise) ? Neighbour.DOWN_LEFT : Neighbour.DOWN_RIGHT;
                break;
            }
            case Neighbour.DOWN_RIGHT:
            {
                _startAngleNeighbour = (clockwise) ? Neighbour.DOWN_CENTER : Neighbour.UP_RIGHT;
                break;
            }
        }

        Debug.Log(_startAngleNeighbour);
    }


    public GameObject SetNewTile(Vector3 position)
    {
        GameObject tile = Instantiate(_newTile, tileHolder.transform);
        tile.transform.position = position;
        TilePeopleArrived?.Invoke(UnityEngine.Random.Range(minPeople, maxPeople));
        Destroy(_newTile.gameObject);
        return tile;
    }
}
