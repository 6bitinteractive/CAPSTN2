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

        while (true)
        {
            if (prepStation.StoveController.CurrentTemperature == RequiredTemperature)
            {
                yield return new WaitForSeconds(2f); // Show a success panel prompt?
                OnProcedureSuccess.Invoke();
            }

            yield return null;
        }
    }

    public override void Reset()
    {
        //throw new System.NotImplementedException();
    }
}
