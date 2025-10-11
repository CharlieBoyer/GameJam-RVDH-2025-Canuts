using Code.Scripts.Types.Dialogues;
using UnityEngine;

namespace Code.Scripts.SO.Dialogues
{
    [CreateAssetMenu(fileName = "New Character", menuName = "SO/Character")]
    public class CharacterSO: ScriptableObject
    {
        [Header("Essentials")]
        [SerializeField] private string _name;
        [SerializeField] private Sprite _mainSprite;

        [Space]
        [Header("Expressions")]
        [SerializeField] private Sprite _suspicious;
        [SerializeField] private Sprite _angry;
        [SerializeField] private Sprite _objection;
        [SerializeField] private Sprite _whiner;
        [SerializeField] private Sprite _pleased;

        public string Name => _name;
        public Sprite Sprite => _mainSprite;

        public Sprite Neutral       => _mainSprite;
        public Sprite Suspicious    => (_suspicious)? _suspicious   : Neutral;
        public Sprite Angry         => (_angry)     ? _angry        : Neutral;
        public Sprite Objection     => (_objection) ? _objection    : Neutral;
        public Sprite Whiner        => (_whiner)    ? _whiner       : Neutral;
        public Sprite Pleased       => (_pleased)   ? _pleased      : Neutral;

        // ----- //

        /// <summary>
        /// Converts an <c>Emotion</c> enum to the corresponding <c>Sprite</c>.
        /// </summary>
        /// <param name="emotion"></param>
        /// <returns></returns>
        public Sprite GetSprite(Emotion emotion)
        {
            return emotion switch
            {
                Emotion.Neutral     => Neutral,
                Emotion.Suspicious  => Suspicious,
                Emotion.Angry       => Angry,
                Emotion.Objection   => Objection,
                Emotion.Whiner      => Whiner,
                Emotion.Pleased     => Pleased,

                _ => Neutral
            };
        }
    }
}
