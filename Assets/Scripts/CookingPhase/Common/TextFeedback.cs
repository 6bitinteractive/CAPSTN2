using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TextFeedback : MonoBehaviour
{
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip[] clips;
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
                PlaySFX(0);
                break;
            case PromptRating.Good:
                spawnZones[1].Spawn();
                PlaySFX(1);
                break;
            case PromptRating.Great:
            case PromptRating.Perfect:
                spawnZones[0].Spawn();
                PlaySFX(1);
                break;
        }
    }
    public void PlaySFX(int index)
    {
        sfx.clip = clips[index];
        sfx.Play();
    }
}