using System;
using Code.Scripts.Singleton;
using Code.Scripts.SO;
using Code.Scripts.Types;
using Code.Scripts.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Entities
{
    public class Judge: MonoBehaviour
    {
        // Statics
        public static Judge[] Instances { get; private set; }

        [Header("Identity")]
        [SerializeField] [Tooltip("Be cautious of setting only one flag per judge!")] private JudgeRef _refID;
        [SerializeField] private Sensibilities _sensibilities;

        [Header("UI Module")]
        [SerializeField] private TMP_Text _nameBox;
        [SerializeField] private Slider _convictionGauge;
        [SerializeField] private float _convictionGaugeAnimDuration;

        private int _currentConviction;
        private int Conviction
        {
            get => _currentConviction;

            set
            {
                _currentConviction = Mathf.FloorToInt(value);
                _convictionGauge.DOValue(_currentConviction, _convictionGaugeAnimDuration);
            }
        }

        private void OnEnable()
        {
            GameManager.Instance.OnPlayerAction += ResolveDefenseStatement;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnPlayerAction -= ResolveDefenseStatement;
        }

        // ----- //

        public static void InitializeJudges()
        {
            Instances = FindObjectsByType<Judge>(FindObjectsSortMode.InstanceID);

            foreach (Judge judge in Instances)
            {
                judge.Init();
            }
        }

        public void Init()
        {
            _nameBox.text = _refID.ToString().AddSpacesToEnum();

            _convictionGauge.wholeNumbers = true;
            _convictionGauge.maxValue = GameManager.Instance.JudgesMaxConviction;
            _convictionGauge.value = 0;
        }

        public void ResolveDefenseStatement(PlayerChoiceSO action)
        {
            int baseValue = action.ConvictionValue;
            int totalConvictionPoints;

            if (!action.Targets.HasFlag(_refID))
                return;

            switch (action.Category)
            {
                case PlayerChoiceCategory.Injustice:
                    totalConvictionPoints = Mathf.RoundToInt(baseValue * _sensibilities.Injustice * _sensibilities.AffinityMultiplier);
                    break;
                case PlayerChoiceCategory.Attack:
                    totalConvictionPoints = Mathf.RoundToInt(baseValue * _sensibilities.Injustice * _sensibilities.AffinityMultiplier);
                    break;
                case PlayerChoiceCategory.Dishonor:
                    totalConvictionPoints = Mathf.RoundToInt(baseValue * _sensibilities.Injustice * _sensibilities.AffinityMultiplier);
                    break;
                case PlayerChoiceCategory.Threat:
                    totalConvictionPoints = Mathf.RoundToInt(baseValue * _sensibilities.Injustice * _sensibilities.AffinityMultiplier);
                    break;
                case PlayerChoiceCategory.Diplomacy:
                    totalConvictionPoints = Mathf.RoundToInt(baseValue * _sensibilities.Injustice * _sensibilities.AffinityMultiplier);
                    break;

                default:
                    throw new ArgumentException($"Error: Invalid PlayerChoiceSO category, please check SO: \"{action.name}\".");
            }

            Conviction += totalConvictionPoints;
        }
    }
}
