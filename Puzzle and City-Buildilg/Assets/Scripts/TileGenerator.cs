using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

public class TileGenerator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> tileList;
    [SerializeField]
    private List<GameObject> plusTileList;
    [SerializeField]
    private List<GameObject> mulTileList;
    [FormerlySerializedAs("tileHolder")] [SerializeField]
    private GameObject holder;
    [SerializeField] 
    protected Grid grid;
    [SerializeField]
    protected GameObject rotationIndicatorParent;
    [SerializeField]
    protected GameObject rotationIndicatorChild;

    private GameObject _newTile;
    private GameObject _childTile;
    private Neighbours _startAngleNeighbours;

    void Start()
    {
        _newTile = null;
        _childTile = null;
        rotationIndicatorParent.transform.position = grid.CellToWorld(new Vector3Int(0, 0));
        rotationIndicatorChild.transform.position = grid.CellToWorld(new Vector3Int(0, -1));
        rotationIndicatorChild.transform.parent = rotationIndicatorParent.transform;
        _startAngleNeighbours = Neighbours.UP_LEFT;
    }

    void Update()
    {
        if (_newTile)
        {
            rotationIndicatorParent.transform.position = _newTile.transform.position;
            var localPosition = rotationIndicatorChild.transform.localPosition;
            localPosition = new Vector3(localPosition.x, localPosition.y, 0);
            rotationIndicatorChild.transform.localPosition = localPosition;
            _childTile.transform.position =
                grid.CellToWorld(grid.WorldToCell(rotationIndicatorChild.transform.position));
        }
    }

    public bool IsOtherActionBlocked()
    {
        return _newTile;
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
        _newTile = Instantiate(tileList[variant], holder.transform);
        variant = UnityEngine.Random.Range(0, 10);
        if (variant < 4)
        {
            variant = UnityEngine.Random.Range(0, mulTileList.Capacity);
            _childTile = Instantiate(mulTileList[variant], holder.transform);
            _childTile.transform.parent = _newTile.transform;
        }
        else
        {
            variant = UnityEngine.Random.Range(0, plusTileList.Capacity);
            _childTile = Instantiate(plusTileList[variant], holder.transform);
            _childTile.transform.parent = _newTile.transform;
        }

        Tile bonusTile = _childTile.gameObject.GetComponent<Tile>();
        bonusTile.gameObject.GetComponent<NeighbourController>().neighboursFree[bonusTile.GetOppositeIndex(_startAngleNeighbours)] = _newTile.GetComponent<ResourceTile>();
        _newTile.gameObject.GetComponent<NeighbourController>().neighboursFree[_startAngleNeighbours] = bonusTile;
    }

    public void ReleaseTile()
    {
        if(_newTile && _childTile)
        {
            _newTile.gameObject.SetActive(true);
            _childTile.gameObject.SetActive(true);
        }
    }
    
    public void HideTile()
    {
        if (_newTile && _childTile)
        {
            _newTile.gameObject.SetActive(false);
            _childTile.gameObject.SetActive(false);
        }
    }
    
    public void DeleteTile()
    {
        if (_newTile && _childTile)
        {
            _newTile = null;
            _childTile = null;
        }
    }

    public void LeftRotate()
    {
        if (_newTile && _childTile)
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
                    _newTile.gameObject.GetComponent<NeighbourController>().SetNeighbour((Neighbours) i, null);
                    if (bonusNeighbourC.neighboursFree[(Neighbours) i])
                    {
                        bonusNeighbourC.neighboursFree[(Neighbours) i] = null;
                        Neighbours index = (Neighbours) i;
                        RotateStartAngle(_startAngleNeighbours, false);
                        switch (index)
                        {
                            case Neighbours.UP_LEFT:
                            {
                                _newTile.gameObject.GetComponent<NeighbourController>()
                                    .neighboursFree[Neighbours.DOWN_RIGHT] = null;
                                bonusNeighbourC.neighboursFree[Neighbours.DOWN_LEFT] =
                                    _newTile.gameObject.GetComponent<ResourceTile>();
                                _newTile.gameObject.GetComponent<NeighbourController>()
                                    .neighboursFree[Neighbours.UP_RIGHT] = bonusTile;
                            }
                                break;
                            case Neighbours.UP_CENTER:
                            {
                                _newTile.gameObject.GetComponent<NeighbourController>()
                                    .SetNeighbour(Neighbours.DOWN_CENTER, null);
                                bonusNeighbourC.SetNeighbour(Neighbours.UP_LEFT,
                                    _newTile.gameObject.GetComponent<ResourceTile>());
                                _newTile.gameObject.GetComponent<NeighbourController>()
                                    .SetNeighbour(Neighbours.DOWN_RIGHT, bonusTile);
                            }
                                break;
                            case Neighbours.UP_RIGHT:
                            {
                                _newTile.gameObject.GetComponent<NeighbourController>()
                                    .SetNeighbour(Neighbours.DOWN_LEFT, null);
                                bonusNeighbourC.SetNeighbour(Neighbours.UP_CENTER,
                                    _newTile.gameObject.GetComponent<ResourceTile>());
                                _newTile.gameObject.GetComponent<NeighbourController>()
                                    .SetNeighbour(Neighbours.DOWN_CENTER, bonusTile);
                            }
                                break;
                            case Neighbours.DOWN_LEFT:
                            {
                                _newTile.gameObject.GetComponent<NeighbourController>()
                                    .SetNeighbour(Neighbours.UP_RIGHT, null);
                                bonusNeighbourC.SetNeighbour(Neighbours.DOWN_CENTER,
                                    _newTile.gameObject.GetComponent<ResourceTile>());
                                _newTile.gameObject.GetComponent<NeighbourController>()
                                    .SetNeighbour(Neighbours.UP_CENTER, bonusTile);
                            }
                                break;
                            case Neighbours.DOWN_CENTER:
                            {
                                _newTile.gameObject.GetComponent<NeighbourController>()
                                    .SetNeighbour(Neighbours.UP_CENTER, null);
                                bonusNeighbourC.SetNeighbour(Neighbours.DOWN_RIGHT,
                                    _newTile.gameObject.GetComponent<ResourceTile>());
                                _newTile.gameObject.GetComponent<NeighbourController>()
                                    .SetNeighbour(Neighbours.UP_LEFT, bonusTile);
                            }
                                break;
                            case Neighbours.DOWN_RIGHT:
                            {
                                _newTile.gameObject.GetComponent<NeighbourController>()
                                    .SetNeighbour(Neighbours.UP_LEFT, null);
                                bonusNeighbourC.SetNeighbour(Neighbours.UP_RIGHT,
                                    _newTile.gameObject.GetComponent<ResourceTile>());
                                _newTile.gameObject.GetComponent<NeighbourController>()
                                    .SetNeighbour(Neighbours.DOWN_LEFT, bonusTile);
                            }
                                break;
                        }

                        i = 6;
                    }
                }
            }
        }
    }

    public void RightRotate()
    {
        if (_newTile && _childTile)
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
                    newNeighbourC.SetNeighbour((Neighbours) i, null);
                    if (bonusNeighbourC.neighboursFree[(Neighbours) i])
                    {
                        bonusNeighbourC.SetNeighbour((Neighbours) i, null);
                        Neighbours index = (Neighbours) i;
                        RotateStartAngle(_startAngleNeighbours, true);
                        switch (index)
                        {
                            case Neighbours.UP_LEFT:
                            {
                                newNeighbourC.SetNeighbour(Neighbours.DOWN_RIGHT, null);
                                bonusNeighbourC.SetNeighbour(Neighbours.UP_CENTER,
                                    _newTile.gameObject.GetComponent<ResourceTile>());
                                newNeighbourC.SetNeighbour(Neighbours.DOWN_CENTER, bonusTile);
                            }
                                break;
                            case Neighbours.UP_CENTER:
                            {
                                newNeighbourC.SetNeighbour(Neighbours.DOWN_CENTER, null);
                                bonusNeighbourC.SetNeighbour(Neighbours.UP_RIGHT,
                                    _newTile.gameObject.GetComponent<ResourceTile>());
                                newNeighbourC.SetNeighbour(Neighbours.DOWN_LEFT, bonusTile);
                            }
                                break;
                            case Neighbours.UP_RIGHT:
                            {
                                newNeighbourC.SetNeighbour(Neighbours.DOWN_LEFT, null);
                                bonusNeighbourC.SetNeighbour(Neighbours.DOWN_RIGHT,
                                    _newTile.gameObject.GetComponent<ResourceTile>());
                                newNeighbourC.SetNeighbour(Neighbours.UP_LEFT, bonusTile);
                            }
                                break;
                            case Neighbours.DOWN_LEFT:
                            {
                                newNeighbourC.SetNeighbour(Neighbours.UP_RIGHT, null);
                                bonusNeighbourC.SetNeighbour((int) Neighbours.UP_LEFT,
                                    _newTile.gameObject.GetComponent<ResourceTile>());
                                newNeighbourC.SetNeighbour(Neighbours.DOWN_RIGHT, bonusTile);
                            }
                                break;
                            case Neighbours.DOWN_CENTER:
                            {
                                newNeighbourC.SetNeighbour(Neighbours.UP_CENTER, null);
                                bonusNeighbourC.SetNeighbour(Neighbours.DOWN_LEFT,
                                    _newTile.gameObject.GetComponent<ResourceTile>());
                                newNeighbourC.SetNeighbour(Neighbours.UP_RIGHT, bonusTile);
                            }
                                break;
                            case Neighbours.DOWN_RIGHT:
                            {
                                newNeighbourC.SetNeighbour((int) Neighbours.UP_LEFT, null);
                                bonusNeighbourC.SetNeighbour(Neighbours.DOWN_CENTER,
                                    _newTile.gameObject.GetComponent<ResourceTile>());
                                newNeighbourC.SetNeighbour(Neighbours.UP_CENTER, bonusTile);
                            }
                                break;
                        }

                        i = 6;
                    }
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
                _startAngleNeighbours = (clockwise) ? Neighbours.UP_CENTER : Neighbours.DOWN_LEFT;
                break;
            }
            case Neighbours.UP_CENTER:
            {
                _startAngleNeighbours = (clockwise) ? Neighbours.UP_RIGHT : Neighbours.UP_LEFT;
                break;
            }
            case Neighbours.UP_RIGHT:
            {
                _startAngleNeighbours = (clockwise) ? Neighbours.DOWN_RIGHT : Neighbours.UP_CENTER;
                break;
            }
            case Neighbours.DOWN_LEFT:
            {
                _startAngleNeighbours = (clockwise) ? Neighbours.UP_LEFT : Neighbours.DOWN_CENTER;
                break;
            }
            case Neighbours.DOWN_CENTER:
            {
                _startAngleNeighbours = (clockwise) ? Neighbours.DOWN_LEFT : Neighbours.DOWN_RIGHT;
                break;
            }
            case Neighbours.DOWN_RIGHT:
            {
                _startAngleNeighbours = (clockwise) ? Neighbours.DOWN_CENTER : Neighbours.UP_RIGHT;
                break;
            }
        }

        Debug.Log(_startAngleNeighbours);
    }


    public GameObject SetNewTile(Vector3 position)
    {
        GameObject tile = Instantiate(_newTile, holder.transform);
        tile.transform.position = position;
        Destroy(_newTile.gameObject);
        return tile;
    }
}
