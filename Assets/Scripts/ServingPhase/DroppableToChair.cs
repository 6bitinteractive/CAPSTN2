﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableToChair : MonoBehaviour
{
    private bool grabbed;
    private Chair chair;
    private Vector3 originalPos;

    private void Update()
    {
        if (grabbed)
        {
            Drag();
        }
    }

    private void Drag()
    {
        #region Standard Input
#if UNITY_STANDALONE_WIN
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f); // set z-position to 0 to avoid it going "invisible" (being set to -10)
#endif
        #endregion

        #region Mobile Input
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                case TouchPhase.Moved:
                    transform.position = new Vector3(touchPosition.x, touchPosition.y, 0f);
                    break;
            }
        }
#endif
        #endregion
    }

    private void OnMouseDown()
    {
        grabbed = true;
        originalPos = transform.position; // Store original position
    }

    private void OnMouseUp()
    {
        grabbed = false;

        // Drop to chair
        if (chair != null && !chair.isOccupied)
        {
            Debug.Log("Dropped " + gameObject.name + " to " + chair.name);
            chair.isOccupied = true;

            Customer customer = gameObject.GetComponent<Customer>();
            if (customer)
            {
                customer.transform.right = chair.transform.right;
                customer.MyChair = chair; // Set the customer's chair
                customer.curState = Customer.FSMState.Ordering;
            }
        }

        else
        {
           transform.position = originalPos; // Return to original position
        }
    }
   
    private void OnTriggerStay2D(Collider2D collision)
    {
        chair = collision.GetComponent<Chair>();         
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        chair = null;
    }  
}
