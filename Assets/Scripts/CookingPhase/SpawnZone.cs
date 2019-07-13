using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [Header("Object Pool")]
    [SerializeField] private GameObject[] promptPrefab;
    [Tooltip("Per unique prefab; if another SpawnZone also contains the same set of prefabs, no new set of pool objects will be created.")]
    [SerializeField] private int poolSize = 3;

    [Header("Spawn Point | Range in pixels")]
    [SerializeField] private bool randomSpawnPoint;
    [SerializeField] private FloatRange horizontalRange;
    [SerializeField] private FloatRange verticalRange;

    private RectTransform rectTransform;
    private Vector2 spawnPoint => Random.insideUnitCircle * new Vector2(horizontalRange.RandomInRange, verticalRange.RandomInRange) + rectTransform.anchoredPosition;

    private static PoolManager poolManager;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Create pool
        poolManager = SingletonManager.GetInstance<PoolManager>();

        foreach (var prefab in promptPrefab)
            poolManager.CreatePool(prefab, poolSize, transform);
    }

    public void Spawn()
    {
        // Reuse from pool
        GameObject randomPrefab = promptPrefab[Random.Range(0, promptPrefab.Length)];
        // FIX: avoid repeatedly calling GetComponent?
        poolManager.ReuseObject(randomPrefab, randomSpawnPoint ? spawnPoint : randomPrefab.GetComponent<RectTransform>().anchoredPosition, Quaternion.identity);
    }
}
