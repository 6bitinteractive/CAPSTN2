using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Food/Chopping Procedure", fileName = "ChoppingProcedure")]
public class ChoppingProcedure : Procedure
{
    public override void Apply(PrepStation prepStation)
    {
        Debug.Log("Chopping... " + prepStation.BaseRecipe.DisplayName);

        prepStation.ChoppingPanel.gameObject.SetActive(true);

        //OnProcedureDone.Invoke();
    }
}
