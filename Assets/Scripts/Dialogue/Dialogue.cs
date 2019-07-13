using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public string name;
    public Sprite leftSpeaker;
    public Sprite rightSpeaker;

    [TextArea(3, 10)]
    public string sentence;
    public UnityEvent onEndSentence;
}