using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject touchIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private TileGenerator generator;
    [SerializeField] private List<GameObject> starterTiles;

    private void Start()
    {
        starterTiles[(int)Neighbours.UP_LEFT].transform.position = grid.CellToWorld(new Vector3Int(0, -1));
        starterTiles[(int)Neighbours.UP_CENTER].transform.position = grid.CellToWorld(new Vector3Int(1, 0));
        starterTiles[(int)Neighbours.UP_RIGHT].transform.position = grid.CellToWorld(new Vector3Int(0, 1));
        starterTiles[(int)Neighbours.DOWN_LEFT].transform.position = grid.CellToWorld(new Vector3Int(-1, 1));
        starterTiles[(int)Neighbours.DOWN_CENTER].transform.position = grid.CellToWorld(new Vector3Int(-1, 0));
        starterTiles[(int)Neighbours.DOWN_RIGHT].transform.position = grid.CellToWorld(new Vector3Int(-1, -1));
        starterTiles[6].transform.position = grid.CellToWorld(new Vector3Int(0, 0));
    }

    private void Update()
    {
        touchIndicator = generator.GetNewTile();
        if (touchIndicator)
        {
            Vector3 touchPosition = inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(touchPosition);
            touchIndicator.transform.position = grid.CellToWorld(gridPosition);
        }
    }

    public void Apply()
    {
        if (touchIndicator != null)
        {
            BonusTile bonusTile = touchIndicator.GetComponent<BonusTile>();
            if(bonusTile)
                bonusTile.SetBaseTile();
            
        }
        touchIndicator = null;
    }
}
