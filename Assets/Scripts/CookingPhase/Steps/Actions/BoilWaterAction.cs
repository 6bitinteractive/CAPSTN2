using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilWaterAction : Action
{
    [SerializeField] private GameObject boilingWater;
    [SerializeField] private GameObject stoveController;

    public override void ResetAction()
    {
        boilingWater.SetActive(false);
        stoveController.SetActive(false);
    }

    public override void Begin()
    {
        boilingWater.SetActive(true);
        stoveController.SetActive(true);
        Active = true;
    }
}
