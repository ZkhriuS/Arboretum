using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Edited;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using GameObject = UnityEngine.GameObject;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject touchIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    [SerializeField] private TileGenerator generator;
    [SerializeField] private List<GameObject> starterTiles;
    [SerializeField] private TileObjectGrid tileGrid;

    private void Start()
    {
        starterTiles[(int)Neighbours.UP_LEFT].transform.position = grid.CellToWorld(new Vector3Int(0, -1));
        tileGrid.AddTileObject(starterTiles[(int)Neighbours.UP_LEFT],new Vector3Int(0, -1));
        starterTiles[(int)Neighbours.UP_CENTER].transform.position = grid.CellToWorld(new Vector3Int(1, 0));
        tileGrid.AddTileObject(starterTiles[(int)Neighbours.UP_CENTER],new Vector3Int(1, 0));
        starterTiles[(int)Neighbours.UP_RIGHT].transform.position = grid.CellToWorld(new Vector3Int(0, 1));
        tileGrid.AddTileObject(starterTiles[(int)Neighbours.UP_RIGHT],new Vector3Int(0, 1));
        starterTiles[(int)Neighbours.DOWN_LEFT].transform.position = grid.CellToWorld(new Vector3Int(-1, -1));
        tileGrid.AddTileObject(starterTiles[(int)Neighbours.DOWN_LEFT],new Vector3Int(-1, -1));
        starterTiles[(int)Neighbours.DOWN_CENTER].transform.position = grid.CellToWorld(new Vector3Int(-1, 0));
        tileGrid.AddTileObject(starterTiles[(int)Neighbours.DOWN_CENTER],new Vector3Int(-1, 0));
        starterTiles[(int)Neighbours.DOWN_RIGHT].transform.position = grid.CellToWorld(new Vector3Int(-1, 1));
        tileGrid.AddTileObject(starterTiles[(int)Neighbours.DOWN_RIGHT],new Vector3Int(-1, 1));
        starterTiles[6].transform.position = grid.CellToWorld(new Vector3Int(0, 0));
        tileGrid.AddTileObject(starterTiles[6],new Vector3Int(0, 0));
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

    public void ResetTouchIndicator()
    {
        touchIndicator = null;
    }

    public void SetTile(Tile baseTile)
    {
        GameObject nextTile = generator.SetNewTile(baseTile.transform.position);
        nextTile.GetComponent<Tile>().isGrounded = true;
        
        NeighbourController nextTileNeighbourController = nextTile.GetComponent<NeighbourController>();
        NeighbourController childTileNeighbourController = nextTile.GetComponentsInChildren<NeighbourController>()[1];
        
        GameObject childTile = childTileNeighbourController.gameObject;
        
        Vector3Int nextTileCellPosition = grid.WorldToCell(nextTile.transform.position);
        Vector3Int childTileCellPosition = grid.WorldToCell(childTile.transform.position);
        
        tileGrid.UpdateTileObject(nextTile, nextTileCellPosition);
        tileGrid.AddTileObject(childTile,childTileCellPosition);
        
        ScanTilesAround(nextTileCellPosition, nextTileNeighbourController);
        ScanTilesAround(childTileCellPosition, childTileNeighbourController);
        
        foreach (var tile in nextTileNeighbourController.neighboursFree)
        {
            if (tile.Value)
            {
                ScanTilesAround(grid.WorldToCell(tile.Value.gameObject.transform.position), tile.Value.GetComponent<NeighbourController>());
            }
        }
        foreach (var tile in childTileNeighbourController.neighboursFree)
        {
            if (tile.Value)
            {
                ScanTilesAround(grid.WorldToCell(tile.Value.gameObject.transform.position), tile.Value.GetComponent<NeighbourController>());
            }
        }
        
        nextTile.GetComponent<ChainController>().IncreaseScore(baseTile.GetComponent<BonusTile>());
        
        Destroy(baseTile.gameObject);
    }

    private Tile HitTile(Vector3Int origin)
    {
        TileObject tileObject = tileGrid.FindTileObject(origin);
        return tileObject?.GetTileObject().GetComponent<Tile>();
    }

    private void ScanTilesAround(Vector3Int cell, NeighbourController tile)
    {
        var other = new SerializedDictionary<Neighbours, Tile>
        {
            { Neighbours.UP_LEFT, HitTile(cell + new Vector3Int(Math.Abs(cell.y) % 2, -1)) },
            { Neighbours.UP_CENTER, HitTile(cell + new Vector3Int(1, 0)) },
            { Neighbours.UP_RIGHT, HitTile(cell + new Vector3Int(Math.Abs(cell.y) % 2, 1)) },
            { Neighbours.DOWN_LEFT, HitTile(cell + new Vector3Int(-(Math.Abs(cell.y) + 1) % 2, -1)) },
            { Neighbours.DOWN_CENTER, HitTile(cell + new Vector3Int(-1, 0)) },
            { Neighbours.DOWN_RIGHT, HitTile(cell + new Vector3Int(-(Math.Abs(cell.y) + 1) % 2, 1)) }
        };
        
        tile.neighboursFree = other;
    }
}
