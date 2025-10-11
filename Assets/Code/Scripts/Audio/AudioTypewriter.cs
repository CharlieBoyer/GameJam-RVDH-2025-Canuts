using System;
using UnityEngine;

namespace Code.Scripts.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioTypewriter: MonoBehaviour
    {
        [SerializeField] private AudioClip _sfx;
        [SerializeField] [Range(0,1)] private float _volumeOverride = 1f;
        private AudioSource _source;

        public static Action OnTypewriterSound;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            OnTypewriterSound += PlayTypewriterSound;
        }

        private void OnDisable()
        {
            OnTypewriterSound -= PlayTypewriterSound;
        }

        public void PlayTypewriterSound()
        {
            _source.PlayOneShot(_sfx, _volumeOverride);
        }
    }
}
