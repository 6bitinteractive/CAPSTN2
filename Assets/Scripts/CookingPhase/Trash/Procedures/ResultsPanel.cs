using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI SuccessText, FailText, ResultsList;

    public void ShowResult(Step step)
    {
        string text = "";
        for (int i = 0; i < step.Actions.Count; i++)
        {
            text += string.Format("Task {0}: {1} - {2}\n", i+1, step.Actions[i].Instruction, step.Actions[i].Successful ? "Success" : "Fail");
        }

        ResultsList.text = text;

        ShowPanel(step.Successful);
    }

    private void ShowPanel(bool procedureSuccess)
    {
        gameObject.SetActive(true);
        SuccessText.gameObject.SetActive(procedureSuccess);
        FailText.gameObject.SetActive(!procedureSuccess);
    }
}
