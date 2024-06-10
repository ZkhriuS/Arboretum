using TMPro;
using UnityEngine;

namespace Money
{
    public class MoneyCollector : MonoBehaviour
    {
        [SerializeField] private MoneyOperator moneyOperator;
        private int value;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit))
                {
                    if (hit.collider.TryGetComponent(out MoneyGeneratorDisplay moneyGeneratorDisplay))
                    {
                        moneyGeneratorDisplay.Interact(this);
                    }
                }
            }
        }

        public void CollectMoney(MoneyGenerator generator)
        {
            float addMoney = generator.moneyForGenerate;
            moneyOperator.CollectMoney(addMoney);
        }
    }
}