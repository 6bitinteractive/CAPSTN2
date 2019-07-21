using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class ChoppingStep : MonoBehaviour
{
    [SerializeField] private float initialPromptDelay = 6f;
    [SerializeField] private float promptDelay = 1.5f;
    [SerializeField] private int maxIngredientSlices = 11;
    [SerializeField] private List<GameObject> ingredientsToSlice;

    public UnityEvent OnEnd = new UnityEvent();

    private Queue<GameObject> ingredientsLeft;
    private GameObject currentIngredient;
    [SerializeField] private Sprite[] currentIngredientState;
    private int sliceCount;

    private void Start()
    {
        ingredientsLeft = new Queue<GameObject>(ingredientsToSlice);
        BeginSlicingIngredients();
    }
    public void BeginSlicingIngredients()
    {
        sliceCount = 0;
        currentIngredient = ingredientsLeft.Peek();
        currentIngredient.SetActive(true);

        ingredientsLeft.Dequeue();
    }
    public void SliceIngredient()
    {
        Debug.Log("Working!");
        sliceCount++;
        currentIngredient.GetComponent<Image>().sprite = currentIngredientState[sliceCount];
    }
    public void CheckGameState()
    {
        if (MaxSliceReached()) // Check if we're done with one ingredient
        {
            if (StageEnd()) // Check if no ingredients left
            {
                OnEnd.Invoke();
                return;
            }
            else // Start with the next
            {
                StopAllCoroutines();
                StartCoroutine(BeginNext());
            }
        }
    }
    public bool MaxSliceReached()
    {
        return sliceCount >= maxIngredientSlices;
    }
    private IEnumerator BeginNext()
    {
        // Wait for animation to finish
        //yield return new WaitUntil(() => AnimatorUtils.IsInState(currentIngredientAnim, "Flip"));
        //yield return new WaitUntil(() => AnimatorUtils.IsDonePlaying(currentIngredientAnim, "Flip"));
        yield return new WaitForSeconds(2f);

        // Hide previous ingredient
        currentIngredient.SetActive(false);

        // Start next
        BeginSlicingIngredients();
    }
    private bool StageEnd()
    {
        return ingredientsLeft.Count == 0;
    }
}
