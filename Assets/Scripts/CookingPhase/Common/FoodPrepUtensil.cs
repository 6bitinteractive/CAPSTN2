using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(ImageOnTap))]

public class FoodPrepUtensil : MonoBehaviour
{
    [SerializeField] private Animator addIngredientAnimator;
    [SerializeField] private GameObject ingredientToSpawnPrefab;
    [SerializeField] private int minSpawnCount = 1;
    [SerializeField] private int maxSpawnCount = 3;

    public UnityEvent OnAddIngredient = new UnityEvent();

    private Image image;
    private RectTransform rectTransform;
    private Vector2 defaultPosition;
    private PolygonCollider2D polygonCollider;


    private void Awake()
    {
        image = GetComponent<Image>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        rectTransform = GetComponent<RectTransform>();
        defaultPosition = rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        polygonCollider.enabled = true;
    }

    public void SpawnIngredient(Collider2D collider2D)
    {
        // Avoid triggering the collider multiple times
        polygonCollider.enabled = false;

        StartCoroutine(AddIngredient(collider2D));
    }

    public void Reset()
    {
        rectTransform.anchoredPosition = defaultPosition;
        image.enabled = true;
        polygonCollider.enabled = true;
    }

    private IEnumerator AddIngredient(Collider2D collider2D)
    {
        // Hide default image of utensil (the one that can be dragged by the player)
        image.enabled = false;

        // Start animation of adding in the ingredient
        addIngredientAnimator.gameObject.SetActive(true);
        addIngredientAnimator.SetTrigger("Add");

        // Wait for animation to finish
        yield return new WaitUntil(() => AnimatorUtils.IsInState(addIngredientAnimator, "FoodPrepUtensilAddIngredient")
                                        && AnimatorUtils.IsDonePlaying(addIngredientAnimator, "FoodPrepUtensilAddIngredient"));

        // Hide
        addIngredientAnimator.gameObject.SetActive(false);

        // Have a random count if necessary
        int randomSpawnCount = Random.Range(minSpawnCount, maxSpawnCount);
        int currentCount = 0;

        while (currentCount < randomSpawnCount)
        {
            // Spawn at the same x-axis as this utensil, randomize the y based on the Cookware's polygonCollider's bounds
            Vector3 position = new Vector3(transform.position.x, Random.Range(collider2D.bounds.min.y, collider2D.bounds.max.y), 0f);
            GameObject go = Instantiate(ingredientToSpawnPrefab, position, Quaternion.identity, collider2D.transform);

            currentCount++;
            //Debug.Log("Spawned ingredient count: " + currentCount);
        }

        // Broadcast that the ingredient has been added
        OnAddIngredient.Invoke();

        // Reset back to default image; wait for a few seconds so that enabling back the collider won't continuosly trigger spawning of ingredients
        yield return new WaitForSeconds(1.5f); // FIX? hard-coded
        Reset();
    }
}
