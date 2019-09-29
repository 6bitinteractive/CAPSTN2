using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Choppable : MonoBehaviour
{   
    [SerializeField] private GameObject[] currentIngredientState;
    [SerializeField] private int maxIngredientSlices;
    [SerializeField] private int sliceCount;
    [SerializeField] private bool isChopped = false;
    [SerializeField] private UnityEvent onIngredientChop = new UnityEvent();
    [SerializeField] private UnityEvent onIngredientChopEnd = new UnityEvent();

    public int MaxIngredientSlices { get => maxIngredientSlices; set => maxIngredientSlices = value; }
    public bool IsChopped { get => isChopped; set => isChopped = value; }
    public UnityEvent OnIngredientChopEnd { get => onIngredientChopEnd; set => onIngredientChopEnd = value; }

    private Swipeable swipeable;

    private void Awake()
    {
        swipeable = GetComponent<Swipeable>();
    }

    void OnEnable()
    {
       maxIngredientSlices += currentIngredientState.Length;
       swipeable.SwipeDirection = currentIngredientState[sliceCount].GetComponent<Swipeable>().SwipeDirection;
    }

    public void OnChop()
    {
        if (MaxSliceReached())
        {
            isChopped = true;
            onIngredientChopEnd.Invoke();
            gameObject.SetActive(false);
            return;
        }
        
        else
        {           
            sliceCount++;
            gameObject.GetComponent<Image>().sprite = currentIngredientState[sliceCount].GetComponent<Image>().sprite; // Change sprite
            swipeable.SwipeDirection = currentIngredientState[sliceCount].GetComponent<Swipeable>().SwipeDirection; // Change direction
            onIngredientChop.Invoke();
        }
    }

    public bool MaxSliceReached()
    {
        return sliceCount >= MaxIngredientSlices - 1;
    }
}
