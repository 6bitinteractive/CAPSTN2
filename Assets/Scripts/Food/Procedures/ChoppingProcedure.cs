using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Food/Chopping Procedure", fileName = "ChoppingProcedure")]
public class ChoppingProcedure : Procedure
{
    public Sprite[] ChoppedIngredientSequence;
    public int SliceCount = 1;

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
            if (prepStation.InputHandler.SwipeInput.ContainsKey(SwipeDirection.Down))
            {
                SliceCount++;
                prepStation.ChoppingIngredient.GetComponent<Image>().sprite = ChoppedIngredientSequence[SliceCount];
                prepStation.InputHandler.SwipeInput.Clear();
            }

            yield return null;

            if (SliceCount >= ChoppedIngredientSequence.Length - 1)
            {
                Debug.Log("Procedure done.");
                // Make sure to reset this scriptable object, clear Input then disable it
                Reset();
                prepStation.InputHandler.SwipeInput.Clear();
                prepStation.InputHandler.SwipeDetector.enabled = false;

                yield return new WaitForSeconds(3f);
                OnProcedureDone.Invoke();
            }
        }
    }

    public override void Reset()
    {
        SliceCount = 0;
    }
}
