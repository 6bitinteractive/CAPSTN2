using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueHintDisplay : MonoBehaviour
{
    [SerializeField] private float delay = 3f;

    public Image characterPortrait;
    public TextMeshProUGUI dialogueText;

    // FIX: Redundant functions (Show, Hide)
    private void OnEnable()
    {
        Invoke("Hide", delay);
    }

    public void Show(bool value = false)
    {
        gameObject.SetActive(value);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
