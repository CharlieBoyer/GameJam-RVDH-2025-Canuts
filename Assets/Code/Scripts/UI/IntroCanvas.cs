using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.UI
{
    public class IntroCanvas: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup _screenCanvasGroup;
        [SerializeField] private CanvasGroup _textCanvasGroup;
        [SerializeField] private TMP_Text _textBox;
        [SerializeField] private Button _clickArea;

        [Header("Animations")]
        [SerializeField] private float _textFadeDuration = 1f;
        [SerializeField] private float _screenFadeOutDuration = 2f;

        public Action OnIntroCanvasClick;

        private void OnEnable()
        {
            _clickArea.onClick.AddListener(() =>
            {
                OnIntroCanvasClick?.Invoke();
                StartCoroutine(PostClickDelay());
            });
        }

        private void OnDisable()
        {
            _clickArea.onClick.RemoveAllListeners();
        }

        // ----- //

        private IEnumerator PostClickDelay()
        {
            _clickArea.interactable = false;
            yield return new WaitForSeconds(_textFadeDuration * 2);
            _clickArea.interactable = true;
        }

        // ----- //

        public void ShowInTextBox(string text)
        {
            _textCanvasGroup.DOFade(0, _textFadeDuration).OnComplete(() =>
            {
                _textBox.text = text;
                _textCanvasGroup.DOFade(1, _textFadeDuration);
            });
        }

        public void FadeOutBlackScreen()
        {
            IEnumerator AnimationRoutine()
            {
                _textCanvasGroup.DOFade(0, _textFadeDuration);
                yield return new WaitForSeconds(_textFadeDuration);
                _screenCanvasGroup.DOFade(0, _screenFadeOutDuration).OnComplete(() =>
                {
                    this.gameObject.SetActive(false);
                });

            }

            StartCoroutine(AnimationRoutine());
        }
    }
}
