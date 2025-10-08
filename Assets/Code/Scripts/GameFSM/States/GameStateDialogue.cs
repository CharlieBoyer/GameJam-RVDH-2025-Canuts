using System.Collections.Generic;
using Code.Scripts.SO;
using Code.Scripts.Utils;

namespace Code.Scripts.GameFSM.States
{
    public class GameStateDialogue: GameBaseState
    {
        /*  TODO:
         *      'GameStateDialogue' should herit any logic from the 'DialogueManager'.
         *      With it's own instance, this state can easily store dialogue states such as chapters/sequences, etc...
         *      However, this state might need to communicate with a dedicated UI module (and optionally dialogue settings class).
         */

        private List<DialogueSequenceSO> _sequenceAssets;

        private void InitState(GameStateManager context)
        {
            _manager = context;
            AddressablesUtils.LoadResources<DialogueSequenceSO>("Sequence", _manager, (assets) =>
            {
                _sequenceAssets = assets;
                _sequenceAssets.Sort();
                StartDialogue(0);
            });
        }

        public override void EnterState(GameStateManager context)
        {
            if (!_isInit)
                InitState(context);
        }

        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }

        // ----- //

        private void StartDialogue(int startIndex)
        {
            // TODO: Dialogue system startup
        }
    }
}
