using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Food/Boiling Procedure", fileName = "BoilingProcedure")]
public class BoilingProcedure : Procedure
{
    public StoveController.Temperature RequiredTemperature;

    public override IEnumerator Apply(PrepStation prepStation)
    {
        Debug.Log("Boiling... " + prepStation.BaseRecipe.DisplayName);

        // Show the boiling station
        prepStation.BoilingPanel.gameObject.SetActive(true);

        while (!Done)
        {
            yield return new WaitUntil(() => prepStation.StoveController.CurrentTemperature == RequiredTemperature);

            OnProcedureSuccess.Invoke();
            Done = true;
        }
    }

    public override void Reset()
    {
        Done = false;
    }

}
