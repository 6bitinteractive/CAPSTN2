using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddIngredientDragTask : Task
{
    [SerializeField] private List<GameObject> ingredientsInCookware;

    protected override bool SuccessConditionMet()
    {
        // If one ingredient hasn't been enabled (ie. not in the cookware), the action is considered a failure
        return !ingredientsInCookware.Exists(x => !x.gameObject.activeInHierarchy);
    }
}
