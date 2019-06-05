using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Food/Boiling Procedure", fileName = "BoilingProcedure")]
public class BoilingProcedure : Procedure
{
    public override IEnumerator Apply(PrepStation prepStation)
    {
        Debug.Log("Boiling... " + prepStation.BaseRecipe.DisplayName);
        prepStation.BoilingPanel.gameObject.SetActive(true);

        yield return null;
    }

    public override void Reset()
    {
        //throw new System.NotImplementedException();
    }
}
