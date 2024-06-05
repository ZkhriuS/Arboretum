using UnityEngine;

namespace Edited
{
    public class TileHolder : MonoBehaviour
    {
        private GameObject _bonusTileForApply;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private PlacementSystem placement;
        [SerializeField] private GroundPlacementSystem groundPlacement;
        public void Apply()
        {
            if (_bonusTileForApply != null)
            {
                BonusTile bonusTile = _bonusTileForApply.GetComponent<BonusTile>();
                if (bonusTile)
                {
                    //bonusTile.SetBaseTile();
                    placement.SetTile(bonusTile);
                    placement.ResetTouchIndicator();
                    gameManager.ChangeTileSetMode();
                }
            }
            
        }
        public void SetTriggeredBonusTile(GameObject other)
        {
            _bonusTileForApply = other;
        }

        public void UpdateGrid(GameObject tile, GameObject ground)
        {
            groundPlacement.UpdateGroundGrid(tile, ground);
        }
    }
}
