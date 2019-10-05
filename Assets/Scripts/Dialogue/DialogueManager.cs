using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image leftSpeaker;
    public Image rightSpeaker;
    public Animator animator;
    public Button nextButton;
    public PlayerAdventureController playerAdventureController;

    private Queue<Dialogue> dialogue;
    private Dialogue lastSentence;
    public UnityEvent OnFullDialogueEnd; // Note: Added this to avoid creating different GameEvent triggers

    void Awake()
    {
        dialogue = new Queue<Dialogue>();
        if (OnFullDialogueEnd == null) OnFullDialogueEnd = new UnityEvent();
    }

    public void StartDialogue(DialogueTrigger dialogueTrigger)
    {
        if ((animator != null) && (animator.isActiveAndEnabled))
        {
            animator.SetBool("IsOpen", true);
        }

        nextButton.image.raycastTarget = true;

        // Clear previous sentences
        dialogue.Clear();

        foreach (Dialogue dialogueEntry in dialogueTrigger.dialogueArray)
        {
            dialogue.Enqueue(dialogueEntry);
            lastSentence = dialogueEntry;
        }

        // Stop player from moving
        if (playerAdventureController != null)
        {
            playerAdventureController.enabled = false;
        }

          DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        // If no dialogue entries remain, end dialogue
        if (dialogue.Count <= 0)
        {
            EndDialogue();
            return;
        }

        Dialogue dialogueEntry = dialogue.Dequeue();
        // Makes sure all sentence animations are stopped before typing in new sentence
        
        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogueEntry));
    }

    // Types sentence per letter
    IEnumerator TypeSentence(Dialogue dialogueEntry)
    {
        nameText.text = dialogueEntry.name;

        // Checks the portraits if they are null
        CheckLeftPortrait(dialogueEntry);
        CheckRightPortrait(dialogueEntry);
        dialogueText.text = dialogueEntry.sentence;
        dialogueEntry.onEndSentence.Invoke();
        yield return null;
        //dialogueText.text = "";

        /*
        foreach (char letter in dialogueEntry.sentence.ToCharArray())
        {
            // Adds letter to dialogue text string every after 1 frame
            dialogueText.text += letter;
           
        }
        */
    }

    public void ChangeDialogueText(TextMeshProUGUI newText)
    {
        // Makes sure all sentence animations are stopped before changing dialogue text
        StopAllCoroutines();
        dialogueText = newText;
    }

    public void EndDialogue()
    {
        StartCoroutine(EndingDialogue());
    }

    private IEnumerator EndingDialogue()
    {
        yield return new WaitForSeconds(0.5f); // Fixes freezing bug when player presses skip button too fast
        OnFullDialogueEnd.Invoke();

        if ((animator != null) && (animator.isActiveAndEnabled))
        {
            animator.SetBool("IsOpen", false);
        }

        // Lets player move
        if (playerAdventureController != null)
        {
            playerAdventureController.enabled = true;
        }

        nextButton.image.raycastTarget = false;

        lastSentence.onEndSentence.Invoke();
    }

        public void CheckRightPortrait(Dialogue dialogueEntry)
    {

        // If its null take the last portrait and set it as current portrait
        if (dialogueEntry.rightSpeaker == null)
        {
            rightSpeaker.gameObject.SetActive(false);
            return;
        }
        else
        {
            rightSpeaker.gameObject.SetActive(true);
            rightSpeaker.sprite = dialogueEntry.rightSpeaker;
        }
    }

    public void CheckLeftPortrait(Dialogue dialogueEntry)
    {
        if (dialogueEntry.leftSpeaker == null)
        {
            leftSpeaker.gameObject.SetActive(false);
            return;
        }
        else
        {
            leftSpeaker.gameObject.SetActive(true);
            leftSpeaker.sprite = dialogueEntry.leftSpeaker;
        }
    }
}
