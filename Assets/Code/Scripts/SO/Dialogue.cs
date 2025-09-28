using UnityEngine;
using Code.Scripts.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;

namespace Code.Scripts.SO
{
    [CreateAssetMenu(fileName = "New PlayerChoice", menuName = "SO/Dialogue", order = 0)]
    [System.Serializable]
    public class Dialogue : ScriptableObject
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
