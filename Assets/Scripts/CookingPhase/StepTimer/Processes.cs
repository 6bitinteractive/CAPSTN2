﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Processes : MonoBehaviour
{
    public ProcessBox[] ProcessBoxes;
    [HideInInspector] public UnityEvent OnAllProcessesDone = new UnityEvent();
    //public int Score { get; private set; }

    private int currentProcessIndex;

    private void Start()
    {
        RunProcess(currentProcessIndex);
    }

    //private void Update()
    //{
    //    // Test success
    //    if (Input.GetMouseButtonDown(1) && (currentProcessIndex < ProcessBoxes.Length))
    //        ProcessBoxes[currentProcessIndex].SetSuccess(true);
    //}

    public void RunProcess(int processIndex)
    {
        Debug.Log("Current step: " + processIndex);
        ProcessBoxes[processIndex].SetActive(true);
    }

    //public void MoveToNextProcess()
    //{
    //    // Move to next index
    //    currentProcessIndex++;

    //    if (currentProcessIndex >= ProcessBoxes.Length)
    //    {
    //        OnAllProcessesDone.Invoke();
    //        return;
    //    }
    //    else
    //    {
    //        RunProcess(currentProcessIndex);
    //    }
    //}

    public void EndProcess(int processIndex)
    {
        ProcessBoxes[processIndex].SetSuccess(true);
    }
}
