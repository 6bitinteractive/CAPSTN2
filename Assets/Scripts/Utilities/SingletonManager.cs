using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SingletonManager
{
    private static Dictionary<System.Type, MonoBehaviour> singletonInstances = new Dictionary<System.Type, MonoBehaviour>();

    static SingletonManager()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public static T GetInstance<T>() where T : MonoBehaviour
    {
        return singletonInstances[typeof(T)] as T;
    }

    public static void Register<T>(MonoBehaviour obj) where T : MonoBehaviour
    {
        singletonInstances.Add(typeof(T), obj);
    }

    public static void UnRegister<T>() where T : MonoBehaviour
    {
        singletonInstances.Remove(typeof(T));
    }

    private static void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("Scene unloaded: " + scene.name);

        //var instancesToRemove = singletonInstances
        //    .Where(x => x.Value.gameObject.scene == scene)
        //    .Select(x => x.Key)
        //    .ToList();

        var keys = new List<System.Type>(singletonInstances.Keys);
        keys.Select(x => singletonInstances[x].gameObject.scene == scene);

        foreach (var key in keys)
        {
            singletonInstances.Remove(key);
            Debug.Log("Removed:" + key.Name);
        }
    }
}
