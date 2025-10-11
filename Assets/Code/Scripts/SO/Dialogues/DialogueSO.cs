using System.Collections.Generic;
using Code.Scripts.Types.Dialogues;
using UnityEngine;

namespace Code.Scripts.SO.Dialogues
{
    [CreateAssetMenu(fileName = "dialogue test", menuName = "SO (Test)/Dialogue_v2", order = 0)]
    public class DialogueSO: ScriptableObject
    {
        [Header("Character presence (Scene)")]
        [SerializeField] private CharacterRef.Index _left;
        [SerializeField] private CharacterRef.Index _center;
        [SerializeField] private CharacterRef.Index _right;

        [Header("Dialogue Subsets")]
        [SerializeField] private List<SubsetSO> _subsets;

        public CharacterRef.Index LeftID => _left;
        public CharacterRef.Index CenterID => _center;
        public CharacterRef.Index RightID => _right;

        public CharacterSO Left  => CharacterRef.GetCharacter(_left);
        public CharacterSO Center => CharacterRef.GetCharacter(_center);
        public CharacterSO Right => CharacterRef.GetCharacter(_right);

        public List<SubsetSO> Subsets => _subsets;

        // ----- //
    }
}
