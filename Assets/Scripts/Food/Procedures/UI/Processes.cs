using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Processes : MonoBehaviour
{
    public ProcessBox[] ProcessBoxes;

    [HideInInspector] public UnityEvent OnAllProcessesDone = new UnityEvent();

    private int currentProcessIndex;

    private void OnEnable()
    {
        foreach (var processBox in ProcessBoxes)
        {
            processBox.OnProcessDone.AddListener(MoveToNextProcess);
        }
    }

    private void Start()
    {
        ProcessBoxes[currentProcessIndex].SetActive(true);
    }

    private void Update()
    {
        // Test success
        if (Input.GetMouseButtonDown(0))
            ProcessBoxes[currentProcessIndex].SetSuccess(true);
    }

    private void MoveToNextProcess()
    {
        // Move to next index
        currentProcessIndex++;
        Debug.Log("Index: " + currentProcessIndex);

        if (currentProcessIndex >= ProcessBoxes.Length)
        {
            OnAllProcessesDone.Invoke();
            return;
        }
        else
        {
            ProcessBoxes[currentProcessIndex].SetActive(true);
        }
    }
}
