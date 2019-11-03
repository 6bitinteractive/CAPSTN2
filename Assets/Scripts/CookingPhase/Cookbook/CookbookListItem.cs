using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]

public class CookbookListItem : MonoBehaviour
{
    public CookbookNoteData Note { get; set; }

    public bool Locked
    {
        get { return _locked; }
        set
        {
            _locked = value;
            button.interactable = _locked;
        }
    }

    public TextMeshProUGUI displayNameText;
    private static CookbookContentManager cookbookContentManager;
    private Button button;
    private bool _locked;

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
        cookbookContentManager.DisplayNote(Note);
    }
}
