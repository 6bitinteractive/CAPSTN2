using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Food/Boiling Procedure", fileName = "BoilingProcedure")]
public class BoilingProcedure : Procedure
{
    public override void Apply(Serving serving)
    {
        Debug.Log("Boiling... " + serving.BaseRecipe.DisplayName);
        serving.ProcessesPanel.OnAllProcessesDone.AddListener(() => Debug.Log("Boiling... End."));

        // Show process boxes
        serving.ProcessesPanel.gameObject.SetActive(true);
    }
}
