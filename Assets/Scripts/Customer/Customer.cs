using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Timer))]

public class Customer : MonoBehaviour
{
    public Menu Menu;
    [SerializeField] float PatienceTimer = 10;
    [SerializeField] float Speed = 0.02f;
    [SerializeField] float OrderingDuration = 3;
    [SerializeField] float EatingDuration = 3.5f;
    [SerializeField] Image OrderPrompt;

    //   public int myQueue;
    public Vector3 Destination;

    private Chair myChair;
    private Order myOrder;
    private Timer timer;
    private DroppableToChair droppableToChair;
    private SpriteOutline spriteOutline;
    private Vector3 startingPosition;
    private bool isInQueue = false;
    private bool isReadyToOrder = false;
    private bool isWaitingForOrder = false;

    // Events
    public UnityEvent OnEnterQueue = new UnityEvent();
    public UnityEvent OnStartOrdering = new UnityEvent();
    public UnityEvent OnOrderingEnd = new UnityEvent();
    public UnityEvent OnOrderTaken = new UnityEvent();
    public UnityEvent OnStartEating = new UnityEvent();
    public UnityEvent OnEatingEnd = new UnityEvent();

    // FSM Boolean Control
    private bool isOrdering = true;
    private bool isWaiting = true;
    private bool isEating = true;

    public enum FSMState
    {
        Idle,
        Queuing,
        Ordering,
        ReadyToOrder,
        Eating,
        Leaving,
        WaitingForOrder
    }

    public FSMState curState; // current state

    public Chair MyChair { get => myChair; set => myChair = value; }
    public bool IsReadyToOrder { get => isReadyToOrder; set => isReadyToOrder = value; }
    public bool IsWaitingForOrder { get => isWaitingForOrder; set => isWaitingForOrder = value; }
    public Order MyOrder { get => myOrder; set => myOrder = value; }
    public SpriteOutline SpriteOutline { get => spriteOutline; set => spriteOutline = value; }

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
            case FSMState.Queuing: QueuingState(); break; // Used if AI has movement
            case FSMState.Ordering: OrderingState(); break;
            case FSMState.WaitingForOrder: WaitingForOrder(); break;
            case FSMState.Eating: EatingState(); break;
            case FSMState.Leaving: LeavingState(); break;
        }
    }

    private void IdleState()
    {
        // Play an idling animation
        //Debug.Log("Idle");
    }

    private void QueuingState()
    {
        //StartCoroutine(goToQueue());
    }

    private void OrderingState()
    {     
        if (isOrdering)
        {
            Destroy(gameObject.GetComponent<DroppableToChair>(), 0);
            OnStartOrdering.Invoke();
            timer.DisableTimer();        
            StartCoroutine(ordering());
            isOrdering = false;
        }
    }

    private void WaitingForOrder()
    {
        if (isWaiting)
        {
            OrderPrompt.gameObject.SetActive(true); // Display the order's sprite
            OrderPrompt.sprite = myOrder.Sprite;
            OnOrderTaken.Invoke();
            isWaitingForOrder = true;
            timer.EnableTimer();
            timer.ResetTimer();
            isWaiting = false;
        }
    }

    private void EatingState()
    {
        if (isEating)
        {
            OrderPrompt.gameObject.SetActive(false);
            StartCoroutine(eating());
            OnStartEating.Invoke();
            timer.DisableTimer();

        }
    }

    public void LeavingState()
    {
        if (myChair != null)
            MyChair.isOccupied = false;

        gameObject.SetActive(false);
        // Customer dissappears through an effect or walks out?

    }

    /* Incase we add movement to the AI
    IEnumerator goToQueue()
    {
        while (!isInQueue)
        {
            MoveUpInQueue();

            if (transform.position.x >= Destination.x)
            {
                isInQueue = true;
                 timer.EnableTimer();
                gameObject.AddComponent<DroppableToChair>(); // Hack for some reason even when the script is disabled player can still drag the object
                OnEnterQueue.Invoke();
                curState = FSMState.Idle;
                yield break;
            }
            yield return 0;
        }
    }
    */

    IEnumerator ordering()
    {
        yield return new WaitForSeconds(OrderingDuration);
        int randomOrder = UnityEngine.Random.Range(0, Menu.MenuList.Count); // Get a random order from the menu
        MyOrder = Menu.MenuList[randomOrder];   
        isReadyToOrder = true;
        OnOrderingEnd.Invoke();
       // timer.EnableTimer();
       // timer.ResetTimer();
        curState = FSMState.WaitingForOrder;
        Debug.Log("I Ordered:" + MyOrder.Name);
    }

    IEnumerator eating()
    {
        yield return new WaitForSeconds(EatingDuration);
        OnEatingEnd.Invoke();
    }

    public void ResetObject()
    {
        //   isInQueue = false;
        MyChair = null;
        MyOrder = null;
        isReadyToOrder = false;
        isWaitingForOrder = false;
        isOrdering = true;
        isWaiting = true;
        spriteOutline.enabled = false;
        OnEnterQueue.Invoke();
        curState = FSMState.Idle;
        OrderPrompt.gameObject.SetActive(false);
        timer.ResetTimer();
        Destroy(gameObject.GetComponent<DroppableToChair>(), 0);
        gameObject.AddComponent<DroppableToChair>();
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
        startingPosition = transform.position;
        spriteOutline = GetComponent<SpriteOutline>();
        spriteOutline.enabled = false;
        curState = FSMState.Idle;
    }
}
