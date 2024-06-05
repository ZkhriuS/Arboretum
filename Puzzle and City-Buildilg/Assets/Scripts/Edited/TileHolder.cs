using Tiles;
using UnityEngine;

namespace Edited
{
    public class TileHolder : MonoBehaviour
    {
        private GameObject _tileForApply;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private PlacementSystem placement;
        [SerializeField] private GroundPlacementSystem groundPlacement;
        public void Apply()
        {
            if (_tileForApply != null)
            {
                if (_tileForApply.TryGetComponent(out BonusTile bonusTile))
                {
                    placement.SetTile(bonusTile);
                    placement.ResetTouchIndicator();
                    gameManager.ChangeTileSetMode();
                } else if (_tileForApply.TryGetComponent(out GroundTile groundTile))
                {
                    groundPlacement.SetTile(groundTile);
                    groundPlacement.ResetTouchIndicator();
                    gameManager.ChangeTileSetMode();
                }
            }
            
        }
        public void SetTriggeredBonusTile(GameObject other)
        {
            _tileForApply = other;
        }

        public void UpdateGrid(GameObject tile, GameObject ground)
        {
            groundPlacement.UpdateGroundGrid(tile, ground);
        }
    }
}
