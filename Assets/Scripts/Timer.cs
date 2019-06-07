using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float TimerValue = 10;
    public Canvas TimerCanvas;
    public UnityEvent OnCountdownEnd;

    private float currentTimerValue;

    public float CurrentTimerValue
    {
        get
        {
            return currentTimerValue;
        }

        set
        {
            currentTimerValue = value;
        }
    }

    void Start()
    {
        if (OnCountdownEnd == null) OnCountdownEnd = new UnityEvent();

        CurrentTimerValue = TimerValue;
        StartCoroutine(StartCountdown());
    }

    void Update()
    {
        // If countdown ends call CountdownEnd function
        if (CurrentTimerValue <= 0)
        {
            CurrentTimerValue = 0f;
            CountdownEnd();
        }
    }

    public IEnumerator StartCountdown()
    {
        while (CurrentTimerValue > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime); // Scale timer
            CurrentTimerValue -= Time.deltaTime; // Reduce timer
        }
    }

    private void CountdownEnd()
    {
        if (OnCountdownEnd != null) OnCountdownEnd.Invoke(); // Broadcast to listeners that countdown has ended
    }

    public void ResetTimer()
    {
        CurrentTimerValue = TimerValue;
        StartCoroutine(StartCountdown());
    }

    public void DisableTimer()
    {
        TimerCanvas.enabled = false;
        this.enabled = false;
    }

    public void EnableTimer()
    {
        TimerCanvas.enabled = true;
        this.enabled = true;
    }
}
