using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Customer : MonoBehaviour
{
    [SerializeField] float PatienceTimer;
    [SerializeField] float Speed = 0.02f;

    public int myQueue;
    public Vector3 destination;

    private Timer timer;
    private Vector3 startingPosition;
    private bool isInQueue = false;

    public UnityEvent OnEnterQueue = new UnityEvent();

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
        timer.enabled = false; // Disable timer

        startingPosition = transform.position;

        curState = FSMState.Queuing;
    }

    // Update is called once per frame
    void Update()
    {
        switch (curState)
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
    //    throw new NotImplementedException();
    }

    private void QueuingState()
    {
        StartCoroutine(goToQueue());
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

    IEnumerator goToQueue()
    {
        while (!isInQueue)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, Speed * Time.deltaTime);
            transform.right = destination - transform.position; // Rotate Towards destination

            if (transform.position.x >= destination.x)
            {
                isInQueue = true;
                timer.enabled = true;
                OnEnterQueue.Invoke();
                curState = FSMState.Idle;
                yield break;
            }
            yield return 0;
        }
    }
}
