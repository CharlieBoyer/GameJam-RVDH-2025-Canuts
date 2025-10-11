using System;
using UnityEngine;

namespace Code.Scripts.Audio
{
    [Serializable]
    public class AudioClipsIndex
    {
        // TODO: All audio clips of the project
        [Header("OusamaQuote")]
        [field:SerializeField] public AudioClip Attack { get; private set; }
        [field:SerializeField] public AudioClip Diplomacy { get; private set; }
        [field:SerializeField] public AudioClip Dishonor { get; private set; }
        [field: SerializeField] public AudioClip Injustice { get; private set; }
        [field: SerializeField] public AudioClip Threat { get; private set; }

        [Header("Ambiance")]
        [field:SerializeField] public AudioClip MenuMusic { get; private set; }
        [field:SerializeField] public AudioClip GameMusic { get; private set; }

        [Header("Victory/Defeat")]
        [field: SerializeField] public AudioClip GameOver { get; private set; }
        [field: SerializeField] public AudioClip GameWon { get; private set; }

        [Header("Pleading")]
        [field: SerializeField] public AudioClip PleadingEnd { get; private set; }
        [field: SerializeField] public AudioClip PleadingTimer { get; private set; }
        [field: SerializeField] public AudioClip PleadingStart { get; private set; }

        [Header("Dialogue")]
        [field: SerializeField] public AudioClip PassText { get; private set; }
        [field: SerializeField] public AudioClip OusamaVoice { get; private set; }
        [field: SerializeField] public AudioClip RenierVoice { get; private set; }

        [Header("UI")]
        [field: SerializeField] public AudioClip UIHover { get; private set; }
        [field: SerializeField] public AudioClip UIReturn { get; private set; }
        [field: SerializeField] public AudioClip UIValidation { get; private set; }



    }
}
