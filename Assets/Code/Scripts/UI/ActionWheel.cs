using System.Collections.Generic;
using Code.Scripts.GameFSM;
using Code.Scripts.MonoBehaviours;
using Code.Scripts.SO.Gameplay;
using UnityEngine;

namespace Code.Scripts.UI
{
    public class ActionWheel: MonoBehaviour
    {
        [SerializeField] private RectTransform _playerChoicePanel;
        [SerializeField] private GameObject _choicePrefab;

        private int ActionWheelCapacity => GameStateManager.Instance.PlayerMaxChoices;

        public void Activate(bool enable)
        {
            gameObject.SetActive(enable);
        }

        public void Refresh(List<PlayerChoiceSO> choices)
        {
            for (int index = 0; index < ActionWheelCapacity; index++)
            {
                Choice playerChoice = Instantiate(_choicePrefab, _playerChoicePanel).GetComponent<Choice>();
                PlayerChoiceSO choiceData = choices[index];
                playerChoice.Setup(choiceData);
            }
        }

        public void Cleanup()
        {
            foreach (Transform child  in _playerChoicePanel.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
