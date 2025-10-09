using System;
using Code.Scripts.Singleton;
using Code.Scripts.SO.Gameplay;
using Code.Scripts.Types.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Entities
{
    public class Choice: MonoBehaviour
    {
        private Button _button;
        private TMP_Text _label;
        private PlayerChoiceSO _actionSO;

        private void Awake()
        {
            _button = GetComponentInChildren<Button>();
            _label = GetComponentInChildren<TMP_Text>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnChoiceClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        // ----- ///

        public void Setup(PlayerChoiceSO data)
        {
            ColorBlock colors = _button.colors;

            _actionSO = data;

            switch (_actionSO.Category)
            {
                case PlayerChoiceCategory.Injustice:
                    colors.normalColor = new Color32(11, 83, 148, 255);
                    break;
                case PlayerChoiceCategory.Attack:
                    colors.normalColor = new Color32(103, 78, 167, 255);
                    break;
                case PlayerChoiceCategory.Dishonor:
                    colors.normalColor = new Color32(106, 168, 79, 255);
                    break;
                case PlayerChoiceCategory.Threat:
                    colors.normalColor = new Color32(204, 0, 0, 255);
                    break;
                case PlayerChoiceCategory.Diplomacy:
                    colors.normalColor = new Color32(191, 144, 0, 255);
                    break;

                default:
                    throw new ArgumentException("Error: Invalid PlayerChoiceSO category, please check SO: \"{action.name}\".");
            }

            _label.text = _actionSO.Name;
            _button.colors = colors;
        }

        private void OnChoiceClick()
        {
            // TODO: On button choice click
            // GameManager.Instance?.OnPlayerAction.Invoke(_actionSO);
        }
    }
}
