using System;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Entities;
using Code.Scripts.SO;
using Code.Scripts.Types;
using Code.Scripts.UI;
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
        [SerializeField] private int _judgesConvincedThreshold;
        [SerializeField] private float _delayBetweenActions = 3f;

        [Header("Trial References")]
        [SerializeField] private GameTimer _gameTimer;
        [SerializeField] private GameObject _choicePrefab;
        [SerializeField] private List<PlayerChoiceSO> _choicePool = new ();
        [SerializeField] private RectTransform _playerChoicePanel;

        public int JudgesMaxConviction => _judgesMaxConviction;

        // Game Mode
        private GameMode _currentGameMode = GameMode.Trial;

        // Events
        public Action<PlayerChoiceSO> OnPlayerAction;

        // ----- //

        private void Awake()
        {
            Judge.InitializeJudges();
        }

        private void OnEnable()
        {
            OnPlayerAction += PrepareNextAction;
            _gameTimer.OnGameTimerEnd += EndGameSequence;
        }

        private void OnDisable()
        {
            OnPlayerAction -= PrepareNextAction;
        }

        private void Update()
        {
            _gameTimer.Tick();
        }

        // ----- //

        private void Start()
        {
            // Show Start text

            AudioManager.Instance.PlayMusic();

            _gameTimer.Begin(_roundDuration);

            RefreshPlayerChoices();
        }

        private void EndGameSequence()
        {
            if (IsJudgesConvinced())
            {
                // Play victory sound
            }
            else
            {
                // Play loss sound
            }

            // TransitionToNarrativeMode()
        }

        // ----- //

        private bool IsJudgesConvinced()
        {
            int totalJudges = Judge.Instances.Length;
            int convincedJudge = 0;

            foreach (Judge judge in Judge.Instances)
            {
                if (judge.Conviction > _judgesConvincedThreshold)
                    convincedJudge++;
            }

            return convincedJudge >= Mathf.CeilToInt(totalJudges / 2f);
        }

        private void PrepareNextAction(PlayerChoiceSO discard)
        {
            StartCoroutine(PrepareNextActionCoroutine());
        }

        private IEnumerator PrepareNextActionCoroutine()
        {
            CleanupActionWheel();

            yield return new WaitForSeconds(_delayBetweenActions);

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
            foreach (Transform children in _playerChoicePanel.transform)
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


    }
}
