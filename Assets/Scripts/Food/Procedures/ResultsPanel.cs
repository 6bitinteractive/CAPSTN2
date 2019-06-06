using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI SuccessText, FailText;

    public void ShowPanel(bool procedureSuccess)
    {
        gameObject.SetActive(true);
        SuccessText.gameObject.SetActive(procedureSuccess);
        FailText.gameObject.SetActive(!procedureSuccess);
    }
}
