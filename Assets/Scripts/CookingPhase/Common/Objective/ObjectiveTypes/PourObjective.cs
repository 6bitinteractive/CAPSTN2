using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FIX: Hard-coded; for water only D:

public class PourObjective : Objective
{
    [SerializeField] private WaterStateController water; // FIX: Need to allow any type of liquid
    [SerializeField] private float requiredPourDuration = 2f;
    [SerializeField] private float requiredPouringAngle = -0.3f;
    [SerializeField] private DialogueHint dialogueHint;

    [SerializeField] private ObjectiveState perfectState = new ObjectiveState(ObjectiveState.Status.Perfect);
    [SerializeField] private ObjectiveState underState = new ObjectiveState(ObjectiveState.Status.Under);
    [SerializeField] private ObjectiveState overState = new ObjectiveState(ObjectiveState.Status.Over);

    private float currentPourDuration;
    private int currentWaterState = 9;
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
        perfectState.HasBeenReached = () => currentWaterState == 14;
        underState.HasBeenReached = () => currentWaterState > 10 && currentWaterState < 14 && !IsPouring();
        overState.HasBeenReached = () => currentWaterState == 15;
    }

    protected override void InitializeObjective()
    {
        base.InitializeObjective();
        SingletonManager.GetInstance<DialogueHintManager>().Show(dialogueHint);
    }

    protected override void RunObjective()
    {
        base.RunObjective();

        // FIX: Hard-coded; abstract these and shove to WaterStateController
        if (currentWaterState >= 16) { return; }

        if (IsPouring())
        {
            // For first, have water appear right away
            if (currentWaterState == 9)
            {
                currentWaterState++;
                GoToNextObjective(false); // Show next button at this point
                water.SwitchState(currentWaterState);
            }

            currentPourDuration += Time.deltaTime;
        }

        if (currentPourDuration >= requiredPourDuration)
        {
            currentPourDuration = 0;
            currentWaterState++;
            water.SwitchState(currentWaterState);
        }
    }

    protected override void PostFinalizeObjective()
    {
        base.PostFinalizeObjective();
        SwitchToPerfectState();
    }

    protected override bool SuccessConditionMet()
    {
        return currentWaterState == 14;
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

    private void SwitchToPerfectState()
    {
        if (currentWaterState != 14) // Poured too much/little water
        {
            currentWaterState = 14;
            water.SwitchState(currentWaterState);

            DialogueHint dialogue = new DialogueHint(dialogueHint.characterPortrait, string.Empty);
            dialogue.dialogueText = "Let's adjust the amount of water.";
            SingletonManager.GetInstance<DialogueHintManager>().Show(dialogue);
        }
    }
}
