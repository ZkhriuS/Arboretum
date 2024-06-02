using TMPro;
using UnityEngine;

namespace Tools.Timer
{
    public class TimerTMPDisplay : TimerDisplay
    {
        public TextMeshProUGUI label;

        private void Update()
        {
            label.text = $"{timer.delay:##.00}";
        }
    }
}