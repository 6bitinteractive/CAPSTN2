using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookbookContentManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject previousButton;

    private List<CookbookLayout> contents = new List<CookbookLayout>();
    private CookbookLayout currentPageLayout;
    private CookbookNoteData currentNote;
    private int currentPage;

    private void Start()
    {
        // Get all Cookbook content types attached to this object
        contents.AddRange(GetComponentsInChildren<CookbookLayout>());

        // Set a default layout
        currentPageLayout = contents[0];

        // Hide all contents
        foreach (var item in contents)
            item.Display(false);

        // Hide page buttons
        nextButton.SetActive(false);
        previousButton.SetActive(false);
    }

    public void DisplayNote(CookbookNoteData note)
    {
        currentNote = note;
        currentPage = 0;
        DisplayPage(currentNote.pages[0]);
    }

    // nextPage = false: display previous
    public void DisplayNextPage(bool nextPage = true)
    {
        currentPage += (nextPage ? +1 : -1);
        //currentPage %= currentNote.pages.Count; // Allow loop back to first page
        DisplayPage(currentNote.pages[currentPage]);
    }

    private void DisplayPage(CookbookPageData pageData)
    {
        // UI
        previousButton.SetActive(currentPage != 0);
        nextButton.SetActive(currentPage < currentNote.pages.Count - 1);

        // If not the same page type as next page
        if (currentPageLayout.PageType != pageData.pageType)
        {
            // Hide current page layout
            currentPageLayout.Display(false);

            // Find and use correct layout
            currentPageLayout = contents.Find((x) => x.PageType == pageData.pageType);
        }

        // Display content
        currentPageLayout.SetContent(pageData);
        currentPageLayout.Display(true);
    }
}
