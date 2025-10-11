using Code.Scripts.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Scripts.UI
{
    public class PlaySoundOnHover: MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private AudioClip _sfx;
        [SerializeField] private float _volumeOverride = 1f;

        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.Instance.PlaySFX(_sfx, _volumeOverride);
        }
    }
}
