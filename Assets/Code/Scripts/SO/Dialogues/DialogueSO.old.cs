using System;
using Code.Scripts.Types;
using UnityEngine;

namespace Code.Scripts.SO.Dialogues
{
    [CreateAssetMenu(fileName = "New PlayerChoice", menuName = "SO/Dialogue", order = 1)]
    [Serializable]
    public class DialogueSO_Old : ScriptableObject
    {
        public string CharacterName;

        [TextArea(3,10)] public string[] DialogueSentences;
        public Position Pos;
        public Sprite CharacterSprite;
    }
}
