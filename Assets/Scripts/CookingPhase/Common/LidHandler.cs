using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(AudioSource))]

public class LidHandler : MonoBehaviour
{
    [SerializeField] private AudioClip lidOnSfx, lidOffSfx;

    public bool IsCoveringCookware { get; private set; }
    public UnityEvent OnCoverCookware = new UnityEvent();
    public UnityEvent OnTakeOffCookware = new UnityEvent();

    private RectTransform rectTransform;
    private BoxCollider2D boxCollider;
    private AudioSource audioSource;
    private Draggable draggable;
    private Cookware cookware;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        draggable = GetComponent<Draggable>();
    }

    private void OnEnable()
    {
        GetComponent<Image>().raycastTarget = true;
        draggable.OnDropItem.AddListener(CoverCookware);
    }

    private void CoverCookware(Draggable draggedObject)
    {
        if(IsCoveringCookware)
        {
            OnCoverCookware.Invoke();
            this.GetComponent<Image>().raycastTarget = false;
            rectTransform.position = cookware.LidPosition.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cookware = collision.GetComponent<Cookware>();

        IsCoveringCookware = cookware;
        audioSource.clip = lidOffSfx;
        audioSource.Play();
        Debug.Log("Lid - Covering cookware");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        cookware = null;
        IsCoveringCookware = false;
        OnTakeOffCookware.Invoke();
        audioSource.clip = lidOnSfx;
        audioSource.Play();

        Debug.Log("Lid - off");
    }
}
