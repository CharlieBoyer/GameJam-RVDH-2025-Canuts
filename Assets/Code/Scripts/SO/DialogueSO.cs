using System;
using Code.Scripts.Types;
using UnityEngine;

namespace Code.Scripts.SO
{
    [CreateAssetMenu(fileName = "New PlayerChoice", menuName = "SO/Dialogue", order = 1)]
    [Serializable]
    public class DialogueSO : ScriptableObject
    {
        public string CharacterName;

        [TextArea(3,10)] public string[] DialogueSentences;
        public Position Pos;
        public Sprite CharacterSprite;
    }
}
