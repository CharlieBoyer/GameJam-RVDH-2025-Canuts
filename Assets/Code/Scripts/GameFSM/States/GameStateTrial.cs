using System;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Audio;
using Code.Scripts.MonoBehaviours;
using Code.Scripts.SO.Gameplay;
using Code.Scripts.Systems;
using Code.Scripts.UI;
using Code.Scripts.Utils;
using UnityEngine;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Code.Scripts.GameFSM.States
{
    public class GameStateTrial: GameBaseState
    {
        /*  TODO:
         *      'GameStateTrial' will inherit most of the GameManager responsibilities, focusing the gameplay.
         *      This includes handling player actions/choices, manage 'PlayerChoiceSO' data and UI links.
         *      The state should also manage transitions to 'GameStateTrialResolution' based on the trial's outcome.
         */

        public static Action<PlayerChoiceSO> OnPlayerAction;

        private TrialShowdownPanel _trialShowdown;
        private ActionWheel _actionWheel;
        private GameTimer _gameTimer;

        private List<PlayerChoiceSO> _choicePool;

        private bool _doOneTime = true;

        // ----- //

        #region States

        private void InitState(GameStateManager context)
        {
            _manager = context;
            _gameTimer = Object.FindFirstObjectByType<GameTimer>();
            _actionWheel = Object.FindFirstObjectByType<ActionWheel>(FindObjectsInactive.Include);
            _trialShowdown = Object.FindFirstObjectByType<TrialShowdownPanel>(FindObjectsInactive.Include);

            AddressablesUtils.LoadResources<PlayerChoiceSO>("Choices", _manager, assets =>
            {
                _choicePool = assets;
                StartTrial();
            });

            _isInit = true;
        }

        public override void EnterState(GameStateManager context)
        {
            if (!_isInit)
                InitState(context);
            else
                StartTrial();

            Judge.InitializeJudges();
            _actionWheel.Activate(false);

            OnPlayerAction += PrepareNextAction;
            _gameTimer.OnGameTimerEnd += EndTrial;
        }

        public override void UpdateState()
        {
            // _gameTimer.Tick() is done internally
        }

        public override void ExitState()
        {
            _doOneTime = false;

            OnPlayerAction -= PrepareNextAction;
            _gameTimer.OnGameTimerEnd -= EndTrial;
        }

        #endregion

        // ----- //

        #region Gameplay Sequence

        private void StartTrial()
        {
            _manager.StartCoroutine(StartTrialSequence());
        }

        private void PrepareNextAction(PlayerChoiceSO action)
        {
            _manager.StartCoroutine(PrepareNextActionCoroutine(action));
        }

        private void EndTrial()
        {
            _manager.StopAllCoroutines();
            _actionWheel.Cleanup();

            _manager.SwitchState(_manager.States.TrialResolution);
        }

        // ----- //

        private IEnumerator StartTrialSequence()
        {
            yield return BeginPreTrialSequence();

            if (_doOneTime) {
                AudioManager.Instance.PlayMainMusic();
            }
            else {
                // TODO: Use mixer system to switch off EQ/Lowpass effects
            }

            _gameTimer.Begin(_manager.RoundDuration);
            RefreshPlayerChoices();
            _actionWheel.Activate(true);
        }

        private IEnumerator BeginPreTrialSequence()
        {
            AudioManager.Instance.PlaySFX(AudioManager.ClipsIndex.PleadingStart);
            _trialShowdown.ShowBegin(true);

            yield return new WaitForSeconds(_manager.TrialShowdownDuration);

            _trialShowdown.ShowBegin(false);
            PostProcessController.Instance.ToggleDOF(false);
        }

        private IEnumerator PrepareNextActionCoroutine(PlayerChoiceSO action)
        {
            yield return Judge.ResolveAction(action);

            _actionWheel.Cleanup();
            yield return new WaitForSeconds(_manager.PlayerChoiceCooldown);

            RefreshPlayerChoices();
        }

        #endregion

        // ----- //

        private void RefreshPlayerChoices()
        {
            List<PlayerChoiceSO> actions = PullPlayerChoiceSO();
            _actionWheel.Refresh(actions);
        }

        private List<PlayerChoiceSO> PullPlayerChoiceSO()
        {
            List<PlayerChoiceSO> so = new(_manager.PlayerMaxChoices);

            while (so.Count < _manager.PlayerMaxChoices)
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
