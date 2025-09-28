using System;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts.SO;
using Code.Scripts.Utils;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Code.Scripts.Singleton
{
    public class DialogueManager : SingletonMonoBehaviour<DialogueManager>
    {
        public TextMeshProUGUI dialogueText;
        public TextMeshProUGUI nameText;

        public Image characterLeft;
        public Image characterRight;
        public Image characterMiddle;

        private Queue<string> sentences;
        private int indexList = 0;
        private int indexDialogue = 0;
        public float dialogueSpeed;
        
        public List<SequenceDialogue> sequences = new();

        private void Start()
        {
            sentences = new Queue<string>();

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
            Debug.Log("index" + indexList);
            nameText.text = sequences[indexList].Dialogues[indexDialogue].characterName;
            sentences.Clear();

            ChangeCharacterDisplay(sequences[indexList].Dialogues[indexDialogue]);

            foreach (string sentence in sequences[indexList].Dialogues[indexDialogue].dialogueSentences)
            {

                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                indexDialogue++;
                if (indexDialogue == sequences[indexList].Dialogues.Count)
                {
                    indexList++;
                    indexDialogue = 0;
                }
                StartDialogue(indexList,indexDialogue);
                return;
            }

            string sentence = sentences.Dequeue();
            dialogueText.text = sentence;
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        void EndDialogue()
        {
            Debug.Log("End conversation");
        }

        IEnumerator TypeSentence(string sentence)
        {
            dialogueText.text = "";
            foreach (char character in sentence.ToCharArray())
            {
                dialogueText.text += character;
                //yield return new waitforseconds(dialoguespeed);
                yield return null;
            }
        }

        public void ChangeCharacterDisplay(Dialogue dialogue)
        {
            characterLeft.gameObject.SetActive(false);
            characterRight.gameObject.SetActive(false);
            characterMiddle.gameObject.SetActive(false);

            if (dialogue.pos == POSITION.LEFT)
            {
                characterLeft.gameObject.SetActive(true);
                characterLeft.sprite = dialogue.characterSprite;
            }
            else
            if (dialogue.pos == POSITION.RIGHT)
            {
                characterRight.gameObject.SetActive(true);
                characterRight.sprite = dialogue.characterSprite;
            }
            else if (dialogue.pos == POSITION.MIDDLE)
            {
                characterMiddle.gameObject.SetActive(true);
                characterMiddle.sprite = dialogue.characterSprite;
            }
        }
    }
}
