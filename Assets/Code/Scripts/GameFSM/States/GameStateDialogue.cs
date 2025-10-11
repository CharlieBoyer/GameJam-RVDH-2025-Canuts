using System;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Audio;
using Code.Scripts.SO.Dialogues;
using Code.Scripts.Types.Dialogues;
using Code.Scripts.UI;
using Code.Scripts.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Scripts.GameFSM.States
{
    public class GameStateDialogue: GameBaseState
    {
        /*  TODO:
         *      'GameStateDialogue' should herit any logic from the 'DialogueManager'.
         *      With it's own instance, this state can easily store dialogue states such as chapters/sequences, etc...
         *      However, this state might need to communicate with a dedicated UI module (and optionally dialogue settings class).
         */

        private IntroCanvas _introCanvas;
        private DialogueCanvas _dialogueCanvas;

        private List<SequenceSO> _sequences;
        private List<DialogueSO> _parts;
        private List<SubsetSO> _subsets;

        private SequenceSO _activeSequence;
        private DialogueSO _activePart;
        private SubsetSO _activeSubset;

        private int _sequenceIndex = 0;
        private int _partIndex = 0;
        private int _subsetIndex = 0;

        private bool _catchUpNextSequences = false;

        // ----- //

        private void InitState(GameStateManager context)
        {
            _manager = context;
            _dialogueCanvas = Object.FindFirstObjectByType<DialogueCanvas>();

            if (!_dialogueCanvas)
                throw new Exception("[GameStateDialogue] No DialogueCanvas script-object found in the scene.");

            CharacterRef.Init(_manager);

            AddressablesUtils.LoadResources<SequenceSO>("Sequences", _manager, (assets) =>
            {
                _sequences = assets;
                _sequences.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));
                StartSequence(_sequenceIndex);
            });

            _isInit = true;
        }

        public override void EnterState(GameStateManager context)
        {
            if (!_isInit)
                InitState(context);
            else
                Debug.Log("[GameStateDialogue] Already initialized (re-entering state)");

            _dialogueCanvas.OnNextDialogueClicked += ManageActivePart;

            _dialogueCanvas.gameObject.SetActive(true);
            PostProcessController.Instance.ToggleDOF(true);
            _dialogueCanvas.FadeDialogueCanvas(1f, true);

            if (_catchUpNextSequences)
                ManageActivePart();
        }

        public override void UpdateState() {}

        public override void ExitState()
        {
            _catchUpNextSequences = true;
            _dialogueCanvas.OnNextDialogueClicked -= ManageActivePart;
        }

        // ----- //

        private void StartSequence(int seqIndex)
        {
            if (!CharacterRef.IsInit)
                throw new Exception("[GameStateDialogue] CharacterRef class seems to not be initialized before starting a sequence...");

            _activeSequence = _sequences[seqIndex];
            _parts = _activeSequence.Parts;

            ManageActivePart();
        }

        private void ManageActivePart()
        {
            if (_catchUpNextSequences)
            {
                // TODO: Use mixer system to switch on EQ/Lowpass effects while in [GameStateDialogue]
            }

            if (_partIndex >= _parts.Count)
            {
                EndSequence();
                return;
            }

            _activePart = _activeSequence.Parts[_partIndex];
            _activeSubset = _activePart.Subsets[_subsetIndex];

            _dialogueCanvas.Talk(_activeSubset.Talker, _activeSubset.Text);
            _dialogueCanvas.UpdateActiveTalker(GetPositionIndex(_activePart, _activeSubset), _activeSubset.Sprite);

            _subsetIndex++;

            if (_subsetIndex >= _activePart.Subsets.Count)
            {
                _subsetIndex = 0;
                _partIndex++;
            }
        }

        private void EndSequence()
        {


            _partIndex = 0;
            _subsetIndex = 0;
            _sequenceIndex++;
            _activeSequence = _sequences[_sequenceIndex];
            _parts = _activeSequence.Parts;

            if (_sequenceIndex >= _sequences.Count - 1)
            {
                AudioManager.Instance.PlaySFX(AudioManager.ClipsIndex.GameWon);
                _introCanvas.ReturnToMenu();
            }

            _manager.StartCoroutine(EndSequenceCoroutine());
        }

        private IEnumerator EndSequenceCoroutine()
        {
            _dialogueCanvas.FadeDialogueCanvas(0f, true);
            _dialogueCanvas.gameObject.SetActive(false);
            yield return new WaitForSeconds(_manager.GameStateTransitionDelay);
            _manager.SwitchState(_manager.States.Trial);
        }

        // ----- //

        private int GetPositionIndex(DialogueSO scene, SubsetSO subsetTalk)
        {
            if (subsetTalk.TalkerID == scene.LeftID)
            {
                return 1;
            }
            else if (subsetTalk.TalkerID == scene.CenterID)
            {
                return 2;
            }
            else if (subsetTalk.TalkerID == scene.RightID)
            {
                return 3;
            }
            else
            {
                throw new Exception($"GetCharacterPositionIndex(): Character {subsetTalk.Talker} is not in the scene layout.");
            }
        }
    }
}
