using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Saver : MonoBehaviour
{
    // A unique string set to identify what is being saved
    [SerializeField] protected string uniqueIdentifier;
    [SerializeField] protected SaveData saveData;

    // A string to identify what is being saved
    // It is set using information about the data as well as the `uniqueIdentifier`
    protected string key;

    private void Awake()
    {
        key = SetKey();
    }

    private void OnEnable()
    {
        // Subscribe the Save function to the BeforeSceneUnload event
        SceneController.BeforeSceneUnload += Save;

        // Subscribe the Load function to the AfterSceneLoad event
        SceneController.AfterSceneLoad += Load;
    }

    private void OnDisable()
    {
        // Usubscribe the Save/Load functions
        SceneController.BeforeSceneUnload -= Save;
        SceneController.AfterSceneLoad -= Load;
    }

    // The key must be unique across all Saver scripts
    protected abstract string SetKey();

    // Calls saveData.Save, passing in the key and relevant data
    protected abstract void Save();

    // Calls saveData.Load with a `ref` parameter to get the data out
    protected abstract void Load();
}
