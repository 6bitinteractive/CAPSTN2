using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientDetector : MonoBehaviour
{
    [HideInInspector] public bool isCollidingWithIngredient;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCollidingWithIngredient = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isCollidingWithIngredient = false;
    }
}
