using Code.Scripts.Utils;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

namespace Code.Scripts.UI
{
    public class PostProcessController: SingletonMonoBehaviour<PostProcessController>
    {
        [SerializeField] private Volume _postProcess;
        [SerializeField] private float _blurStart = 0.1f;
        [SerializeField] private float _defaultDistance = 10f;
        [SerializeField] private float _smoothDuration = 1f;

        private DepthOfField _dofModule;

        private void Start()
        {
            if (_postProcess.profile.TryGet(out DepthOfField dof))
            {
                _dofModule = dof;
            }
        }

        public void ToggleDOF(bool enable)
        {
            if (enable)
            {
                DOTween.To(
                    () => _dofModule.focusDistance.value,
                    x => _dofModule.focusDistance.value = x,
                    _blurStart,
                    _smoothDuration
                ).SetEase(Ease.InOutSine);
            }
            else
            {
                DOTween.To(
                    () => _dofModule.focusDistance.value,
                    x => _dofModule.focusDistance.value = x,
                    _defaultDistance,
                    _smoothDuration
                ).SetEase(Ease.InOutSine);
            }
        }
    }
}
