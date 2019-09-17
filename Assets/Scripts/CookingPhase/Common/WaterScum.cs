using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterScum : MonoBehaviour
{
    [SerializeField] private float delayAppearance = 2f;
    private List<Image> waterScum = new List<Image>();
    private int currentIndex;

    // FIX?: better variable name D:
    public bool Displayed { get; private set; }
    public bool Removed { get; private set; }

    private void Awake()
    {
        waterScum.AddRange(GetComponentsInChildren<Image>());

        foreach (var item in waterScum)
            item.gameObject.SetActive(false);
    }

    public IEnumerator Display()
    {
        while (currentIndex < waterScum.Count)
        {
            Debug.Log("Display: " + currentIndex);
            yield return new WaitForSeconds(delayAppearance);
            Debug.Log("Showing scum...");
            waterScum[currentIndex].gameObject.SetActive(true);
            currentIndex++; // Note: This would mean the index will end up +1 from what was shown...
        }

        Displayed = currentIndex == waterScum.Count;
    }

    public void Remove()
    {
        currentIndex--; // Note (cont'd):...so we subtract the index by 1 first

        waterScum[currentIndex].gameObject.SetActive(false);
        Debug.Log("Removed " + waterScum[currentIndex].gameObject);

        if (currentIndex <= 0)
        {
            Debug.Log("Removed all scum");
            Removed = true;
            return;
        }
    }
}
