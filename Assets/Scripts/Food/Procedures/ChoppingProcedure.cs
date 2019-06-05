using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Food/Chopping Procedure", fileName = "ChoppingProcedure")]
public class ChoppingProcedure : Procedure
{
    public Sprite[] ChoppedIngredientSequence;
    public int CurrentSlice;

    public override IEnumerator Apply(PrepStation prepStation)
    {
        Debug.Log("Chopping... " + prepStation.BaseRecipe.DisplayName);

        prepStation.ChoppingPanel.gameObject.SetActive(true);

        while (CurrentSlice < ChoppedIngredientSequence.Length)
        {
            if (prepStation.InputHandler.test)
            {
                CurrentSlice++;
                prepStation.ChoppingIngredient.GetComponent<Image>().sprite = ChoppedIngredientSequence[CurrentSlice];
                prepStation.InputHandler.test = false;
            }

            yield return null;
        }
    }

    public override void Reset()
    {
        CurrentSlice = 0;
    }
}
