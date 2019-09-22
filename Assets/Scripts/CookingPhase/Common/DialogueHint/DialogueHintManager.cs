using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHintManager : MonoBehaviour
{
    [SerializeField] private ObjectiveManager objectiveManager;
    [SerializeField] private DialogueHintDisplay dialogueHintDisplay;

    private void Awake()
    {
        SingletonManager.Register<DialogueHintManager>(this);
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

    public void Show(ObjectiveState objectiveState)
    {
        dialogueHintDisplay.characterPortrait.sprite = objectiveState.dialogueHint.characterPortrait;
        dialogueHintDisplay.dialogueText.text = objectiveState.dialogueHint.dialogueText;
        dialogueHintDisplay.Show(true);
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
