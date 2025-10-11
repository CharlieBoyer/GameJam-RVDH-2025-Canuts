using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.UI
{
    public class TrialShowdownPanel: MonoBehaviour
    {
        [SerializeField] private CanvasGroup _root;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _fadeDuration = 1f;

        [SerializeField] private RectTransform _beginPanel;
        [SerializeField] private RectTransform _endPanel;

        private static readonly int Begin = Animator.StringToHash("Begin");
        private static readonly int End = Animator.StringToHash("End");

        private void Start()
        {
            _root.alpha = 0f;
        }

        public void ShowBegin(bool enable)
        {
            if (enable)
            {
                _beginPanel.gameObject.SetActive(true);
                _root.DOFade(1, _fadeDuration).SetEase(Ease.InOutSine).OnComplete(() => PlayAnimation(Begin)).SetUpdate(true);
            }
            else
            {
                _root.DOFade(0, _fadeDuration).SetEase(Ease.InOutSine).OnComplete(() => _beginPanel.gameObject.SetActive(false));
            }
        }

        public void ShowEnd(bool enable)
        {
            if (enable)
            {
                _endPanel.gameObject.SetActive(true);
                _root.DOFade(1, _fadeDuration).SetEase(Ease.InOutSine).OnComplete(() => PlayAnimation(End)).SetUpdate(true);
            }
            else
            {
                _root.DOFade(0, _fadeDuration).SetEase(Ease.InOutSine).OnComplete(() => _endPanel.gameObject.SetActive(false));
            }
        }

        public void PlayAnimation(int state)
        {
            _animator.SetTrigger(state);
        }
    }
}
