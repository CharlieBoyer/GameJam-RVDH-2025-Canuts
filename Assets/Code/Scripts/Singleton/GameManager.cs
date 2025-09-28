using System;
using System.Collections;
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
        [Header("Trial settings")]
        [SerializeField] private float _roundDuration = 30f;
        [SerializeField] [Range(1, 5)] private int _maxPlayerChoices = 3;
        [SerializeField] private int _judgesMaxConviction;
        [SerializeField] private float _delayBetweenActions = 3f;

        [Header("Trial References")]
        [SerializeField] private GameTimer
        [SerializeField] private GameObject _choicePrefab;
        [SerializeField] private List<PlayerChoiceSO> _choicePool = new ();
        [SerializeField] private RectTransform _playerChoicePanel;

        public int JudgesMaxConviction => _judgesMaxConviction;

        // Game Mode
        private GameMode _currentGameMode = GameMode.Trial;

        // Events
        public Action<PlayerChoiceSO> OnPlayerAction;

        // Trial
        private float _timer = 0f;

        private void Awake()
        {
            Judge.InitializeJudges();
        }

        private void Start()
        {
            // Show Start text

            AudioManager.Instance.PlayMusic();
            RefreshPlayerChoices();
        }

        // ----- //

        private void RefreshPlayerChoices()
        {
            List<PlayerChoiceSO> so = PullPlayerChoiceSO();

            for (int index = 0; index < _maxPlayerChoices; index++)
            {
                Choice playerChoice = Instantiate(_choicePrefab, _playerChoicePanel).GetComponent<Choice>();
                PlayerChoiceSO choiceData = so[index];
                playerChoice.Setup(choiceData);
            }
        }

        private void CleanupActionWheel()
        {
            foreach (Transform children in transform)
            {
                Destroy(children.gameObject);
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

        // ----- //

        private void PrepareNextAction()
        {
            StartCoroutine(PrepareNextActionCoroutine());
        }

        private IEnumerator PrepareNextActionCoroutine()
        {
            CleanupActionWheel();

            yield return new WaitForSeconds(_delayBetweenActions);

            RefreshPlayerChoices();
        }
    }
}
