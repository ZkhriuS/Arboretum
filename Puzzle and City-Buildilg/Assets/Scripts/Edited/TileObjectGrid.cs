using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edited
{
    public class TileObjectGrid : MonoBehaviour
    {
        [SerializeField] private List<TileObject> grid;

        private void Start()
        {
            grid = new List<TileObject>();
        }

        public TileObject FindTileObject(Vector3Int cell)
        {
            foreach (var tile in grid)
            {
                if (tile.GetCell().Equals(cell))
                {
                    return tile;
                }
            }

            return null;
        }

        public void AddTileObject(GameObject tile, Vector3Int cell)
        {
            TileObject tileObject = new TileObject(tile,cell);
            grid.Add(tileObject);
        }
        
        public void UpdateTileObject(GameObject newTile, Vector3Int cell)
        {
            TileObject tileObject = FindTileObject(cell);
            tileObject.UpdateTileObject(newTile);
        }
    }
}
