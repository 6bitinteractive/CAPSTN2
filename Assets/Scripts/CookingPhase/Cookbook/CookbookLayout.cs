using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasDisplayManager))]
public class CookbookLayout : MonoBehaviour
{
    [SerializeField] private CookbookPageType pageType;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private List<TextMeshProUGUI> description;

    [Tooltip("Leave empty if there's no need to display an image.")]
    [SerializeField] private Image image;

    public CookbookPageType PageType => pageType;

    private CanvasDisplayManager canvasDisplay;

    private void OnEnable()
    {
        canvasDisplay = GetComponent<CanvasDisplayManager>();
    }

    public void SetContent(CookbookPageData pageData)
    {
        // Clear previous content
        title.text = string.Empty;

        if (image != null)
            image.sprite = null;

        foreach (var item in description)
            item.text = string.Empty;

        // Set new content
        title.text = pageData.title;

        if (pageData.description.Count > 0)
        {
            for (int i = 0; i < description.Count; i++)
            {
                // Since current available layouts only support two columns, produce an error
                if (pageData.description.Count > 2)
                {
                    Debug.LogError("Current layout styles only support two description columns; Either limit descriptions to two or create new layout type.");
                    return;
                }

                description[i].fontSize = pageData.descriptionTextFontSize;
                description[i].text = pageData.description[i];
            }
        }

        if (image != null && pageData.image != null)
            image.sprite = pageData.image;
    }

    public void Display(bool display = true)
    {
        canvasDisplay.Display(display);
    }
}
