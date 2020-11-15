using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHintManager : MonoBehaviour
{
    [SerializeField] private ObjectiveManager objectiveManager;
    [SerializeField] private Transform dialogueHintPanel;
    [SerializeField] private GameObject dialogueHintDisplayPrefab;
    [SerializeField] private int poolCount = 3;

    private List<DialogueHintDisplay> dialogueHints = new List<DialogueHintDisplay>();
    private int currentDialoguePanel;

    private void Awake()
    {
        SingletonManager.Register<DialogueHintManager>(this);

        // Create a reusable pool of dialogueHintDisplay objects
        for (int i = 0; i < poolCount; i++)
        {
            GameObject obj = Instantiate(dialogueHintDisplayPrefab);
            obj.transform.SetParent(dialogueHintPanel, false);
            dialogueHints.Add(obj.GetComponent<DialogueHintDisplay>());
        }
    }

    private void OnDestroy()
    {
        SingletonManager.UnRegister<DialogueHintManager>();
    }

    private void Start()
    {
        foreach (var objective in objectiveManager.Objectives)
        {
            objective.OnBegin.AddListener(SubscribeToEvent);
        }
    }

    public void Show(DialogueHint dialogueHint)
    {
        int previousDialoguePanelIndex = (dialogueHints.Count + currentDialoguePanel - 1) % dialogueHints.Count;
        DialogueHintDisplay display = dialogueHints[currentDialoguePanel % dialogueHints.Count], 
            previousDisplay = dialogueHints[previousDialoguePanelIndex];

        // If the previous display has the same text and it is still shown, don't run
        if (previousDisplay.dialogueText.text == dialogueHint.dialogueText && previousDisplay.shown)
            return;

        // If the previous display is still shown, hide it for the new hint
        if (previousDisplay.shown) previousDisplay.Hide();

        display.transform.SetAsLastSibling(); // Alyways have the current display at the "top" of the canvas, i.e. it's not covered by previous displays
        display.characterPortrait.sprite = dialogueHint.characterPortrait;
        display.dialogueText.text = dialogueHint.dialogueText;
        display.Show(true);

        currentDialoguePanel++;
    }

    private void Show(ObjectiveState objectiveState)
    {
        // If there's no dialogue, don't show the dialogue hint panel
        if (objectiveState.dialogueHint.dialogueText?.Length == 0) { return; }

        Show(objectiveState.dialogueHint);
    }

    private void SubscribeToEvent(Objective objective)
    {
        if (objective.ObjectiveStates.Count == 0)
        {
            Debug.Log(objective.gameObject.name + "State Count: " + objective.ObjectiveStates.Count);
            return;
        }

        foreach (var state in objective.ObjectiveStates)
        {
            state.OnStateReached.AddListener(Show);
        }
    }
}
