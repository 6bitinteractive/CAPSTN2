using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipIngredientObjective : Objective
{
    [SerializeField] private Cookware cookware;
    [SerializeField] private float waitTime = 10f;
    [SerializeField] private DialogueHint dialogueHint;
    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);
    [SerializeField] private ObjectiveState underState = new ObjectiveState(ObjectiveState.Status.Under);
    [SerializeField] private ObjectiveState overState = new ObjectiveState(ObjectiveState.Status.Over);

    private List<Flippable> flippableIngredients = new List<Flippable>();
    private List<IngredientStateController> ingredients = new List<IngredientStateController>();
    private IngredientState flippedState;

    protected override void Awake()
    {
        base.Awake();

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);
        ObjectiveStates.Add(underState);
        ObjectiveStates.Add(overState);

        // Define condition
        // FIX?/NOTE: This assumes that there's just one ingredient being flipped despite being a List<> and that it's the only state that's being checked
        perfectState.HasBeenReached = () => SuccessConditionMet();
        underState.HasBeenReached = () => flippedState == IngredientState.Undercooked;
        overState.HasBeenReached = () => flippedState == IngredientState.Overcooked;
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        SingletonManager.GetInstance<DialogueHintManager>().Show(dialogueHint);

        foreach (var item in cookware.CookableIngredients)
        {
            Flippable flippable = item.IngredientInCookware.GetComponent<Flippable>(); // Note: It's the IngredientInCookware that we need to check
            if (flippable)
                flippableIngredients.Add(flippable);

            Debug.Log("Flippable: " + flippable.gameObject.name);

            IngredientStateController ingredient = item.IngredientInCookware.GetComponent<IngredientStateController>();
            if (ingredient)
            {
                ingredient.OnSwitchState.AddListener((x) => flippedState = x);
                ingredients.Add(ingredient);
            }
        }
    }

    protected override void RunObjective()
    {
        base.RunObjective();

        if (Flipped())
        {
            foreach (var item in flippableIngredients)
            {
                item.Flip();
            }

            foreach (var item in ingredients)
            {
                item.IsCooking = false; // Stop the cooking
            }

            GoToNextObjective(true);
        }
    }

    private bool Flipped()
    {
        #region Standalone Input
#if UNITY_STANDALONE_WIN

        // Right-click to test flipping
        return Input.GetMouseButton(1);
#endif
        #endregion

        #region Mobile Input
#if UNITY_ANDROID || UNITY_IOS

        // Check if player tilts the phone up

#endif
        #endregion
    }

    protected override bool SuccessConditionMet()
    {
        return flippedState == IngredientState.Perfect;
    }
}
