using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSaver : Saver
{
    [SerializeField] private Resource resourceToSave;

    protected override string SetKey()
    {
        return resourceToSave.name + resourceToSave.GetType().FullName + uniqueIdentifier;
    }

    protected override void Save()
    {
        Debug.Log(resourceToSave.name + ": " + resourceToSave.Value);
        saveData.Save(key, resourceToSave.Value);
    }

    protected override void Load()
    {
        int value = 0;

        if (saveData.Load(key, ref value))
            resourceToSave.Value = value;
    }
}
