using UnityEngine;

namespace Edited
{
    public class TileHolder : MonoBehaviour
    {
        private GameObject _bonusTileForApply;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private PlacementSystem placement;
        public void Apply()
        {
            if (_bonusTileForApply != null)
            {
                BonusTile bonusTile = _bonusTileForApply.GetComponent<BonusTile>();
                if (bonusTile)
                {
                    bonusTile.SetBaseTile();
                    placement.ResetTouchIndicator();
                    gameManager.ChangeTileSetMode();
                }
            }
            
        }
        public void SetTriggeredBonusTile(GameObject other)
        {
            _bonusTileForApply = other;
        }
    }
}
