using System.Collections.Generic;
using Code.Scripts.SO;
using Code.Scripts.UI;
using Code.Scripts.Utils;
using UnityEngine;

namespace Code.Scripts.GameFSM.States
{
    public class GameStateIntro: GameBaseState
    {
        /*  TODO:
         *      'GameStateIntro' is responsible for preparing 'GameStateDialogue'.
         *      This also include any content that could be played/displayed before the dialogue system starts.
         */

        private IntroCanvas _introCanvas;

        private List<ContextSO> _contextAssets;
        private int _contextIndex = 0;

        // ----- //

        private void InitState(GameStateManager context)
        {
            _manager = context;
            _introCanvas = Object.FindFirstObjectByType<IntroCanvas>();

            LoadContextAssets();

            _isInit = true;
        }

        public override void EnterState(GameStateManager context)
        {
            if (!_isInit)
                InitState(context);

            _introCanvas.OnIntroCanvasClick += ShowNextContextSentence;
        }

        public override void UpdateState()
        {
        }

        public override void ExitState()
        {
            _introCanvas.OnIntroCanvasClick -= ShowNextContextSentence;
        }

        // ----- //

        private void ShowNextContextSentence()
        {
            _introCanvas.ShowInTextBox(_contextAssets[_contextIndex].Text);
            _contextIndex++;

            if (_contextIndex > _contextAssets.Count)
            {
                _introCanvas.FadeOutBlackScreen();
            }
        }

        private void LoadContextAssets()
        {
            AddressablesUtils.LoadResources<ContextSO>("Context", _manager, assets =>
            {
                _contextAssets = assets;
                _contextAssets.Sort((a, b) => a.IndexID.CompareTo(b.IndexID));

                ShowNextContextSentence();
            });
        }
    }
}
