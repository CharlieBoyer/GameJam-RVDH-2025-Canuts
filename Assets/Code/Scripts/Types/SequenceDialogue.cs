using System.Collections.Generic;
using Code.Scripts.SO;
using UnityEngine;

namespace Code.Scripts.Types
{
    [System.Serializable]
    public class SequenceDialogue
    {
        [SerializeField]
        private List<DialogueSO> dialogues;

        public List<DialogueSO> Dialogues => dialogues;
    }
}
