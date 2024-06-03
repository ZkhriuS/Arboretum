using System;
using System.Collections;
using System.Collections.Generic;
using Edited;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

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
        nextTile.GetComponent<ChainController>().IncreaseScore();
        
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
            if (tile)
            {
                ScanTilesAround(grid.WorldToCell(tile.gameObject.transform.position), tile.GetComponent<NeighbourController>());
            }
        }
        foreach (var tile in childTileNeighbourController.neighboursFree)
        {
            if (tile)
            {
                ScanTilesAround(grid.WorldToCell(tile.gameObject.transform.position), tile.GetComponent<NeighbourController>());
            }
        }
        Destroy(baseTile.gameObject);
    }

    private GameObject HitTile(Vector3Int origin)
    {
        TileObject tileObject = tileGrid.FindTileObject(origin);
        return tileObject?.GetTileObject();
    }

    private void ScanTilesAround(Vector3Int cell, NeighbourController tile)
    {
        GameObject[] other = new GameObject[6];
        int index = (int) Neighbours.UP_LEFT;
        other[index] = HitTile(cell + new Vector3Int(cell.y%2, -1));
        tile.neighboursFree[index] = (other[index])? other[index].GetComponent<Tile>():null;
        index = (int) Neighbours.UP_CENTER;
        other[index] = HitTile(cell + new Vector3Int(1, 0));
        tile.neighboursFree[index] = (other[index])? other[index].GetComponent<Tile>():null;
        index = (int) Neighbours.UP_RIGHT;
        other[index] = HitTile(cell + new Vector3Int(cell.y%2, 1));
        tile.neighboursFree[index] = (other[index])? other[index].GetComponent<Tile>():null;
        index = (int) Neighbours.DOWN_LEFT;
        other[index] = HitTile(cell + new Vector3Int(-(cell.y+1)%2, -1));
        tile.neighboursFree[index] = (other[index])? other[index].GetComponent<Tile>():null;
        index = (int) Neighbours.DOWN_CENTER;
        other[index] = HitTile(cell + new Vector3Int(-1, 0));
        tile.neighboursFree[index] = (other[index])? other[index].GetComponent<Tile>():null;
        index = (int) Neighbours.DOWN_RIGHT;
        other[index] = HitTile(cell + new Vector3Int(-(cell.y+1)%2, 1));
        tile.neighboursFree[index] = (other[index])? other[index].GetComponent<Tile>():null;
    }
}
