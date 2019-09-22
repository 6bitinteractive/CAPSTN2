using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueHint
{
    public Sprite characterPortrait;

    [TextArea(1, 3)] public string dialogueText;

    public DialogueHint(Sprite _characterPortrait, string _dialogueText)
    {
        characterPortrait = _characterPortrait;
        dialogueText = _dialogueText;
    }
}
