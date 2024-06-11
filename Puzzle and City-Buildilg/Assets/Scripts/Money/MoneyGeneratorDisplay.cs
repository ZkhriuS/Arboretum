using System;
using Tools.Timer;
using UnityEngine;

namespace Money
{
    public class MoneyGeneratorDisplay : MonoBehaviour
    {
        public MoneyGenerator moneyGenerator;
        public TownObjectController controller;
        [Space]
        public RectTransform timerDisplay;
        public RectTransform moneyDisplay;

        private bool _canStartGenerator = true;

        private void Start()
        {
            timerDisplay.gameObject.SetActive(false);
            moneyDisplay.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!controller.IsPlaced())
                _canStartGenerator = false;
        }

        private void OnEnable()
        {
            moneyGenerator.OnStartGenerate += ShowTimer;
            moneyGenerator.OnMoneyReady += ShowMoney;
            moneyGenerator.OnMoneyCollect += MoneyCollect;
        }
        
        private void OnDisable()
        {
            moneyGenerator.OnStartGenerate -= ShowTimer;
            moneyGenerator.OnMoneyReady -= ShowMoney;
            moneyGenerator.OnMoneyCollect -= MoneyCollect;
        }

        public void Interact(MoneyCollector collector)
        {
            if (_canStartGenerator) moneyGenerator.StartGenerate();
            
            if(moneyGenerator.CollectMoney())
                collector.CollectMoney(gameObject.GetComponent<MoneyGenerator>());
        }

        private void ShowTimer()
        {
            _canStartGenerator = false;
            timerDisplay.gameObject.SetActive(true);
        }
        
        private void ShowMoney(bool ready)
        {
            moneyDisplay.gameObject.SetActive(ready);
            timerDisplay.gameObject.SetActive(false);
        }

        private void MoneyCollect(float money)
        {
            _canStartGenerator = true;
        }
        
    }
}