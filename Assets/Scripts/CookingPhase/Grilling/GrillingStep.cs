using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SwipeDetector))]

public class GrillingStep : MonoBehaviour
{
    [SerializeField] private List<GameObject> ingredientsToGrill;
    [SerializeField] private int maxPromptsPerIngredient = 5;
    [SerializeField] private List<SpawnZone> spawnZones;

    public UnityEvent OnEnd = new UnityEvent();

    private Queue<GameObject> ingredientsLeft;
    private GameObject currentIngredient;
    private Animator currentIngredientAnim;
    private int promptCount;

    private void Start()
    {
        ingredientsLeft = new Queue<GameObject>(ingredientsToGrill);
        BeginCookingIngredient();
    }

    public void BeginCookingIngredient()
    {
        promptCount = 0;
        currentIngredient = ingredientsLeft.Peek();
        currentIngredientAnim = currentIngredient.GetComponent<Animator>();
        currentIngredient.SetActive(true);
        ingredientsLeft.Dequeue();
    }

    public void SpawnPrompt()
    {
        spawnZones[Random.Range(0, spawnZones.Count)].Spawn();
        promptCount++;
    }

    public void FlipIngredient()
    {
        currentIngredientAnim.SetTrigger("Flip");
    }

    private void CheckState()
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
                BeginCookingIngredient();
            }
        }
    }

    private bool MaxPromptReached()
    {
        return promptCount >= maxPromptsPerIngredient;
    }

    private bool StageEnd()
    {
        return ingredientsLeft.Count == 0;
    }
}
