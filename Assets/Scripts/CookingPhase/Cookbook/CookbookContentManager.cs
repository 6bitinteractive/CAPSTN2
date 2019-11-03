using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookbookContentManager : MonoBehaviour
{
    private List<CookbookLayout> contents = new List<CookbookLayout>();
    private CookbookLayout currentPage;
    private CookbookNoteData currentNote;

    private void Awake()
    {
        // Get all Cookbook content types attached to this object
        contents.AddRange(GetComponentsInChildren<CookbookLayout>());

        // Hide all contents
        foreach (var item in contents)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void DisplayNote(CookbookNoteData note)
    {
        currentNote = note;
    }

    private void SelectPageLayout(CookbookPageData pageData)
    {
        // Hide current page layout if not the same page type as next page
        if (currentPage.PageType != pageData.pageType)
        {
            currentPage.gameObject.SetActive(false);

            // Find and use correct layout
            currentPage = contents.Find((x) => x.PageType == pageData.pageType);
        }
    }
}
