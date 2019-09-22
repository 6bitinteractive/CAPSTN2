using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourObjective : Objective
{
    [SerializeField] private WaterStateController water;
    [SerializeField] private float requiredPourDuration = 2f;
    [SerializeField] private float requiredPouringAngle = -0.3f;
    [SerializeField] private DialogueHint dialogueHint;

    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);
    [SerializeField] private ObjectiveState underState = new ObjectiveState(ObjectiveState.Status.Under);
    [SerializeField] private ObjectiveState overState = new ObjectiveState(ObjectiveState.Status.Over);

    private float currentPourDuration;
    private int currentWaterState = 10;
    private Vector3 InitialTilt;
    private Vector3 Tilt;

    protected override void Awake()
    {
        base.Awake();

        // Setup objectives
        // Add to list
        ObjectiveStates.Add(perfectState);
        ObjectiveStates.Add(underState);
        ObjectiveStates.Add(overState);

        // Define condition
        perfectState.HasBeenReached = () => currentWaterState == 15; // 15 for now so that it ends up with the still animation
        underState.HasBeenReached = () => currentWaterState > 10 && currentWaterState < 15 && !IsPouring();
        overState.HasBeenReached = () => false; // TODO: Implement when art assets are done
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        SingletonManager.GetInstance<DialogueHintManager>().Show(dialogueHint);
    }

    protected override void RunObjective()
    {
        base.RunObjective();

        // FIX: Hard-coded
        if (currentWaterState >= 15) { return; }

        if (IsPouring())
        {
            // For first, have water appear right away
            if (currentWaterState == 10)
            {
                water.SwitchState(currentWaterState);
                currentWaterState++;
            }

            currentPourDuration += Time.deltaTime;
        }

        if (currentPourDuration >= requiredPourDuration)
        {
            currentPourDuration = 0;
            water.SwitchState(currentWaterState);
            currentWaterState++;
        }
    }

    protected override bool SuccessConditionMet()
    {
        return currentWaterState >= 14;
    }

    private bool IsPouring()
    {

        #region Standalone Input
#if UNITY_STANDALONE_WIN
        return Input.GetMouseButton(0);
#endif
        #endregion

        #region Mobile Input
#if UNITY_ANDROID || UNITY_IOS

        Tilt = Input.acceleration;

        Debug.Log("Current Tilt: " + Tilt);

        // Pouring Leftwards
        if (Tilt.x <= requiredPouringAngle)
        {
            Debug.Log("Pouring");
            return true;
        }

        return false;
#endif
        #endregion
    }
}
