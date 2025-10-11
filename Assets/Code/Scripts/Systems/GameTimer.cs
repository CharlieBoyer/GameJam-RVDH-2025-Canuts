using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Systems
{
    public class GameTimer: MonoBehaviour
    {
        [SerializeField] private Slider _sliderL;
        [SerializeField] private Slider _sliderR;

        private float _timer = 0f;

        private bool _isRunning = false;

        public Action OnGameTimerEnd;

        // ----- //

        private void Update()
        {
            if (_isRunning)
                Tick();
        }

        // ----- //

        public void Begin(float maxDuration, bool startFull = true)
        {
            _sliderL.maxValue = maxDuration;
            _sliderR.maxValue = maxDuration;
            _sliderL.wholeNumbers = false;
            _sliderR.wholeNumbers = false;

            if (startFull)
            {
                _sliderL.value = maxDuration;
                _sliderR.value = maxDuration;
            }

            _timer = maxDuration;
            _isRunning = true;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;

            _sliderL.value = _timer;
            _sliderR.value = _timer;

            if (_timer <= 0)
            {
                OnGameTimerEnd?.Invoke();
                _isRunning = false;
            }
        }
    }
}
