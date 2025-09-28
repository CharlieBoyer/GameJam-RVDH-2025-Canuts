using System;
using UnityEngine;

namespace Code.Scripts.Types
{
    [Serializable]
    public class Sensibilities
    {
        [SerializeField] [Range(-2f, 2f)] private float _injustice;
        [SerializeField] [Range(-2f, 2f)] private float _attack;
        [SerializeField] [Range(-2f, 2f)] private float _dishonor;
        [SerializeField] [Range(-2f, 2f)] private float _threat;
        [SerializeField] [Range(-2f, 2f)] private float _diplomacy;

        public float Injustice => _injustice;
        public float Attack => _attack;
        public float Dishonor => _dishonor;
        public float Threat => _threat;
        public float Diplomacy => _diplomacy;

        public float AffinityMultiplier { get; set; } = 1;
    }
}
