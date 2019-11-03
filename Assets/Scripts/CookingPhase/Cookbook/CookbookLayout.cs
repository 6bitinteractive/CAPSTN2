using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookbookLayout : MonoBehaviour
{
    [SerializeField] private CookbookPageType pageType;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private List<TextMeshProUGUI> description;

    [Tooltip("Leave empty if there's no need to display an image.")]
    [SerializeField] private Image image;

    public CookbookPageType PageType => pageType;

    public void SetContent(CookbookPageData pageData)
    {
        // Clear previous content
        title.text = string.Empty;
        image.sprite = null;
        foreach (var item in description)
            item.text = string.Empty;

        // Set new content
        title.text = pageData.title;
        for (int i = 0; i < pageData.description.Count; i++)
        {
            // Since current available layouts only support two columns, produce an error
            if (pageData.description.Count >= 2)
            {
                Debug.LogError("Current layout styles only support two description columns; Either limit descriptions to two or create new layout type.");
                return;
            }

            description[i].text = pageData.description[i];
        }

        if (image != null && pageData.image != null)
            image.sprite = pageData.image;
    }
}
