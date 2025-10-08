using System;
using System.Collections;
using Code.Scripts.GameFSM;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Code.Scripts.UI
{
    public class DialogueCanvas: MonoBehaviour
    {
        [Header("UI Hierarchy")]
        [SerializeField] private CanvasGroup _root;
        [SerializeField] private TMP_Text _characterNameBox;
        [SerializeField] private TMP_Text _dialogueTextBox;

        [Header("Character Sprites")]
        [SerializeField] private Image _leftSlot;
        [SerializeField] private Image _centerSlot;
        [SerializeField] private Image _rightSlot;

        [Header("Animation Settings")]
        [SerializeField] private float _characterSmoothDuration = 0.5f;

        private float TypeWritingSpeed => GameStateManager.Instance.TextWritingSpeed;

        private (Image slot, int index) _characterLeft;
        private (Image slot, int index) _characterCenter;
        private (Image slot, int index) _characterRight;

        private readonly Color _transparent = new(1,1,1,0f);
        private readonly Color _inactiveColor = new(1,1,1,0.5f);
        private readonly Color _activeColor = new(1,1,1,1f);

        // ----- //

        private void Awake()
        {
            _characterLeft   = new ValueTuple<Image, int>(_leftSlot, 1);
            _characterCenter = new ValueTuple<Image, int>(_centerSlot, 2);
            _characterRight  = new ValueTuple<Image, int>(_rightSlot, 3);
        }

        // ----- //

        /// <summary>
        /// Update all text boxes for the assumed talking character.
        /// </summary>
        /// <param name="characterName">Replacement for the talker name.</param>
        /// <param name="dialogue">Sentence to display</param>
        public void Talk(string characterName, string dialogue)
        {
            _characterNameBox.text = characterName;
            StartTyping(dialogue);
        }

        /// <summary>
        /// Update the character sprites layout according to who's present and who's talking.
        /// </summary>
        /// <param name="left">Character's sprite. Can be null if no character is present.</param>
        /// <param name="center">Character's sprite. Can be null if no character is present.</param>
        /// <param name="right">Character's sprite. Can be null if no character is present.</param>
        /// <param name="activeTalker">Index from 1 to 3 to identify the talking character and highlight him.</param>
        public void ChangeCharactersLayout([CanBeNull] Sprite left, [CanBeNull] Sprite center, [CanBeNull] Sprite right, int activeTalker)
        {
            if (activeTalker is < 1 or > 3) {
                Debug.LogError("Invalid Active talker index : must be between 1 and 3. Default to 1.");
                activeTalker = 1;
            }

            SmoothSpriteUpdate(_characterLeft, left, _characterLeft.index == activeTalker);
            SmoothSpriteUpdate(_characterCenter, center, _characterCenter.index == activeTalker);
            SmoothSpriteUpdate(_characterRight, right, _characterRight.index == activeTalker);
        }

        // ----- //

        private void SmoothSpriteUpdate((Image slot, int index) character, Sprite newSprite, bool highlight)
        {
            if (character.slot.sprite != newSprite)
            {
                character.slot.DOColor(_transparent, _characterSmoothDuration).OnComplete(() =>
                {
                    character.slot.sprite = newSprite;
                    character.slot.DOColor((highlight ? _activeColor : _inactiveColor), _characterSmoothDuration);
                });
            }
            else if (highlight && !IsHighlighted(character.slot))
            {
                character.slot.DOColor(_activeColor, _characterSmoothDuration);
            }
            else if (!highlight && IsHighlighted(character.slot))
            {
                character.slot.DOColor(_inactiveColor, _characterSmoothDuration);
            }
        }

        private bool IsHighlighted(Image slot)
        {
            return Mathf.Approximately(slot.color.a, 1f);
        }

        private void StartTyping(string text)
        {
            _dialogueTextBox.maxVisibleCharacters = 0;
            _dialogueTextBox.text = text;

            DOTween.To(
                () => _dialogueTextBox.maxVisibleCharacters,
                x => _dialogueTextBox.maxVisibleCharacters = x,
                text.Length,
                text.Length / TypeWritingSpeed
            ).SetEase(Ease.Linear);
        }
    }
}
