using System;
using UnityEngine;

namespace Tools.Timer
{
    public class Timer : MonoBehaviour
    {
        [Header("Settings")]
        public bool timerOn;
        public float startDelay;
        public float delay;

        public Action OnTimerDone;

        private void Update()
        {
            if (!timerOn) return;
            delay -= Time.deltaTime;

            if (delay > 0) return;
            delay = 0;
            timerOn = false;
            OnTimerDone?.Invoke();
        }

        public void StartTimer()
        {
            delay = startDelay;
            timerOn = true;
        }
        
        public void StopTimer()
        {
            timerOn = false;
        }

        public void SetBiggerDelay(float otherDelay)
        {
            if (delay < otherDelay) delay = otherDelay;
        }

        public void SetDelay(float newDelay)
        {
            startDelay = newDelay;
        }

        public void AddDelay(float additionalDelay)
        {
            delay += additionalDelay;
        }
    }
}