using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsUI : MonoBehaviour
{
    [SerializeField] private RecipeSequence recipeSequence;
    [SerializeField] private TextMeshProUGUI instructionText;
    //[SerializeField] private Image instructionTimer; // TODO: implement this

    private void OnEnable()
    {
        foreach (var step in recipeSequence.Steps)
        {
            foreach (var action in step.Actions)
                action.OnBegin.AddListener(UpdateInstructionText);
        }
    }

    private void OnDisable()
    {
        foreach (var step in recipeSequence.Steps)
        {
            foreach (var action in step.Actions)
                action.OnBegin.RemoveListener(UpdateInstructionText);
        }
    }

    public void UpdateInstructionText(Action action)
    {
        instructionText.text = action.Instruction;
    }
}
