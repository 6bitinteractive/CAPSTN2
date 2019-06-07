using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public enum TimerTypes
    {
        Text,
        Image
    }

    [SerializeField] private TimerTypes TimerType;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image timerImage;
    private Timer timer;

    public TextMeshProUGUI TimerText { get => timerText; set => timerText = value; }
    public Image TimerImage { get => timerImage; set => timerImage = value; }

    // Start is called before the first frame update
    void Start()
    {
        timer = GetComponent<Timer>();

        switch (TimerType)
        {
            case TimerTypes.Text:
                TimerText.text = timer.CurrentTimerValue.ToString("F2");
                break;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.TimerValue >= 0f)
        {
            switch (TimerType)
            {
                case TimerTypes.Text:
                    TimerText.text = timer.CurrentTimerValue.ToString("F2");
                    break;

                case TimerTypes.Image:
                    timerImage.fillAmount = timer.CurrentTimerValue / timer.TimerValue;
                    break;
            }       
        }
    }
}
