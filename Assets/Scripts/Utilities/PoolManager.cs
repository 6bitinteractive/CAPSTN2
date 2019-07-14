using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

    private void OnEnable()
    {
        SingletonManager.Register<PoolManager>(this);
    }

    private void OnDisable()
    {
        SingletonManager.UnRegister<PoolManager>();
    }

    public void CreatePool(GameObject prefab, int poolSize, Transform source = null)
    {
        int poolKey = prefab.GetInstanceID();

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

            // To keep things organized, place objects under one object
            GameObject poolHolder = null;
            if (source == null) // If source parent is undefined
            {
                poolHolder = new GameObject(prefab.name + " Pool");
                poolHolder.transform.parent = transform;
            }

            for (int i = 0; i < poolSize; i++)
            {
                ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
                poolDictionary[poolKey].Enqueue(newObject);

                if (source == null)
                    newObject.SetParent(poolHolder.transform);
                else
                    newObject.SetParent(source);
            }
        }
    }

    public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objectToReuse);

            objectToReuse.Reuse(position, rotation);
        }
    }

    // Store information about the instance of game object
    public class ObjectInstance
    {
        private GameObject gameObject;
        private Transform transform;
        private RectTransform rectTransform;

        private bool hasPoolObjectComponent; // Not all objects might have the PoolObject script, ie. it's not enforced
        private PoolObject poolObjectScript;

        public ObjectInstance(GameObject objectInstance)
        {
            gameObject = objectInstance;

            transform = gameObject.transform;
            rectTransform = objectInstance.GetComponent<RectTransform>();
            gameObject.SetActive(false);

            if (gameObject.GetComponent<PoolObject>())
            {
                hasPoolObjectComponent = true;
                poolObjectScript = gameObject.GetComponent<PoolObject>();
            }
        }

        public void Reuse(Vector3 position, Quaternion rotation)
        {
            gameObject.SetActive(true);

            if (rectTransform == null)
            {
                transform.position = position;
                transform.rotation = rotation;
            }
            else
            {
                rectTransform.anchoredPosition = position;
            }

            if (hasPoolObjectComponent)
                poolObjectScript.OnObjectReuse();
        }

        public void SetParent(Transform parent, bool worldPositionStays = false)
        {
            transform.SetParent(parent, worldPositionStays);
        }
    }
}

// Reference:
// https://www.youtube.com/watch?v=LhqP3EghQ-Q
// https://github.com/SebLague/Object-Pooling
