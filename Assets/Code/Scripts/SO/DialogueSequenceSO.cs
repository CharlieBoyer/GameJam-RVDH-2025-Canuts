using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.SO
{
    [CreateAssetMenu(fileName = "New Sequence", menuName = "SO/Sequence", order = 2)]
    public class DialogueSequenceSO: ScriptableObject
    {
        [Header("List of dialogues to chain in the sequence :")]
        [SerializeField] private List<DialogueSO> _dialogues;

        public List<DialogueSO> Dialogues => _dialogues;
    }
}
