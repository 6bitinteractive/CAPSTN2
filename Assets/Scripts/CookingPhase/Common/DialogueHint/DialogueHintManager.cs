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

    private IEnumerator Start()
    {
        foreach (var objective in objectiveManager.Objectives)
        {
            // FIX: multiple checks happening because of race condition ):
            // Skip those without any states
            if (objective.ObjectiveStates.Count == 0)
            {
                yield return new WaitForEndOfFrame(); // try waiting
                Debug.Log(objective.gameObject.name + "State Count: " + objective.ObjectiveStates.Count);

                if (objective.ObjectiveStates.Count == 0) // still nothing means there really are no states
                    continue;
            }

            foreach (var state in objective.ObjectiveStates)
            {
                state.OnStateReached.AddListener(Show);
            }
        }
    }

    private void Show(ObjectiveState objectiveState)
    {
        dialogueHintDisplay.characterPortrait.sprite = objectiveState.dialogueHint.characterPortrait;
        dialogueHintDisplay.dialogueText.text = objectiveState.dialogueHint.dialogueText;
        dialogueHintDisplay.Show(true);
    }
}
