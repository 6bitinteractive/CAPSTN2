using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe Scene List", fileName = "Recipe00")]

public class Recipe : ScriptableObject
{
    [Tooltip("The scene that's loaded every time the player finishes a stage.")]
    public SceneData CookingOverview;

    [Tooltip("The stages related to the recipe, in proper order.")]
    public List<SceneData> Stages;

    [Tooltip("The scene to load after seeing the final, completed dish. Typically, a dialogue scene.")]
    public SceneData PostCookingScene;

    [Tooltip("Sprites of the completed dish (successful), in proper order.")]
    public List<Sprite> SuccessfulFinalDishSequence;

    [Tooltip("Sprites of the completed dish (failed), in proper order.")]
    public List<Sprite> FailedFinalDishSequence;

    public Sprite DishNameImage;
}
