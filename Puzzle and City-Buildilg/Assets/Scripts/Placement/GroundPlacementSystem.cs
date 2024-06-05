using Tiles;
using UnityEngine;

public class GroundPlacementSystem : PlacementSystem
{
    protected override void Update()
    {
        var tile = generator.GetNewTile();
        if (!tile) return;
        touchIndicator = tile.TryGetComponent(out BuildingTile buildingTile) ? buildingTile.gameObject : null;
        if (touchIndicator)
        {
            Vector3 touchPosition = inputManager.GetSelectedMapPosition();
            Vector3Int gridPosition = grid.WorldToCell(touchPosition);
            touchIndicator.transform.position = grid.CellToWorld(gridPosition);
        }
    }
    
    public override void SetTile(Tile baseTile)
    {
        GameObject nextTile = generator.SetNewTile(baseTile.transform.position);
        nextTile.GetComponent<Tile>().isGrounded = true;
        
        Vector3Int nextTileCellPosition = grid.WorldToCell(nextTile.transform.position);
        
        tileGrid.UpdateTileObject(nextTile, nextTileCellPosition);
        
        Destroy(baseTile.gameObject);
    }
    
    public void UpdateGroundGrid(GameObject tile, GameObject ground)
    {
        tileGrid.UpdateTileObject(ground, grid.WorldToCell(tile.gameObject.transform.position));
    }
}
