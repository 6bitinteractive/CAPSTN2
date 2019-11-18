using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KitchenUtensil))]
[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(AudioSource))]

public class SpoonKitchenUtensil : MonoBehaviour
{
    public bool IsMixing { get; private set; }
    public float MixDuration { get; set; }

    private KitchenUtensil kitchenUtensil;
    private AudioSource audioSource;

    private void Awake()
    {
        kitchenUtensil = GetComponent<KitchenUtensil>();
        audioSource = GetComponent<AudioSource>();
        transform.hasChanged = false;
    }

    private void OnEnable()
    {
        MixDuration = 0f;
    }

    private void Update()
    {
        if (!kitchenUtensil.InCookware) { return; }

        if (transform.hasChanged)
        {
            IsMixing = true;
            MixDuration += Time.deltaTime;
            transform.hasChanged = false; // Must always be set back to false

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            //Debug.Log(gameObject.name + " is mixing.");
            //Debug.Log(MixDuration);
        }
        else
        {
            IsMixing = false;
            audioSource.Stop();
        }
    }
}
