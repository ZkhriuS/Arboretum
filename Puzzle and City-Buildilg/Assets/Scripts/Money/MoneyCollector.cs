using UnityEngine;

namespace Money
{
    public class MoneyCollector : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit))
                {
                    if (hit.collider.TryGetComponent(out MoneyGeneratorDisplay moneyGeneratorDisplay))
                    {
                        moneyGeneratorDisplay.Interact();
                    }
                }
            }
        }
    }
}