using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Food/Chopping Procedure", fileName = "ChoppingProcedure")]
public class ChoppingProcedure : Procedure
{
    public Sprite[] ChoppedIngredientSequence;
    public int SliceCount;

    public override IEnumerator Apply(PrepStation prepStation)
    {
        Debug.Log("Chopping... " + prepStation.BaseRecipe.DisplayName);

        // Show the chopping station
        prepStation.ChoppingPanel.gameObject.SetActive(true);

        // Enable swipe detection
        prepStation.InputHandler.SwipeDetector.enabled = true;

        // Slice the ingredient whenever player swipes down
        while (SliceCount < ChoppedIngredientSequence.Length)
        {
            // If the player swiped down
            if (prepStation.InputHandler.SwipeInput.ContainsKey(SwipeDirection.Down)
                || prepStation.InputHandler.SwipeInput.ContainsKey(SwipeDirection.LeftDown)
                || prepStation.InputHandler.SwipeInput.ContainsKey(SwipeDirection.RightDown))
            {
                // Show the ingredient as sliced
                SliceCount++;
                prepStation.ChoppingIngredient.GetComponent<Image>().sprite = ChoppedIngredientSequence[SliceCount];

                // Clear the swipe input container
                prepStation.InputHandler.SwipeInput.Clear();
            }

            // TODO: Sync knife animation with player input

            yield return null;

            // If we've fully sliced the ingredient
            if (SliceCount >= ChoppedIngredientSequence.Length - 1)
            {
                Debug.Log("Procedure done.");
                // Make sure to reset this scriptable object
                Reset();

                // Clear Input then disable it so that no swipe detection will be done
                prepStation.InputHandler.SwipeInput.Clear();
                prepStation.InputHandler.SwipeDetector.enabled = false;

                OnProcedureSuccess.Invoke();
                Done = true;
            }
        }
    }

    public override void Reset()
    {
        SliceCount = 0;
        Done = false;
    }
}
