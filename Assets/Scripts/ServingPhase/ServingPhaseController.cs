﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServingPhaseController : MonoBehaviour
{
    [SerializeField] private Order servingDishHeld;
    [SerializeField] Image servingDishHeldImage;
    private GameObject player;
    private Vector2 target;
    private bool isAtTargetPosition;
    private PathManager pathManager;
    private Timer timer;

    void Awake()
    {
        player = gameObject;
        pathManager = GetComponent<PathManager>();
        timer = GetComponent<Timer>();
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
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 20f);

        if (hit)
        {
            Debug.Log(hit.collider.gameObject.name);

            ServingDish servingDish = hit.collider.gameObject.GetComponent<ServingDish>();
            Customer customer = hit.collider.gameObject.GetComponent<Customer>();
            Waypoint waypoint = hit.collider.gameObject.GetComponent<Waypoint>();

            // If Serving Dish
            if (servingDish)
            {
                servingDishHeldImage.gameObject.SetActive(false);
                StartCoroutine(preppingDish(servingDish, servingDish.OrderType.PrepTime));     
                timer.TimerValue = servingDish.OrderType.PrepTime;
                timer.ResetTimer();
                timer.EnableTimer();

                Debug.Log("Prepping: " + servingDish.OrderType.Name);
                Debug.Log("PrepTime: " + servingDish.OrderType.PrepTime);
            }

            // If Customer and is ready to order
            if (customer && customer.IsReadyToOrder)
            {
                customer.curState = Customer.FSMState.WaitingForOrder;
                customer.OnOrderTaken.Invoke();
                customer.IsReadyToOrder = false;
                Debug.Log("Order Taken");
            }

            // If player has a dish held and target is a Customer waiting for order
            if (servingDishHeld != null && customer && customer.IsWaitingForOrder)
            {
                if (customer.MyOrder.Name == servingDishHeld.Name)
                {
                    customer.curState = Customer.FSMState.Eating;

                    customer.OnStartEating.Invoke();
                    Debug.Log("Correct Order");
                }

                else
                {
                    customer.curState = Customer.FSMState.Leaving;
                    Debug.Log("Wrong Order");
                }
                servingDishHeld = null;
                servingDishHeldImage.gameObject.SetActive(false);
            }

            if (waypoint)
            {
                pathManager.NavigateTo(waypoint.transform.position);
            }
        }

        IEnumerator preppingDish(ServingDish servingDish, float prepTime)
        {
            yield return new WaitForSeconds(prepTime);
            timer.DisableTimer();
            servingDishHeld = servingDish.OrderType;
            servingDishHeldImage.sprite = servingDishHeld.Sprite;
            servingDishHeldImage.gameObject.SetActive(true);
            Debug.Log("Finished prepping" + servingDish.OrderType.Name);
        }


        /*
        IEnumerator goToTarget()
        {
            while (isAtTargetPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
                transform.right = target - transform.position; // Rotate Towards destination

                if (transform.position.x >= target.x)
                {
                    
                    isAtTargetPosition = false;
                    yield break;
                }
                yield return 0;
            }
        }
        */
    }
}

