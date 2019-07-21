using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFeedback : MonoBehaviour
{
    private List<SpawnZone> spawnZones = new List<SpawnZone>();

    private void Awake()
    {
        spawnZones.AddRange(GetComponentsInChildren<SpawnZone>());
    }

    // FIX: Don't assume; For now, we assume that the list(spawners) of text feedback has the following order: Awesome, Good, Oh No
    public void DisplayTextFeedback(PromptRating promptRating)
    {
        switch (promptRating)
        {
            case PromptRating.Miss:
            case PromptRating.Awful:
            case PromptRating.Bad:
                spawnZones[2].Spawn();
                break;
            case PromptRating.Good:
                spawnZones[1].Spawn();
                break;
            case PromptRating.Great:
            case PromptRating.Perfect:
                spawnZones[0].Spawn();
                break;
        }
    }
}