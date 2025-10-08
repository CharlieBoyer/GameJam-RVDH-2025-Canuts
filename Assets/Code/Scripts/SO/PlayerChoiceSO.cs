using Code.Scripts.Types;
using UnityEngine;

namespace Code.Scripts.SO
{
    [CreateAssetMenu(fileName = "New PlayerChoice", menuName = "SO/Player Choice", order = 3)]
    public class PlayerChoiceSO: ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private int _convictionValue;
        [SerializeField] private PlayerChoiceCategory _category;
        [SerializeField] private JudgeRef _targets;

        public string Name => _name;
        public int ConvictionValue => _convictionValue;
        public PlayerChoiceCategory Category => _category;
        public JudgeRef Targets => _targets;
    }
}
