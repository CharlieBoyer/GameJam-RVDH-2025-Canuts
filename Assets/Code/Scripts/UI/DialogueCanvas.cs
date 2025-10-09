using System;
using Code.Scripts.GameFSM;
using Code.Scripts.SO.Dialogues;
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
        [SerializeField] private Button _nextButton;

        [Header("Character Sprites")]
        [SerializeField] private Image _leftSlot;
        [SerializeField] private Image _centerSlot;
        [SerializeField] private Image _rightSlot;

        [Header("Animation Settings")]
        [SerializeField] private float _rootFadeDuration = 0.5f;
        [SerializeField] private float _characterSmoothDuration = 0.5f;
        [SerializeField] private float _nextButtonOffsetDistance = 20f;
        [SerializeField] private float _nextButtonOffsetFrequency = 1;

        public Action OnNextDialogueClicked;
        public float NextCooldown { get; private set; }

        private Tween _typewritingTween;

        private float TypeWritingSpeed => GameStateManager.Instance.TextWritingSpeed;
        private float NextButtonOffsetFrequency => 1f / _nextButtonOffsetFrequency;

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

        private void Start()
        {
            _nextButton.interactable = true;
        }

        private void OnEnable()
        {
            // TODO: Button doesn't seem to work: check if connection/Action is properly set in the GameStateManager
            _nextButton.onClick.AddListener(() =>
            {
                if (_typewritingTween != null && _typewritingTween.IsPlaying())
                {
                    _typewritingTween.Complete(true);
                }
                else
                {
                    ShowNextButton(false);
                    OnNextDialogueClicked.Invoke();
                }
            });
        }

        private void OnDisable()
        {
            _nextButton.onClick.RemoveAllListeners();
        }

        // ----- //

        public void FadeDialogueCanvas(float alpha, bool resetSlate = false)
        {
            alpha = Mathf.Clamp(alpha, 0f, 1f);

            _root.DOFade(alpha, _rootFadeDuration);
            if (resetSlate)
                ResetState();
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
        /// <param name="scene"><c>DialogueSO</c> object from which to pull the character layout.</param>
        public void ChangeCharactersLayout(DialogueSO scene)
        {
            Sprite left = scene.Left.Sprite;
            Sprite center = scene.Center.Sprite;
            Sprite right = scene.Right.Sprite;

            SmoothSpriteUpdate(_characterLeft, left);
            SmoothSpriteUpdate(_characterCenter, center);
            SmoothSpriteUpdate(_characterRight, right);
        }

        public void UpdateActiveTalker(int activeTalker, Sprite emotion)
        {
            if (activeTalker is < 1 or > 3) {
                Debug.LogWarning("Invalid Active talker index : must be between 1 and 3. Default to 1.");
                activeTalker = 1;
            }

            switch (activeTalker)
            {
                case 1:
                    SmoothSpriteUpdate(_characterLeft, emotion);
                    break;
                case 2:
                    SmoothSpriteUpdate(_characterCenter, emotion);
                    break;
                case 3:
                    SmoothSpriteUpdate(_characterRight, emotion);
                    break;
            }

            UpdateHighlighting(_characterLeft, _characterLeft.index == activeTalker);
            UpdateHighlighting(_characterCenter, _characterCenter.index == activeTalker);
            UpdateHighlighting(_characterRight, _characterRight.index == activeTalker);
        }

        // ----- //

        private void SmoothSpriteUpdate((Image slot, int index) character, Sprite newSprite)
        {
            if (character.slot.sprite != newSprite)
            {
                character.slot.DOColor(_transparent, _characterSmoothDuration).OnComplete(() =>
                {
                    character.slot.sprite = newSprite;
                    character.slot.DOColor(_inactiveColor, _characterSmoothDuration);
                });
            }
        }

        private void UpdateHighlighting((Image slot, int index) character, bool toHighlight)
        {
            if (toHighlight && !IsHighlighted(character.slot))
            {
                character.slot.DOColor(_activeColor, _characterSmoothDuration);
            }
            else if (!toHighlight && IsHighlighted(character.slot))
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
            ShowNextButton(true);

            _dialogueTextBox.maxVisibleCharacters = 0;
            _dialogueTextBox.text = text;

            NextCooldown = text.Length / TypeWritingSpeed;

            _typewritingTween = DOTween.To(
                    () => _dialogueTextBox.maxVisibleCharacters,
                    x => _dialogueTextBox.maxVisibleCharacters = x,
                    text.Length,
                    text.Length / TypeWritingSpeed
                ).SetEase(Ease.Linear)
                .OnKill(() => { _typewritingTween = null; });
        }

        private void ShowNextButton(bool enable)
        {
            _nextButton.interactable = enable;
            _nextButton.gameObject.SetActive(enable);

            if (enable)
            {
                _nextButton.transform.DOLocalMoveY(_nextButtonOffsetDistance, _nextButtonOffsetFrequency)
                    .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                _nextButton.DOKill();
            }
        }

        // ----- //

        private void ResetState()
        {
            _characterNameBox.text = string.Empty;
            _dialogueTextBox.text = string.Empty;
            _dialogueTextBox.maxVisibleCharacters = 0;
            ShowNextButton(false);
            _leftSlot.sprite = null;
            _centerSlot.sprite = null;
            _rightSlot.sprite = null;
        }
    }
}
