using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookbookContentManager : MonoBehaviour
{
    [Header("UI - Regular")]
    [SerializeField] private GameObject regularNextButton;
    [SerializeField] private GameObject regularPreviousButton;

    [Header("UI - Grid")]
    [SerializeField] private GameObject gridNextButton;
    [SerializeField] private GameObject gridPreviousButton;

    private List<CookbookLayout> contents = new List<CookbookLayout>();
    private CookbookLayout currentPageLayout;
    private CookbookNoteData currentNote;
    private CookbookList currentList;
    private int currentPage;

    private void OnEnable()
    {
        SingletonManager.Register<CookbookContentManager>(this);
    }

    private void OnDisable()
    {
        SingletonManager.UnRegister<CookbookContentManager>();
    }

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
        regularNextButton.SetActive(false);
        regularPreviousButton.SetActive(false);
        gridNextButton.SetActive(false);
        gridPreviousButton.SetActive(false);
    }

    // Regular page types
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

    public void DisplayPage(CookbookPageData pageData)
    {
        SelectPageLayout(pageData.pageType);

        // UI
        gridNextButton.SetActive(false);
        gridPreviousButton.SetActive(false);
        regularNextButton.SetActive(currentPage < currentNote.pages.Count - 1);
        regularPreviousButton.SetActive(currentPage != 0);

        // Display content
        currentPageLayout.SetContent(pageData);
        currentPageLayout.Display(true);
    }

    // Grid-type
    public void DisplayNextList(bool nextPage = true)
    {
        currentPage += (nextPage ? +1 : -1);
        DisplayList(currentList, currentPage);
    }

    public void DisplayList(CookbookList list, int page = 0)
    {
        currentList = list;
        SelectPageLayout(list.pageType);

        // UI
        regularNextButton.SetActive(false);
        regularPreviousButton.SetActive(false);

        // There's a next page if currentPage is less than the currentLayout's max number of pages
        gridNextButton.SetActive(currentPage < Mathf.CeilToInt((float)list.notes.Count / (float)currentPageLayout.MaxCountPerPage));
        gridPreviousButton.SetActive(currentPage != 0);

        currentPageLayout.SetContent(list, page);
        currentPageLayout.Display(true);
    }

    private void SelectPageLayout(CookbookPageType pageType)
    {
        // If not the same page type as next page
        if (currentPageLayout.PageType != pageType)
        {
            // Hide current page layout
            currentPageLayout.Display(false);

            // Find and use correct layout
            currentPageLayout = contents.Find((x) => x.PageType == pageType);
        }
    }
}
