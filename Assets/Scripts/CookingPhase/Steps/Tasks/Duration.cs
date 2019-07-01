using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Duration : MonoBehaviour
{
    public float CurrentTime;
    public float MaxDuration = 10f;
    public bool Countdown = true;
    public bool End { get; private set; }

    public UnityEvent OnTimerStart = new UnityEvent();
    public UnityEvent OnTimerEnd = new UnityEvent();
    public UnityEvent OnTimerUpdate = new UnityEvent();

    private void Start()
    {
        if (Countdown)
            CurrentTime = MaxDuration;
    }

    public IEnumerator StartTimer()
    {
        Debug.Log("Timer started.");
        OnTimerStart.Invoke();

        while (!End)
        {
            if (Countdown)
            {
                CurrentTime -= Time.deltaTime;

                if (CurrentTime <= 0f)
                    EndTimer();
            }
            else
            {
                CurrentTime += Time.deltaTime;

                if (CurrentTime >= MaxDuration)
                    EndTimer();
            }

            OnTimerUpdate.Invoke();

            //Debug.Log("Current time: " + CurrentTime);
            yield return null;
        }
    }

    private void EndTimer()
    {
        End = true;
        OnTimerEnd.Invoke();
    }
}
