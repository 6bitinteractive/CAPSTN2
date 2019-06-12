using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RecipeSequence : MonoBehaviour
{
    [SerializeField] private Recipe recipe;
    public List<Step> Steps;

    private int currentStep = -1;

    public UnityEvent OnCookingPhaseEnd = new UnityEvent();

    private void OnEnable()
    {
        foreach (var step in Steps)
        {
            step.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        MoveToNextStep();
    }

    public void MoveToNextStep()
    {
        currentStep++;

        if (currentStep < Steps.Count)
        {
            Steps[currentStep].gameObject.SetActive(true);
        }
        else
        {
            // FIX: Don't suddenly jump to the Serving Phase; add some transitioning
            OnCookingPhaseEnd.Invoke();
        }
    }
}
