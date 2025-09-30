using System;
using UnityEngine;

namespace Code.Scripts.SO
{
    [CreateAssetMenu(fileName = "New PlayerChoice", menuName = "SO/Dialogue", order = 0)]
    [Serializable]
    public class DialogueSO : ScriptableObject
    {
        public string characterName;

        [TextArea(3,10)]
        public string[] dialogueSentences;
        public POSITION pos;
        public Sprite characterSprite;
    }

    [System.Serializable]
    public enum POSITION
    {
        LEFT, RIGHT, MIDDLE
    }
}
