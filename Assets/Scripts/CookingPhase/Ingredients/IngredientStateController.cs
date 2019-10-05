using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]

[System.Serializable] public class IngredientStateEvent : UnityEvent<IngredientState> { }
[System.Serializable] public class IngredientCookEvent : UnityEvent<bool> { }

public class IngredientStateController : MonoBehaviour
{
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
    private int current;
    private bool isCooking;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (automaticSwitch)
        {
            IsCooking = true;
            InvokeRepeating("Switch", delayBeforeSwitch, delayBeforeSwitch);
        }
    }

    public void SwitchState(IngredientState ingredientState)
    {
        if (animator == null) { return; }

        switch (ingredientState)
        {
            case IngredientState.Undercooked:
                CurrentState = IngredientState.Undercooked;
                animator.SetTrigger("Undercooked");
                break;
            case IngredientState.Perfect:
                CurrentState = IngredientState.Perfect;
                animator.SetTrigger("Perfect");
                break;
            case IngredientState.Overcooked:
                CurrentState = IngredientState.Overcooked;
                animator.SetTrigger("Overcooked");
                break;
            default:
                CurrentState = IngredientState.Raw;
                break;
        }

        OnSwitchState.Invoke(CurrentState);
        Debug.LogFormat("{0} is now {1}", gameObject.name, CurrentState);
    }

    public void SwitchState(int ingredientState)
    {
        SwitchState((IngredientState)ingredientState);
    }

    private void Switch()
    {
        if (!IsCooking) { return; }

        current++;
        SwitchState(current);

        if (CurrentState == IngredientState.Overcooked)
        {
            CancelInvoke();
            return;
        }
    }
}

public enum IngredientState
{
    Raw, Undercooked, Perfect, Overcooked
}
