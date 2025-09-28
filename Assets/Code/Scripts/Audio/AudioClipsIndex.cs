using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Audio
{
    [Serializable]
    public class AudioClipsIndex
    {
        // TODO: All audio clips of the project
        [Header("UI")]
        [field:SerializeField] public AudioClip UIButtonSelected { get; private set; }
        [field:SerializeField] public AudioClip UIButtonHoverEnter { get; private set; }
        [field:SerializeField] public AudioClip UIButtonHoverExit { get; private set; }

        [Header("Ambiance")]
        [field:SerializeField] public AudioClip MenuMusic { get; private set; }
        [field:SerializeField] public AudioClip GameMusic { get; private set; }

    }
}
