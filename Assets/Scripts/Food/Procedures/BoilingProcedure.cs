using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Food/Create Boiling Procedure", fileName = "BoilingProcedure")]
public class BoilingProcedure : Procedure
{
    public override void Apply(Serving serving)
    {
        if (Done) { return; }

        Debug.Log("Boiling... " + serving.BaseRecipe.DisplayName);
    }
}
