using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.UI
{
    public class GameTimer: MonoBehaviour
    {
        [SerializeField] private Slider _sliderL;
        [SerializeField] private Slider _sliderR;

        public void Setup(float maxDuration, bool startFull = true)
        {
            _sliderL.maxValue = maxDuration;
            _sliderR.maxValue = maxDuration;
            _sliderL.wholeNumbers = false;
            _sliderR.wholeNumbers = false;

            if (startFull)
        }
    }
}
