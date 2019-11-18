using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public class WaterStateController : MonoBehaviour
{
    [SerializeField] private WaterSfx sfx;

    private Animator animator;
    private AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SwitchState(WaterState waterState)
    {
        if (animator == null) { return; }

        // Switch animation
        animator.SetInteger("State", (int)waterState);

        // Play sfx if applicable
        if (waterState == WaterState.Pouring || (int)waterState >= 10)
        {
            audioSource.loop = false;
            audioSource.clip = sfx.PouringSfx;
        }
        else if (waterState == WaterState.Simmering)
        {
            audioSource.loop = true;
            audioSource.clip = sfx.SimmeringSfx;
        }
        else
        {
            audioSource.clip = null;
        }

        if (audioSource.clip != null && !audioSource.isPlaying)
            audioSource.Play();
    }

    // NOTE: fuction overload since Unity doesn't support enum parameters at UnityEvent inspector :(
    public void SwitchState(int state)
    {
        SwitchState((WaterState)state);
    }

    public enum WaterState
    {
        None, Pouring, Still, Simmering,
        Pouring1 = 10, Pouring2 = 11, Pouring3 = 12, Pouring4 = 13, Pouring5 = 14, Pouring6 = 15 // Note: Temporary?
    }

}

[System.Serializable]
public class WaterSfx
{
    public AudioClip PouringSfx, SimmeringSfx;
}
