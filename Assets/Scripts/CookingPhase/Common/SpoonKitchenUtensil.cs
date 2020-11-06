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
    private Vector3 initialPos;

    private IngredientDetector ingredientDetector;

    private void Awake()
    {
        kitchenUtensil = GetComponent<KitchenUtensil>();
        audioSource = GetComponent<AudioSource>();
        transform.hasChanged = false;
        ingredientDetector = GetComponentInChildren<IngredientDetector>();
    }

    private void OnEnable()
    {
        MixDuration = 0f;
        initialPos = this.transform.position;
    }

    private void Update()
    {
        if (!kitchenUtensil.InCookware) { return; }

        if (transform.hasChanged && ingredientDetector.isCollidingWithIngredient)
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

    public void Reset()
    {
        this.transform.position = initialPos;
    }
}
