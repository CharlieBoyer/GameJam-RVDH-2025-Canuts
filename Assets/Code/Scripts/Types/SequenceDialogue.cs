using UnityEngine;
using System.Collections.Generic;
using Code.Scripts.SO;


[System.Serializable]
public class SequenceDialogue
{
    [SerializeField]
    private List<Dialogue> dialogues;

    public List<Dialogue> Dialogues => dialogues;
}
