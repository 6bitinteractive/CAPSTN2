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
    [SerializeField] private GameObject swipePrompt;
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
       swipeable.OnCorrectSwipe.AddListener(OnChop);
       UpdateCurrentIngredient();
    }

    public void OnChop()
    {
        if (MaxSliceReached())
        {
            isChopped = true;
            onIngredientChopEnd.Invoke();
            swipeable.SwipeDirection = SwipeDirection.None;
            return;
        }
        
        else
        {           
            sliceCount++;      
            UpdateCurrentIngredient();
            onIngredientChop.Invoke();
        }
    }

    public bool MaxSliceReached()
    {
        return sliceCount >= MaxIngredientSlices - 1;
    }

    private void UpdateCurrentIngredient()
    {
        //Note* This is starting to look like too much get components might have to clean it someday
        gameObject.GetComponent<Image>().sprite = currentIngredientState[sliceCount].GetComponent<Image>().sprite; // Change sprite
        swipeable.SwipeDirection = currentIngredientState[sliceCount].GetComponent<Swipeable>().SwipeDirection; // Change direction
        swipePrompt.GetComponent<Animator>().runtimeAnimatorController = currentIngredientState[sliceCount].GetComponent<Swipeable>().SwipeAnimatorPrompt;
    }
}
