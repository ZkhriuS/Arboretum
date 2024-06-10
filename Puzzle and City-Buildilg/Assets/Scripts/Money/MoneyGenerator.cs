using System;
using Tools.References;
using Tools.Timer;
using UnityEngine;

namespace Money
{
    [RequireComponent(typeof(Timer))]
    public class MoneyGenerator : MonoBehaviour
    {
        public float moneyForGenerate;
        public float timeToGenerate;
        [Space]
        public FloatReference money;

        public Action OnStartGenerate;
        public Action<float> OnMoneyCollect;
        public Action<bool> OnMoneyReady;
        
        private bool _canCollect;
        private Timer _timer;

        private void Awake()
        {
            _timer = GetComponent<Timer>();
        }

        private void OnEnable()
        {
            _timer.OnTimerDone += MoneyReady;
        }
        
        private void OnDisable()
        {
            _timer.OnTimerDone -= MoneyReady;
        }

        private void Start()
        {
            Reload();
        }

        private void Reload()
        {
            MoneyNotReady();
            _timer.SetDelay(timeToGenerate);
            _timer.StopTimer();
        }
        
        public void StartGenerate()
        {
            if (_timer.timerOn) return;
            
            _timer.StartTimer();
            OnStartGenerate?.Invoke();
        }

        public void MoneyReady()
        {
            _canCollect = true;
            OnMoneyReady?.Invoke(true);
        }

        public void MoneyNotReady()
        {
            _canCollect = false;
            OnMoneyReady?.Invoke(false);
        }
        
        public bool CollectMoney()
        {
            if (!_canCollect) return false;
            
            money.Value += moneyForGenerate;
            OnMoneyCollect?.Invoke(moneyForGenerate);
            Reload();
            return true;
        }
    }
}