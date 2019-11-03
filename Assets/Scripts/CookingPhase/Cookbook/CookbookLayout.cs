using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasDisplayManager))]
public class CookbookLayout : MonoBehaviour
{
    [SerializeField] private CookbookPageType pageType;
    public CookbookPageType PageType => pageType;

    [SerializeField] private TextMeshProUGUI title;

    [Header("Optional")]
    [SerializeField] private List<TextMeshProUGUI> description;

    [Tooltip("Leave empty if there's no need to display an image.")]
    [SerializeField] private Image image;

    [Header("For Grid-type")]
    [SerializeField] private GameObject gridContainer;
    private List<CookbookListItem> gridItems = new List<CookbookListItem>();
    public int MaxCountPerPage { get { return gridItems.Count; } }

    private CanvasDisplayManager canvasDisplay;

    private void Awake()
    {
        if (gridContainer != null)
            gridItems.AddRange(gridContainer.GetComponentsInChildren<CookbookListItem>());
    }

    private void OnEnable()
    {
        canvasDisplay = GetComponent<CanvasDisplayManager>();
    }

    public void SetContent(CookbookPageData pageData)
    {
        ClearContent();

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

    public void SetContent(CookbookList list, int page = 0)
    {
        int startIndex = gridItems.Count * page;
        int lastIndex = (gridItems.Count * (page + 1)) - 1;
        int maxNumberOfPages = Mathf.CeilToInt(list.notes.Count / (float)MaxCountPerPage) - 1; // page starts with 0
        int lastItemIndexForPage = page == maxNumberOfPages ? gridItems.Count - 1 - (lastIndex - list.notes.Count) : MaxCountPerPage;

        EnableButtons(lastItemIndexForPage);
        ClearContent();

        // Set the content of the list
        for (int i = startIndex; i < lastItemIndexForPage; i++)
        {
            foreach (var item in gridItems)
            {
                item.Note = list.notes[i];
                item.displayNameText.text = list.notes[i].displayName;
            }
        }
    }

    public void Display(bool display = true)
    {
        canvasDisplay.Display(display);
    }

    private void ClearContent()
    {
        // Clear previous content
        title.text = string.Empty;

        if (image != null)
            image.sprite = null;

        foreach (var item in description)
            item.text = string.Empty;
    }

    private void EnableButtons(int lastIndex)
    {
        for (int i = 0; i < gridItems.Count; i++)
        {
            gridItems[i].gameObject.SetActive(i < lastIndex);
        }
    }
}
