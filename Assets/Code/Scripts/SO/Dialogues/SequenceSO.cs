using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.SO.Dialogues
{
    [CreateAssetMenu(fileName = "New Sequence", menuName = "SO/Sequence", order = 2)]
    public class SequenceSO: ScriptableObject
    {
        [Header("List of dialogues to chain in the sequence :")]
        [SerializeField] private List<DialogueSO> _parts;

        public List<DialogueSO> Parts => _parts;
    }
}
