using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Food/Boiling Procedure", fileName = "BoilingProcedure")]
public class BoilingProcedure : Procedure
{
    private Processes processes;

    public override void Apply(PrepStation prepStation)
    {
        Debug.Log("Boiling... " + prepStation.BaseRecipe.DisplayName);
        processes = prepStation.ProcessesPanel;

        // Listen to events
        processes.OnAllProcessesDone.AddListener(() =>
        {
            Debug.Log("Boiling... End.");

            // Test; manually switch to next step/procedure
            OnProcedureDone.Invoke();
        });

        foreach (var process in processes.ProcessBoxes)
            process.OnProcessDone.AddListener(() => prepStation.UpdateScoreUI(process.Success ? process.ScoreAddition : process.ScoreDeduction));

        // Show relevant UI
        prepStation.ProcessesPanel.gameObject.SetActive(true);
        prepStation.BoilingPanel.gameObject.SetActive(true);
    }
}
