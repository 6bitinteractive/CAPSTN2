using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Timer))]

public class Customer : MonoBehaviour
{
    [SerializeField] float PatienceTimer;
    [SerializeField] float Speed = 0.02f;
    [SerializeField] float OrderingDuration = 3;

    public int myQueue;
    public Vector3 Destination;

    private Chair MyChair;
    private Timer timer;
    private DroppableToChair droppableToChair;
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

    public Chair MyChair1 { get => MyChair; set => MyChair = value; }

    private void Awake()
    {
        Init();
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
        // Play an idling animation
        // throw new NotImplementedException();
    }

    private void QueuingState()
    {
        StartCoroutine(goToQueue());
    }

    private void OrderingState()
    {
        timer.ResetTimer();
        Destroy(gameObject.GetComponent<DroppableToChair>(),1);
        StartCoroutine(ordering());
        // Order for a set duration
        // Select an order
        // Wait for player click to advance to waiting state
        //Debug.Log("Im ordering");
    }

    private void WaitingState()
    {
        // Reset Patience Timer
        // Able to accept order
        // Upon accepting order go to eating state
        throw new NotImplementedException();
    }

    private void EatingState()
    {
        // Eat for a set duration
        // Upon finished eating move to leaving state
        throw new NotImplementedException();
    }

    private void LeavingState()
    {
        // Earn Score
        // Set the isOccupied for MyChair to false
        // Customer dissappears through an effect or walks out?
        throw new NotImplementedException();
    }

    IEnumerator goToQueue()
    {
        while (!isInQueue)
        {
            MoveUpInQueue();

            if (transform.position.x >= Destination.x)
            {
                isInQueue = true;
                timer.enabled = true;
                gameObject.AddComponent<DroppableToChair>(); // Hack for some reason even when the script is disabled player can still drag the object
                OnEnterQueue.Invoke();
                curState = FSMState.Idle;
                yield break;
            }
            yield return 0;
        }
    }

    IEnumerator ordering()
    {
        yield return new WaitForSeconds(OrderingDuration);
        // Select an order here
        curState = FSMState.Waiting;
    }

    public void ResetObject()
    {
        MyChair1 = null;
    }

    public void MoveUpInQueue()
    {
        transform.position = Vector3.MoveTowards(transform.position, Destination, Speed * Time.deltaTime);
        transform.right = Destination - transform.position; // Rotate Towards destination
    }

    private void Init()
    {
        timer = GetComponent<Timer>();
        timer.TimerValue = PatienceTimer; // Set the timer
        timer.enabled = false; // Disable timer

        startingPosition = transform.position;

        curState = FSMState.Queuing;
    }
}
