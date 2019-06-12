using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ServingPhaseController : MonoBehaviour
{
    [SerializeField] private Order servingDishHeld;
    [SerializeField] Image servingDishHeldImage;
    [SerializeField] private LayerMask LayerMask;

    public UnityEvent OnPreppingStart = new UnityEvent();
    public UnityEvent OnPreppingEnd = new UnityEvent();

    private GameObject player;
    private Vector2 target;
    private PathManager pathManager;
    private Timer timer;
    private bool isMoving = false;
    private Animator animator;

    void Awake()
    {
        player = gameObject;
        pathManager = GetComponent<PathManager>();
        timer = GetComponent<Timer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CastRay();
        }
    }

    void CastRay()
    {
        // Create ray cast from mouse input
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector2 mouseDirection = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 20f, LayerMask);

        if (hit)
        {
            Debug.Log(hit.collider.gameObject.name);

            ServingDish servingDish = hit.collider.gameObject.GetComponent<ServingDish>();
            Customer customer = hit.collider.gameObject.GetComponent<Customer>();
            Waypoint waypoint = hit.collider.gameObject.GetComponent<Waypoint>();

            // Actions unavailable when moving
            if (!isMoving)
            {         
                // If player has a dish held and target is a Customer waiting for order
                if (servingDishHeld != null && customer && customer.IsWaitingForOrder)
                {
                    pathManager.NavigateTo(customer.transform.position); // Move to customer
                    //Debug.Log("Last Node: " + pathManager.LastNode.gameObject.name); // check last node
                    StartCoroutine(serveDish(customer)); // Serve customer
                    isMoving = true;
                    animator.SetBool("isMoving", true);
                }

                // If Serving Dish
                if (servingDish && servingDishHeld == null)
                {
                    pathManager.NavigateTo(pathManager.StartNode.transform.position); // Move to serving dish           
                    servingDishHeldImage.gameObject.SetActive(false);               
                    timer.TimerValue = servingDish.OrderType.PrepTime;  
                    StartCoroutine(preppingDish(servingDish, servingDish.OrderType.PrepTime));
                    isMoving = true;
                    animator.SetBool("isMoving", true);
                    
                    Debug.Log("Prepping: " + servingDish.OrderType.Name);
                    Debug.Log("PrepTime: " + servingDish.OrderType.PrepTime);
                }
            }

            /*
            // If Customer and is ready to order
            if (customer && customer.IsReadyToOrder)
            {
                customer.curState = Customer.FSMState.WaitingForOrder;
                customer.OnOrderTaken.Invoke();
                customer.IsReadyToOrder = false;
                

               // StartCoroutine(goToTarget(customer.gameObject));
               


                // Debug.Log("Order Taken");
            }
            */

            /*
            if (waypoint)
            {
                pathManager.NavigateTo(waypoint.transform.position);
            }
            */
        }

        IEnumerator preppingDish(ServingDish servingDish, float prepTime)
        {             
            bool isAtTargetPosition = false;
            while (!isAtTargetPosition)
            {
                if (gameObject.transform.position == pathManager.LastNode.transform.position)
                {
                    OnPreppingStart.Invoke();
                    timer.ResetTimer();
                    timer.EnableTimer();
                    yield return new WaitForSeconds(prepTime);
                    OnPreppingEnd.Invoke();
                    timer.DisableTimer();
                    servingDishHeld = servingDish.OrderType;
                    servingDishHeldImage.sprite = servingDishHeld.Sprite;
                    servingDishHeldImage.gameObject.SetActive(true);
                    isMoving = false;
                    isAtTargetPosition = true;
                    Debug.Log("Finished prepping" + servingDish.OrderType.Name);
                    yield break;
                }
                yield return 0;
            }       
        }

        IEnumerator serveDish(Customer customer)
        {
            bool isAtTargetPosition = false;
            while (!isAtTargetPosition)
            {
               // float currentDistance = Vector3.Distance(transform.position, customer.transform.position);
               // Debug.Log(currentDistance);
                if (gameObject.transform.position == pathManager.LastNode.transform.position)
                {
                    // If correct order
                    if (customer.MyOrder.Name == servingDishHeld.Name)
                    {
                        customer.curState = Customer.FSMState.Eating;
                        customer.OnStartEating.Invoke();
                        Debug.Log("Correct Order");
                    }

                    // If wrong order
                    else
                    {
                        customer.curState = Customer.FSMState.Leaving;
                        Debug.Log("Wrong Order");
                    }

                    servingDishHeld = null;
                    servingDishHeldImage.gameObject.SetActive(false);
                    isMoving = false;
                    isAtTargetPosition = true;
                    yield break;
                }
                yield return 0;
            }
        }
    }
}