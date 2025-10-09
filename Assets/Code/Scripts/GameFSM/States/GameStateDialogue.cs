using System;
using System.Collections;
using System.Collections.Generic;
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

        private DialogueCanvas _canvas;

        private List<SequenceSO> _sequences;
        private List<DialogueSO> _parts;
        private List<SubsetSO> _subsets;

        private SequenceSO _activeSequence;
        private DialogueSO _activePart;
        private SubsetSO _activeSubset;

        private int _sequenceIndex = 0;
        private int _partIndex = 0;
        private int _subsetIndex = 0;

        // ----- //

        private void InitState(GameStateManager context)
        {
            _manager = context;
            _canvas = Object.FindFirstObjectByType<DialogueCanvas>();

            if (!_canvas)
                throw new Exception("[GameStateDialogue] No DialogueCanvas script-object found in the scene.");

            CharacterRef.Init(_manager);

            LoadDialogueAssets(); // Internal callback starts the dialogue system.

            _isInit = true;
        }

        public override void EnterState(GameStateManager context)
        {
            if (!_isInit)
                InitState(context);

            _canvas.OnNextDialogueClicked += ManageActivePart;
        }

        public override void UpdateState() {}

        public override void ExitState()
        {
            _canvas.OnNextDialogueClicked -= ManageActivePart;
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
            if (_partIndex >= _parts.Count)
            {
                _sequenceIndex++;
                EndSequence();
            }

            _activePart = _activeSequence.Parts[_partIndex];
            _activeSubset = _activePart.Subsets[_subsetIndex];

            _canvas.Talk(_activeSubset.Talker, _activeSubset.Text);
            _canvas.UpdateActiveTalker(GetPositionIndex(_activePart, _activeSubset), _activeSubset.Sprite);

            _subsetIndex++;

            if (_subsetIndex >= _activePart.Subsets.Count)
            {
                _subsetIndex = 0;
                _partIndex++;
            }
        }

        private void EndSequence()
        {
            _manager.StartCoroutine(EndSequenceCoroutine());
        }

        private IEnumerator EndSequenceCoroutine()
        {
            _canvas.FadeDialogueCanvas(0f, true);
            yield return new WaitForSeconds(_manager.GameStateTransitionDelay);
            _manager.SwitchState(_manager.States.Trial);
        }

        // ----- //

        private void LoadDialogueAssets()
        {
            AddressablesUtils.LoadResources<SequenceSO>("Sequences", _manager, (assets) =>
            {
                _sequences = assets;
                _sequences.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));
                StartSequence(_sequenceIndex);
            });
        }

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
