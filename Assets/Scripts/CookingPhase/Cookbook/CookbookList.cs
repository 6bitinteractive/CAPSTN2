using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class CookbookList : MonoBehaviour
{
    public CookbookPageType pageType;
    public List<CookbookNoteData> notes;

    private static CookbookContentManager cookbookContentManager;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(DisplayItem);
    }

    private void Start()
    {
        cookbookContentManager = SingletonManager.GetInstance<CookbookContentManager>();
    }

    private void DisplayItem()
    {
        cookbookContentManager.DisplayList(this);
    }
}
