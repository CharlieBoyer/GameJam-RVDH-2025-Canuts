using System.Collections;
using Code.Scripts.Audio;
using Code.Scripts.MonoBehaviours;
using Code.Scripts.UI;
using UnityEngine;

namespace Code.Scripts.GameFSM.States
{
    public class GameStateTrialResolution: GameBaseState
    {
        /*  TODO:
         *      'GameStateTrialResolution' will handle the aftermath of a trial.
         *      This mostly includes feedback animations after any calculation leading to the game sequence result.
         *      This state might also update GameStateManager context base on the outcome for better game flow tracking.
         */

        private TrialShowdownPanel _trialShowdown;

        // ----- //

        private void InitState(GameStateManager context)
        {
            _manager = context;
            _trialShowdown = Object.FindFirstObjectByType<TrialShowdownPanel>(FindObjectsInactive.Include);

            _isInit = true;
        }

        public override void EnterState(GameStateManager context)
        {
            if (!_isInit)
                InitState(context);

            EntryAnimations();
        }

        public override void UpdateState() {}

        public override void ExitState() {}

        // ----- //

        private bool IsJudgesConvinced()
        {
            int totalJudges = Judge.Instances.Length;
            int convincedJudge = 0;

            foreach (Judge judge in Judge.Instances)
            {
                if (judge.Conviction > _manager.JudgesMaxConviction)
                    convincedJudge++;
            }

            return convincedJudge >= Mathf.CeilToInt(totalJudges / 2f);
        }

        // ----- //

        private void EntryAnimations()
        {
            IEnumerator AnimationCoroutine()
            {
                PostProcessController.Instance.ToggleDOF(true);
                yield return new WaitForSeconds(_manager.GameStateTransitionDelay);
                _trialShowdown.ShowEnd(true);
                AudioManager.Instance.PlaySFX(AudioManager.ClipsIndex.PleadingEnd);
                yield return new WaitForSeconds(_manager.TrialShowdownDuration);
                _trialShowdown.ShowEnd(false);
                PostProcessController.Instance.ToggleDOF(false);

                yield return new WaitForSeconds(_manager.GameStateTransitionDelay);

                // TODO: Doesn't compute conviction scores yet and shouldn't start Dialogues right away:
                _manager.SwitchState(_manager.States.Dialogue);
            }

            _manager.StartCoroutine(AnimationCoroutine());
        }
    }
}
