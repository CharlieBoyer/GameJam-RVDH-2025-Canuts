using System.Collections;
using Code.Scripts.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace Code.Scripts.Audio
{
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        [Space]
        [SerializeField] private AudioMixer _mixerSystem;

        [Space]
        [SerializeField] private AudioClipsIndex _clipsIndex;

        [Header("Initial Mix")]
        [SerializeField] [Range(0, 1)] private float _initialMasterVolume = 0.5f;
        [SerializeField] [Range(0, 1)] private float _initialAmbientVolume = 1f;
        [SerializeField] [Range(0, 1)] private float _initialSFXVolume = 1f;

        [Header("Mixer Groups")]
        [field:SerializeField] public AudioMixerGroup MasterMixerModule;
        [field:SerializeField] public AudioMixerGroup AmbientMixerModule;
        [field:SerializeField] public AudioMixerGroup SFXMixerModule;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private AudioSource _sfxAudioSource;

        [Header("Interpolation parameters")]
        [SerializeField] private float _defaultClipTransitionSpeed = 1f;

        private static readonly string MasterVolumeParameter = "VolumeMaster";
        private static readonly string AmbientVolumeParameter = "VolumeAmbient";
        private static readonly string SFXVolumeParameter = "VolumeSFX";

        #region Properties

        public static AudioClipsIndex ClipsIndex => Instance._clipsIndex;

        public static float DefaultClipTransitionSpeed => Instance._defaultClipTransitionSpeed;

        private float _masterVolume;
        public float MasterVolume {
            get => _masterVolume;

            set {
                _masterVolume = value;
                UpdateVolume(MasterVolumeParameter, value);
            }
        }

        private float _ambientVolume;
        public float AmbientVolume {
            get => _ambientVolume;

            set {
                _ambientVolume = value;
                UpdateVolume(AmbientVolumeParameter, value);
            }
        }

        private float _sfxVolume;
        public float SFXVolume {
            get => _sfxVolume;

            set {
                _sfxVolume = value;
                UpdateVolume(SFXVolumeParameter, value);
            }
        }

        private bool _masterVolumeMuted;
        private bool _ambientVolumeMuted;
        private bool _sfxVolumeMuted;
        public bool MasterVolumeMuted {
            get => _masterVolumeMuted;
            set {
                if (value)
                    UpdateVolume(MasterVolumeParameter, 0);
                else
                    UpdateVolume(MasterVolumeParameter, _initialMasterVolume);
                _masterVolumeMuted = value;
            }
        }
        public bool AmbientVolumeMuted {
            get => _ambientVolumeMuted;
            set {
                if (value)
                    UpdateVolume(AmbientVolumeParameter, 0);
                else
                    UpdateVolume(AmbientVolumeParameter, _initialAmbientVolume);
                _ambientVolumeMuted = value;
            }
        }
        public bool SFXVolumeMuted {
            get => _sfxVolumeMuted;
            set {
                if (value)
                    UpdateVolume(SFXVolumeParameter, 0);
                else
                    UpdateVolume(SFXVolumeParameter, _initialSFXVolume);
                _sfxVolumeMuted = value;
            }
        }

        #endregion

        // ----- //

        private void Awake()
        {
            // DontDestroyOnLoad(this.gameObject);
            MasterVolume = _initialMasterVolume;
            AmbientVolume = _initialAmbientVolume;
            SFXVolume = _initialSFXVolume;
        }

        // ----- //

        /// <summary>
        /// Update the appropriate volume referenced as an exposed mixer parameter
        /// </summary>
        /// <remarks>🛈 The function is called upon setting the Volume properties and shouldn't be used as-is</remarks>
        /// <param name="mixerParameter">Name of the exposed mixer parameter to modify.</param>
        /// <param name="value">Normalized volume (automatically clamped between [0,1])</param>
        private void UpdateVolume(string mixerParameter, float value)
        {
          /*  float decibels = -80 * (1 - Mathf.Clamp(value, 0, 1));

            _mixerSystem.SetFloat(mixerParameter, decibels);*/

            // Clamp l'entrée entre un seuil minimum très faible (mais non nul) et 1
            float clampedValue = Mathf.Clamp(value, 0.0001f, 1f);
            // Converti en décibels : 0 dB = volume max, -80 dB = quasi silence
            float decibels = Mathf.Log10(clampedValue) * 20f;

            _mixerSystem.SetFloat(mixerParameter, decibels);

        }

        /// <summary>
        /// Update manually the appropriate volume referenced as an exposed mixer parameter
        /// </summary>
        /// <remarks>⚠ Use with caution as volume value can exceed 1</remarks>
        /// <param name="mixerParameter">Name of the exposed mixer parameter to modify</param>
        /// <param name="value">Unclamped normalized volume</param>
        public void UpdateVolumeUnclamped(string mixerParameter, float value)
        {
            float decibels = -40 * (1 - value);

            _mixerSystem.SetFloat(mixerParameter, decibels);
        }

        // ----- //

        /// <summary>
        /// Change the audio to the main clip and play it
        /// </summary>
        public void PlayMainMusic()
        {
            _musicAudioSource.clip = _clipsIndex.GameMusic;
            _musicAudioSource.Play();
        }

        public void ChangeMusicSmoothed(AudioClip clip, float volumeOverride = -1f, float smoothSpeed = 1f)
        {
            IEnumerator ChangeMusicCoroutine(AudioClip targetClip, float targetVolume, float transitionDuration)
            {
                float endValue = Mathf.Clamp((targetVolume < 0 ? _musicAudioSource.volume : targetVolume), 0, 1);

                DOTween.To(
                    () => _musicAudioSource.volume,
                    x => _musicAudioSource.volume = x,
                    0, transitionDuration);

                yield return new WaitForSeconds(transitionDuration);
                _musicAudioSource.Stop();
                _musicAudioSource.clip = targetClip;
                _musicAudioSource.Play();

                DOTween.To(
                    () => _musicAudioSource.volume,
                    x => _musicAudioSource.volume = x,
                    endValue, transitionDuration);
            }

            StartCoroutine(ChangeMusicCoroutine(clip, volumeOverride, smoothSpeed));
        }

        public void StopMusicSmoothed(float smoothSpeed = 1f)
        {
            IEnumerator StopMusicCoroutine(float transitionDuration)
            {
                DOTween.To(
                    () => _musicAudioSource.volume,
                    x => _musicAudioSource.volume = x,
                    0, transitionDuration);

                yield return new WaitForSeconds(transitionDuration);
                _musicAudioSource.Stop();
            }

            StartCoroutine(StopMusicCoroutine(smoothSpeed));
        }

        public void PlaySFX(AudioClip sfx, float volumeOverride = -1f)
        {
            float targetVolume = Mathf.Clamp((volumeOverride < 0 ? _musicAudioSource.volume : volumeOverride), 0, 1);

            _sfxAudioSource.PlayOneShot(sfx, targetVolume);
        }

        // ----- //
    }
}
