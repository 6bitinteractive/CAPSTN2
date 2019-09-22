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
        DialogueHintDisplay display = dialogueHints[currentDialoguePanel % dialogueHints.Count];
        display.transform.SetAsLastSibling(); // Alyways have the current display at the "top" of the canvas, i.e. it's not covered by previous displays
        display.characterPortrait.sprite = dialogueHint.characterPortrait;
        display.dialogueText.text = dialogueHint.dialogueText;
        display.Show(true);

        currentDialoguePanel++;
    }

    private void Show(ObjectiveState objectiveState)
    {
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
