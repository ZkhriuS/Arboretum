using UnityEngine;

namespace Edited
{
    public class TileObject
    {
        private Vector3Int _cellPosition;
        private GameObject _tile;
        public TileObject(GameObject tile, Vector3Int cell)
        {
            _tile = tile;
            _cellPosition = cell;
        }

        public void UpdateTileObject(GameObject tile)
        {
            _tile = tile;
        }

        public GameObject GetTileObject()
        {
            return _tile;
        }

        public Vector3Int GetCell()
        {
            return _cellPosition;
        }
    }
}
