using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Progress : MonoBehaviour
{
    // TODO: Set the value for the total count
    private int totalCount; // Value will come from/depend on something else (eg. the number of objectives in a stage)
    private int currentCount;

    public float NormalizedValue { get { return totalCount == 0 ? 0 : (float)currentCount / (float)totalCount; } }
    public UnityEvent OnProgressUpdate = new UnityEvent();

    public void UpdateCount(int value = 1)
    {
        currentCount += value;
        OnProgressUpdate.Invoke();
    }
}
