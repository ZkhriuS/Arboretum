using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Edited;
using Tiles;
using UnityEngine;
using GameObject = UnityEngine.GameObject;

public class PlacementSystem : MonoBehaviour
{
    public TileGenerator generator;
    public GameObject touchIndicator;
    public InputManager inputManager;
    public Grid grid;
    public List<GameObject> starterTiles;
    public TileObjectGrid tileGrid;

    private void Start()
    {
        if (starterTiles is not { Count: > 0 }) return;
        starterTiles[(int)Neighbour.UP_LEFT].transform.position = grid.CellToWorld(new Vector3Int(0, -1));
        tileGrid.AddTileObject(starterTiles[(int)Neighbour.UP_LEFT],new Vector3Int(0, -1));
        starterTiles[(int)Neighbour.UP_CENTER].transform.position = grid.CellToWorld(new Vector3Int(1, 0));
        tileGrid.AddTileObject(starterTiles[(int)Neighbour.UP_CENTER],new Vector3Int(1, 0));
        starterTiles[(int)Neighbour.UP_RIGHT].transform.position = grid.CellToWorld(new Vector3Int(0, 1));
        tileGrid.AddTileObject(starterTiles[(int)Neighbour.UP_RIGHT],new Vector3Int(0, 1));
        starterTiles[(int)Neighbour.DOWN_LEFT].transform.position = grid.CellToWorld(new Vector3Int(-1, -1));
        tileGrid.AddTileObject(starterTiles[(int)Neighbour.DOWN_LEFT],new Vector3Int(-1, -1));
        starterTiles[(int)Neighbour.DOWN_CENTER].transform.position = grid.CellToWorld(new Vector3Int(-1, 0));
        tileGrid.AddTileObject(starterTiles[(int)Neighbour.DOWN_CENTER],new Vector3Int(-1, 0));
        starterTiles[(int)Neighbour.DOWN_RIGHT].transform.position = grid.CellToWorld(new Vector3Int(-1, 1));
        tileGrid.AddTileObject(starterTiles[(int)Neighbour.DOWN_RIGHT],new Vector3Int(-1, 1));
        starterTiles[6].transform.position = grid.CellToWorld(new Vector3Int(0, 0));
        tileGrid.AddTileObject(starterTiles[6],new Vector3Int(0, 0));
    }

    protected virtual void Update()
    {
        var tile = generator.GetNewTile();
        if (!tile) return;
        touchIndicator = tile.TryGetComponent(out ResourceTile resourceTile) ? resourceTile.gameObject : null;
        if (touchIndicator)
        {
            Vector3 touchPosition = inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(touchPosition);
            touchIndicator.transform.position = grid.CellToWorld(gridPosition);
        }
    }

    public void ResetTouchIndicator()
    {
        generator.DeleteTile();
        touchIndicator = null;
    }

    public virtual void SetTile(Tile baseTile)
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
        
        foreach (var tile in nextTileNeighbourController.neighbours)
        {
            if (tile.Value)
            {
                ScanTilesAround(grid.WorldToCell(tile.Value.gameObject.transform.position), tile.Value.GetComponent<NeighbourController>());
            }
        }
        foreach (var tile in childTileNeighbourController.neighbours)
        {
            if (tile.Value)
            {
                ScanTilesAround(grid.WorldToCell(tile.Value.gameObject.transform.position), tile.Value.GetComponent<NeighbourController>());
            }
        }
        
        nextTile.GetComponent<ChainController>().IncreaseScore(baseTile.GetComponent<BonusTile>());
        
        Destroy(baseTile.gameObject);
    }

    public Tile HitTile(Vector3Int origin)
    {
        return tileGrid.FindTileObject(origin)?.GetTileObject().GetComponent<Tile>();
    }

    public void ScanTilesAround(Vector3Int cell, NeighbourController tile)
    {
        if(tile == null) return;
        
        var other = new SerializedDictionary<Neighbour, Tile>
        {
            { Neighbour.UP_LEFT, HitTile(cell + new Vector3Int(Math.Abs(cell.y) % 2, -1)) },
            { Neighbour.UP_CENTER, HitTile(cell + new Vector3Int(1, 0)) },
            { Neighbour.UP_RIGHT, HitTile(cell + new Vector3Int(Math.Abs(cell.y) % 2, 1)) },
            { Neighbour.DOWN_LEFT, HitTile(cell + new Vector3Int(-(Math.Abs(cell.y) + 1) % 2, -1)) },
            { Neighbour.DOWN_CENTER, HitTile(cell + new Vector3Int(-1, 0)) },
            { Neighbour.DOWN_RIGHT, HitTile(cell + new Vector3Int(-(Math.Abs(cell.y) + 1) % 2, 1)) }
        };
        
        tile.neighbours = other;
    }
}
