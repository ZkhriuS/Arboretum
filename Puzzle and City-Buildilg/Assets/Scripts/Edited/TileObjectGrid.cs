using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Edited
{
    public class TileObjectGrid : MonoBehaviour
    {
        private List<TileObject> grid;

        private void Start()
        {
            grid = new List<TileObject>();
        }

        public TileObject FindTileObject(Vector3Int cell)
        {
            return grid.FirstOrDefault(tile => tile.GetCell() == cell);
        }
        
        public void AddTileObject(GameObject tile, Vector3Int cell)
        {
            grid.Add(new TileObject(tile, cell));
        }
        
        public void UpdateTileObject(GameObject newTile, Vector3Int cell)
        {
            FindTileObject(cell)?.UpdateTileObject(newTile);
        }
    }
}
