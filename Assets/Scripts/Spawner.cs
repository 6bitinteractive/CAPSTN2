using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //public LineManager LineManager;
    [SerializeField] private Transform[] SpawnLocations;
    [SerializeField] private float Intervals;
    [SerializeField] private int NumberOfObjects = 1;
    [SerializeField] private GameObject[] PooledObject;
    [SerializeField] private List<GameObject> PooledObjectList;
    [SerializeField] private bool active = true;
    [SerializeField] private int index;
    [SerializeField] private int spawnLocationIndex;

    // Use this for initialization
    void Start()
    {
        SetObjects();
        StartCoroutine(SpawnRandomObjects());
    }

    void SetObjects()
    {
        index = 0;
        spawnLocationIndex = Random.Range(0, SpawnLocations.Length);

        for (int i = 0; i < PooledObject.Length * NumberOfObjects; i++)
        {
            GameObject objects = Instantiate(PooledObject[index], SpawnLocations[spawnLocationIndex].position, Quaternion.identity); // Spawn object prefabs
            PooledObjectList.Add(objects); // Add prefabs to the Pool object list
            PooledObjectList[i].SetActive(false); // Set prefabs as deactivated
            index++;

            // For the sake of multiplying the objects to the number of objects
            if (index == PooledObject.Length)
            {
                index = 0; // Resets index
            }
        }
    }

    IEnumerator SpawnRandomObjects()
    {
        while (active)
        {
            yield return new WaitForSeconds(Intervals);

            int i = Random.Range(0, PooledObjectList.Count); // Spawn a random prefab from pooled object list
            spawnLocationIndex = Random.Range(0, SpawnLocations.Length);

            // Checks if pooled object is not active
            if (!PooledObjectList[i].activeInHierarchy)
            {
                PooledObjectList[i].transform.position = SpawnLocations[spawnLocationIndex].position; // Sets spawn location back to originanl position
                PooledObjectList[i].SetActive(true); // Activate the pooled object

                // If this object is a customer
                Customer customer = PooledObjectList[i].GetComponent<Customer>();
                if (customer)
                {
                    customer.ResetObject();
               //     LineManager.AddToQueue(customer);
                   // customer.destination = SpawnLocations[spawnLocationIndex].position;
                }
            }
            StartCoroutine(SpawnRandomObjects()); // Restart the process
        }
    }
}
