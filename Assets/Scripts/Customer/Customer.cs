using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] float PatienceTimer;
    private Timer timer;

    public enum FSMState
    {
        Idle,
        Queuing,
        Ordering,
        Waiting,
        Eating,
        Leaving
    }

    public FSMState curState; // current state

    private void Awake()
    {
        timer = GetComponent<Timer>();
        timer.TimerValue = PatienceTimer; // Set the timer

        curState = FSMState.Queuing;
    }

    // Update is called once per frame
    void Update()
    {
        switch(curState)
        {
            case FSMState.Idle: IdleState(); break;
            case FSMState.Queuing: QueuingState(); break;
            case FSMState.Ordering: OrderingState(); break;
            case FSMState.Waiting: WaitingState(); break;
            case FSMState.Eating: EatingState(); break;
            case FSMState.Leaving: LeavingState(); break;
        }
    }

    private void IdleState()
    {
        throw new NotImplementedException();
    }

    private void QueuingState()
    {
        throw new NotImplementedException();
    }

    private void OrderingState()
    {
        throw new NotImplementedException();
    }

    private void WaitingState()
    {
        throw new NotImplementedException();
    }

    private void EatingState()
    {
        throw new NotImplementedException();
    }

    private void LeavingState()
    {
        throw new NotImplementedException();
    }
}
