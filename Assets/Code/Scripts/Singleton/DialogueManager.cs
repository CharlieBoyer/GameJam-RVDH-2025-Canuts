using System;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts.SO;
using Code.Scripts.Types;
using Code.Scripts.Utils;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Code.Scripts.Singleton
{
    public class DialogueManager : SingletonMonoBehaviour<DialogueManager>
    {
        public GameObject UI;

        public TextMeshProUGUI DialogueText;
        public TextMeshProUGUI NameText;

        public Image CharacterLeft;
        public Image CharacterRight;
        public Image CharacterMiddle;

        private Queue<string> _sentences;
        private int _indexList = 0;
        private int _indexDialogue = 0;
        public float DialogueSpeed;

        public List<SequenceDialogue> Sequences = new();

        // ---- //

        public static Action OnDialogueSequenceEnd;

        private void Start()
        {
            _sentences = new Queue<string>();

            StartDialogue(0,0);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("NextSentence");
                DisplayNextSentence();
            }
        }

        public void StartDialogue(int indexList, int indexDialogue)
        {
            NameText.text = Sequences[indexList].Dialogues[indexDialogue].characterName;
            _sentences.Clear();

            ChangeCharacterDisplay(Sequences[indexList].Dialogues[indexDialogue]);

            foreach (string sentence in Sequences[indexList].Dialogues[indexDialogue].dialogueSentences)
            {
                _sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (GameManager.Instance.DialogueIndex == Sequences.Count)
            {
                SceneLoader.Instance.Load("Menu");
            }

            if (_sentences.Count == 0)
            {
                EndDialogue();
                if (_indexDialogue == Sequences[GameManager.Instance.DialogueIndex].Dialogues.Count)
                {
                    OnDialogueSequenceEnd?.Invoke();
                }
                return;
            }

            string sentence = _sentences.Dequeue();
            DialogueText.text = sentence;
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        void EndDialogue()
        {
            Debug.Log("End conversation");
        }

        IEnumerator TypeSentence(string sentence)
        {
            DialogueText.text = "";
            foreach (char character in sentence.ToCharArray())
            {
                DialogueText.text += character;
                //yield return new waitforseconds(dialoguespeed);
                yield return null;
            }
        }

        public void ChangeCharacterDisplay(DialogueSO dialogueSO)
        {
            CharacterLeft.gameObject.SetActive(false);
            CharacterRight.gameObject.SetActive(false);
            CharacterMiddle.gameObject.SetActive(false);

            if (dialogueSO.pos == POSITION.LEFT)
            {
                CharacterLeft.gameObject.SetActive(true);
                CharacterLeft.sprite = dialogueSO.characterSprite;
            }
            else
            if (dialogueSO.pos == POSITION.RIGHT)
            {
                CharacterRight.gameObject.SetActive(true);
                CharacterRight.sprite = dialogueSO.characterSprite;
            }
            else if (dialogueSO.pos == POSITION.MIDDLE)
            {
                CharacterMiddle.gameObject.SetActive(true);
                CharacterMiddle.sprite = dialogueSO.characterSprite;
            }
        }
    }
}
