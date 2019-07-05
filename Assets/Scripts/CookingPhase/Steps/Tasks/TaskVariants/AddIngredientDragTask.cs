using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddIngredientDragTask : Task
{
    [SerializeField] private List<GameObject> ingredientsRequiredInCookware;

    protected override void Setup()
    {
        if (ingredientsRequiredInCookware.Count == 0)
            Debug.LogError(gameObject.name + ": No ingredient(s) in required list.");
    }

    protected override bool SuccessConditionMet()
    {
        // If one ingredient hasn't been enabled (ie. not in the cookware), the action is considered a failure
        return !ingredientsRequiredInCookware.Exists(x => !x.gameObject.activeInHierarchy);
    }
}
