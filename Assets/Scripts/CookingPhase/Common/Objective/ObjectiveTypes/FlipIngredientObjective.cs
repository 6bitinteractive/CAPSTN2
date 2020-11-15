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
    private Vector3 InitialTilt;
    private Vector3 Tilt;
    private bool isFlippingUpwards;

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

        SetInitialYTilt(); // Set the phone's current position
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
#if UNITY_STANDALONE_WIN || UNITY_EDITOR

        // Right-click to test flipping
        return Input.GetKeyDown(KeyCode.S);
#endif
        #endregion

        #region Mobile Input
#if UNITY_ANDROID || UNITY_IOS

        // Check if player tilts the phone up
       return CheckFlip();
#endif
        #endregion
    }

    protected override bool SuccessConditionMet()
    {
        return flippedState == IngredientState.Perfect;
    }

    private void SetInitialYTilt()
    {
        InitialTilt = Input.acceleration;
        InitialTilt.y -= 0.2f;
        InitialTilt.y = Mathf.Clamp(InitialTilt.y, -1.0f, 0);

        //Checks if its greater than the maximum threshold
        if (InitialTilt.y == -1.0f)
        {
            isFlippingUpwards = false;
            InitialTilt.y += 0.4f;
        }

        else
            isFlippingUpwards = true;

    }

    private bool CheckFlip()
    {
        Tilt = Input.acceleration;

        // Debug.Log("Initial Tilt: " + InitialTilt);
        // Debug.Log("Current Tilt: " + Tilt);

        // Flip upwards
        if (isFlippingUpwards && Tilt.y <= InitialTilt.y)
            return true;

        //Flipdownwards NOTE* This is used to counteract a bug in which if the player's phone's initial Tilt.y is at -1.0f it will automatically pass a SuccessfulInput
        else if (!isFlippingUpwards && Tilt.y >= InitialTilt.y)
            return true;
         
        return false;
    }

}
