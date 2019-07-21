using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SwipeDetector))]

public class GrillingStep : MonoBehaviour
{
    [SerializeField] private FloatRange promptDelay;
    [SerializeField] private int maxPromptsPerIngredient = 5;
    [SerializeField] private List<GameObject> ingredientsToGrill;
    [SerializeField] private List<SpawnZone> spawnZones;

    public UnityEvent OnEnd = new UnityEvent();

    private Queue<GameObject> ingredientsLeft;
    private GameObject currentIngredient;
    private GameObject currentIngredientInCookware;
    private Animator currentIngredientAnim;
    private int promptCount;

    private void Start()
    {
        ingredientsLeft = new Queue<GameObject>(ingredientsToGrill);
        //BeginCookingIngredient();
    }

    public void BeginCookingIngredient()
    {
        // Reset prompt count
        promptCount = 0;

        // Set the current ingredient
        currentIngredient = ingredientsLeft.Peek();
        currentIngredientInCookware = currentIngredient.GetComponent<Cookable>().IngredientInCookware;
        currentIngredientAnim = currentIngredientInCookware.GetComponent<Animator>();
        currentIngredient.SetActive(true);

        // Remove the ingredient from the queue of ingredients yet to be cooked
        ingredientsLeft.Dequeue();

        // Spawn the first prompt
        //Invoke("SpawnPrompt", initialPromptDelay);
    }

    public void SpawnPrompt()
    {
        Invoke("Spawn", promptDelay.RandomInRange);
    }

    public void FlipIngredient()
    {
        currentIngredientAnim.SetTrigger("Flip");
    }

    public void CheckGameState()
    {
        if (MaxPromptReached()) // Check if we're done with one ingredient
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
        else
        {
            SpawnPrompt();
        }
    }

    private bool MaxPromptReached()
    {
        Debug.Log("Current prompt count: " + promptCount);
        return promptCount >= maxPromptsPerIngredient;
    }

    private bool StageEnd()
    {
        return ingredientsLeft.Count == 0;
    }

    private void Spawn()
    {
        spawnZones[Random.Range(0, spawnZones.Count)].Spawn();
        promptCount++;
    }

    private IEnumerator BeginNext()
    {
        // Wait for animation to finish
        //yield return new WaitUntil(() => AnimatorUtils.IsInState(currentIngredientAnim, "Flip"));
        //yield return new WaitUntil(() => AnimatorUtils.IsDonePlaying(currentIngredientAnim, "Flip"));
        yield return new WaitForSeconds(2f);

        // Hide previous ingredient
        currentIngredientInCookware.SetActive(false);

        // Start next
        BeginCookingIngredient();
    }
}
