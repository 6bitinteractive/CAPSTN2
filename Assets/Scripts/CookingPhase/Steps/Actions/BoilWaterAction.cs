using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoilWaterAction : Action
{
    [SerializeField] private GameObject boilingWaterObj;
    [SerializeField] private GameObject stoveController;

    [Header("Timer")]
    [SerializeField] private Image instructionTimer; // TODO: separate component/implement InstructionUI's
    public float ActionDuration = 10f;

    private float timer;
    private Boilable boilingWater;

    private void Awake()
    {
        boilingWater = boilingWaterObj.GetComponent<Boilable>();
    }

    private void Update()
    {
        if (!Active) { return; }

        timer += Time.deltaTime;
        Debug.Log("Timer: " + timer);

        instructionTimer.fillAmount = Mathf.Clamp(timer / ActionDuration, 0, 1);

        if (timer >= ActionDuration)
        {
            bool success = SuccessConditionMet();
            Debug.Log("Boiling water successful: " + success);

            OnEnd.Invoke(this);
            Active = false;
        }
    }

    public override void ResetAction()
    {
        boilingWaterObj.SetActive(false);
        stoveController.SetActive(false);
    }

    public override void Begin()
    {
        boilingWaterObj.SetActive(true);
        stoveController.SetActive(true);
        OnBegin.Invoke(this);
        Active = true;
    }

    public override bool SuccessConditionMet()
    {
        Successful = boilingWater.BoiledAtRightTemperature;

        // TODO: rewrite; take this out of here
        if (Successful)
            OnSuccess.Invoke(this);
        else
            OnFail.Invoke(this);

        return Successful;
    }
}
