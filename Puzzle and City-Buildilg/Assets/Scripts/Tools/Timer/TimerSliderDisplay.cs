using UnityEngine.UI;

namespace Tools.Timer
{
    public class TimerSliderDisplay : TimerDisplay
    {
        public Slider slider;

        private void Update()
        {
            slider.value = timer.delay / timer.startDelay;
        }
    }
}