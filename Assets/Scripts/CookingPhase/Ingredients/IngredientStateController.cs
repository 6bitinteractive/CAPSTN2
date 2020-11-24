using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]

[System.Serializable] public class IngredientStateEvent : UnityEvent<IngredientState> { }
[System.Serializable] public class IngredientCookEvent : UnityEvent<bool> { }

public class IngredientStateController : MonoBehaviour
{
    [Tooltip("Some mechanics cook the ingredient by time, i.e. automatic, while others depending on the player's action.")]
    [SerializeField] private bool automaticSwitch;

    [Tooltip("Will always be 0 (zero) if not automatic.")]
    [SerializeField] private float delayBeforeSwitch;

    public IngredientStateEvent OnSwitchState = new IngredientStateEvent();
    public IngredientCookEvent OnCookUpdate = new IngredientCookEvent();

    public IngredientState CurrentState { get; private set; }

    public bool IsCooking
    {
        get => isCooking;
        set
        {
            isCooking = value;
            OnCookUpdate.Invoke(isCooking);
        }
    }

    private Animator animator;
    private int currentState;
    private bool isCooking;
    private float currentTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (automaticSwitch)
        {
            StartCooking(new StateSwitchOption(true, true));
        }
    }

    private void Update()
    {
        if (!IsCooking) { return; }

        if (currentTime >= delayBeforeSwitch)
        {
            Switch();
            currentTime = 0;
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }

    public void SwitchState(IngredientState ingredientState)
    {
        if (animator == null) { return; }
        if (CurrentState == IngredientState.Overcooked) { return; } // No further changes can occur once overcooked

        switch (ingredientState)
        {
            case IngredientState.Undercooked:
                CurrentState = IngredientState.Undercooked;
                animator.SetInteger("State", 0);
                break;
            case IngredientState.Perfect:
                CurrentState = IngredientState.Perfect;
                animator.SetInteger("State", 1);
                break;
            case IngredientState.Overcooked:
                CurrentState = IngredientState.Overcooked;
                animator.SetInteger("State", 2);
                break;
            default:
                CurrentState = IngredientState.Raw;
                animator.SetInteger("State", 0);
                break;
        }

        OnSwitchState.Invoke(CurrentState);
        Debug.LogFormat("{0} is now {1}", gameObject.name, CurrentState);
    }

    public void SwitchState(int ingredientState)
    {
        SwitchState((IngredientState)ingredientState);
    }

    // Logically, it should continue from previous state...
    public void StartCooking(StateSwitchOption option)
    {
        IsCooking = true;

        if (!option.continuePreviousState)
        {
            currentState = 0;
            currentTime = 0;
            CurrentState = IngredientState.Raw;
        }

        if (!option.automaticallySwitchState)
            IsCooking = false;
    }

    private void Switch()
    {
        if (!IsCooking) { return; }

        currentState++;
        SwitchState(currentState);

        if (CurrentState == IngredientState.Overcooked)
        {
            IsCooking = false;
            return;
        }
    }
}

public enum IngredientState
{
    Raw, Undercooked, Perfect, Overcooked
}

[System.Serializable]
public struct StateSwitchOption
{
    public bool continuePreviousState;      // If cooking was stopped, resuming it would just proceed to the next state
    public bool automaticallySwitchState;   // Automatically switch to next state after a delay else manually switch states using the provided method above

    public StateSwitchOption(bool _continuePreviousState, bool _automaticallySwitchState)
    {
        continuePreviousState = _continuePreviousState;
        automaticallySwitchState = _automaticallySwitchState;
    }
}
