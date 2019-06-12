using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddIngredientAction : Action
{
    [SerializeField] private List<GameObject> ingredients;
    [SerializeField] private List<GameObject> ingredientsInCookware;

    [Header("Timer")]
    [SerializeField] private Image instructionTimer; // TODO: separate component/implement InstructionUI's
    public float ActionDuration = 10f;

    private float timer;

    private void Update()
    {
        if (!Active) { return; }

        timer += Time.deltaTime;
        Debug.Log("Timer: " + timer);

        instructionTimer.fillAmount = Mathf.Clamp(timer / ActionDuration, 0, 1);

        if (timer >= ActionDuration)
        {
            bool success = SuccessConditionMet();
            Debug.Log("Added all ingredients: " + success);

            if (success)
                OnSuccess.Invoke(this);
            else
                OnFail.Invoke(this);

            OnEnd.Invoke(this);
            Active = false;
        }
    }

    public override void ResetAction()
    {
        foreach (var ingredient in ingredients)
            ingredient.SetActive(false);

        foreach (var ingredient in ingredientsInCookware)
            ingredient.SetActive(false);
    }

    public override void Begin()
    {
        foreach (var ingredient in ingredients)
        {
            ingredient.SetActive(true);
        }

        OnBegin.Invoke(this);
        Active = true;
    }

    public override bool SuccessConditionMet()
    {
        // If one ingredient hasn't been enabled (ie. not in the cookware), the action is considered a failure
        Successful = !ingredientsInCookware.Exists(x => !x.gameObject.activeInHierarchy);

        // TODO: rewrite; take this out of here
        if (Successful)
            OnSuccess.Invoke(this);
        else
            OnFail.Invoke(this);

        return Successful;
    }
}
