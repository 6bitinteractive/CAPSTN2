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
    [SerializeField] private float resetPositionDelay = 0.5f;

    [Header("Spawn Ingredient")]
    [SerializeField] private GameObject ingredientToSpawnPrefab;
    [SerializeField] private int minSpawnCount = 1;
    [SerializeField] private int maxSpawnCount = 3;
    [SerializeField] private SpawnType spawnType = SpawnType.Offset;

    [Header("SpawnType: Position Offset from Utensil")]
    [SerializeField] private Vector3 spawnPositionOffset;

    [Header("SpawnType: Specified Position/s")]
    public List<RectTransform> spawnPositionList;

    [Header("Event")]
    public UnityEvent OnAddIngredient = new UnityEvent();

    private Image image;
    private RectTransform rectTransform;
    private Vector2 defaultPosition;
    private PolygonCollider2D polygonCollider;
    private Queue<RectTransform> spawnPositions = new Queue<RectTransform>();

    private void Awake()
    {
        image = GetComponent<Image>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        rectTransform = GetComponent<RectTransform>();
        defaultPosition = rectTransform.anchoredPosition;

        foreach (var item in spawnPositionList)
            spawnPositions.Enqueue(item);
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
        // Make sure collider is disabled
        yield return new WaitUntil(() => polygonCollider.enabled == false);

        // Hide default image of utensil (the one that can be dragged by the player)
        image.enabled = false;

        // Start animation of adding in the ingredient
        if (addIngredientAnimator != null)
        {
            addIngredientAnimator.gameObject.SetActive(true);
            addIngredientAnimator.SetTrigger("Add");

            // Wait for animation to finish
            yield return new WaitUntil(() => AnimatorUtils.IsInState(addIngredientAnimator, "FoodPrepUtensilAddIngredient")
                                            && AnimatorUtils.IsDonePlaying(addIngredientAnimator, "FoodPrepUtensilAddIngredient"));

            // Hide
            addIngredientAnimator.gameObject.SetActive(false);
        }

        if (ingredientToSpawnPrefab != null) // If there's something to spawn in the cookware...
        {
            // Have a random count if necessary
            int randomSpawnCount = Random.Range(minSpawnCount, maxSpawnCount);
            int currentCount = 0;
            Vector3 position = Vector3.zero;
            Transform parent = collider2D.transform;

            while (currentCount < randomSpawnCount)
            {
                switch (spawnType)
                {
                    case SpawnType.InPosition:
                        {
                            position = ingredientToSpawnPrefab.transform.position;
                            break;
                        }

                    case SpawnType.Offset:
                        {
                            position = new Vector3(transform.position.x, transform.position.y, 0f) + spawnPositionOffset;
                            break;
                        }

                    case SpawnType.SpecifiedPosition:
                        {
                            var queueItem = spawnPositions.Peek();
                            parent = queueItem.transform;
                            position = queueItem.position;
                            spawnPositions.Dequeue();
                            spawnPositions.Enqueue(queueItem);
                            break;
                        }

                    case SpawnType.Random:
                        {
                            // Spawn at the same x-axis as this utensil, randomize the y based on the Cookware's polygonCollider's bounds
                            position = new Vector3(transform.position.x, Random.Range(collider2D.bounds.min.y, collider2D.bounds.max.y), 0f);
                            break;
                        }
                }

                GameObject go = Instantiate(ingredientToSpawnPrefab, position, Quaternion.identity, parent);

                currentCount++;
                Debug.Log("Spawned ingredient count: " + currentCount);
            }
        }

        // Broadcast that the ingredient has been added
        OnAddIngredient.Invoke();

        // Reset back to default image; wait for a few seconds so that enabling back the collider won't continuosly trigger spawning of ingredients
        yield return new WaitForSeconds(resetPositionDelay);

        Reset();
    }

    public enum SpawnType
    {
        InPosition, Offset, SpecifiedPosition, Random
    }
}
