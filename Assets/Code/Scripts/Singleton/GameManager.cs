using System;
using System.Collections.Generic;
using Code.Scripts.Entities;
using Code.Scripts.SO;
using Code.Scripts.Types;
using Code.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts.Singleton
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        [Header("Game settings")]
        [SerializeField] [Range(1, 5)] private int _maxPlayerChoices = 3;
        [SerializeField] private int _judgesMaxConviction;

        [Header("References")]
        [SerializeField] private GameObject _choicePrefab;
        [SerializeField] private List<PlayerChoiceSO> _choicePool = new ();
        [SerializeField] private RectTransform _playerChoicePanel;

        public int JudgesMaxConviction => _judgesMaxConviction;

        // Game Data
        private GameMode _currentGameMode = GameMode.Trial;

        // Events
        public Action<PlayerChoiceSO> OnPlayerAction;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Judge.InitializeJudges();
        }

        private void Start()
        {
            RefreshPlayerChoices();
        }

        private void Update()
        {
        }

        // ----- //

        private void RefreshPlayerChoices()
        {
            List<PlayerChoiceSO> so = PullPlayerChoiceSO();

            for (int index = 0; index < _maxPlayerChoices; index++)
            {
                Choice playerChoice = Instantiate(_choicePrefab, _playerChoicePanel).GetComponent<Choice>();
                PlayerChoiceSO choiceData = _choicePool[index];
                playerChoice.Setup(choiceData);
            }
        }

        private List<PlayerChoiceSO> PullPlayerChoiceSO()
        {
            List<PlayerChoiceSO> so = new(_maxPlayerChoices);

            while (so.Count < _maxPlayerChoices)
            {
                PlayerChoiceSO soData = _choicePool[Random.Range(0, _choicePool.Count)];

                if (so.Contains(soData))
                    continue;

                so.Add(soData);
            }

            return so;
        }
    }
}
