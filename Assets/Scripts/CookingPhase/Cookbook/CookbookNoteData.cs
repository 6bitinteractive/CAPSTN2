using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CookbookNote", menuName = "Cookbook Note")]
public class CookbookNoteData : ScriptableObject
{
    public string displayName;
    public List<CookbookPageData> pages;
}

[System.Serializable]
public class CookbookPageData
{
    public CookbookPageType pageType;
    public string title;

    [Tooltip("If two columns - 0: Left, 1: Right")]
    [TextArea(3, 6)]
    public List<string> description;

    [Tooltip("Set a different font size, if necessary. Default is 36.")]
    public float descriptionTextFontSize = 36;

    public Sprite image;
}

public enum CookbookPageType
{
    Title100Description100,
    Title100ImageLeft30Description70,
    Title100Description50Description50,
    ImageLeft30FullHeightTitle70Description70,
    ImageLeft50FullHeightTitle50Description50,
    Title100ListGrid,
}
