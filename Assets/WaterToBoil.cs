using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class WaterToBoil : MonoBehaviour
{
    public AudioSource sfx;
    public AudioClip[] waterSFX;
    // Start is called before the first frame update
    void Start()
    {
        sfx.clip = waterSFX[0];
        sfx.Play();
        Invoke("PlayBoilSFX", sfx.clip.length);
    }

    void PlayBoilSFX()
    {
        sfx.clip = waterSFX[1];
        sfx.Play();
    }

}
