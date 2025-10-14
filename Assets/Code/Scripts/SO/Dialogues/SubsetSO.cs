using System;
using Code.Scripts.Types.Dialogues;
using UnityEngine;

namespace Code.Scripts.SO.Dialogues
{
    [Serializable]
    public class SubsetSO
    {
        [SerializeField] private CharacterRef.Index _talker;
        [SerializeField] private Emotion _emotion;
        [TextArea(3, 10)] [SerializeField] private string _text;

        public CharacterRef.Index TalkerID => _talker;
        public string Talker  => CharacterRef.GetCharacter(_talker).Name;
        public Sprite Sprite => CharacterRef.GetCharacter(_talker).GetSprite(_emotion);
        public string Text => _text;
    }
}
