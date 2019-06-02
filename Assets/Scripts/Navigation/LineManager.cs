using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is a work in progress

public class LineManager : MonoBehaviour
{
    [SerializeField] private List<Line> LineList;
    [SerializeField] private List<Customer> CustomerList;
    private List<int> AvailableSpotIndex = new List<int>();
    [SerializeField] private bool[] isAvailable;
    private int lineIndex;

    private void Start()
    {
        for(int i = 0; i < LineList.Count; i++)
        {
            Line line = LineList[i].GetComponent<Line>();
            line.Index = i;
            line.IsAvailable = true;

         //   Debug.Log(lineIndex);
        }
    }

    public void AddToQueue(Customer customer)
    {
        // If there are any available spots

        CustomerList.Add(customer);

        for (int i = 0; i < LineList.Count; i++)
        {
            Line line = LineList[i].GetComponent<Line>();

            //Checks if there are any spots available in the line
            if (LineList[line.Index].IsAvailable)
            {
                CustomerList.Add(customer);
                customer.Destination = LineList[line.Index].transform.position;
                LineList[line.Index].IsAvailable = false;
                Debug.Log(line.Index);
            }
        }
        
    }

    /*
    public void UpdateCustomerPositions()
    {    
        for (int i = 0; i < CustomerList.Count; i++)
        {
            Customer customer = CustomerList[i].GetComponent<Customer>();
      
            customer.Destination = LineList[lineIndex - 1].transform.position; // Move up 1
            customer.MoveUpInQueue();
            Debug.Log("Customer Destination: " + customer.Destination);
        }
    }
    */
}


