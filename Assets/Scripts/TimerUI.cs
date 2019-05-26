using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private Timer timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = GetComponent<Timer>();

        timerText.text = timer.CurrentTimerValue.ToString("F2");
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.TimerValue >= 0f)
        {
            timerText.text = timer.CurrentTimerValue.ToString("F2");
        }
    }
}
