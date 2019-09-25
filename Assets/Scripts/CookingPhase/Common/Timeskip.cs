using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Animator))]

public class Timeskip : MonoBehaviour, IPointerDownHandler
{
    private Image image;
    private Animator animator;

    public UnityEvent OnClick = new UnityEvent();

    private void Awake()
    {
        SingletonManager.Register<Timeskip>(this);
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        image.raycastTarget = false;
    }

    private void OnDestroy()
    {
        SingletonManager.UnRegister<Timeskip>();
    }

    public void Show(Sprite sprite)
    {
        image.sprite = sprite;
        image.raycastTarget = true;
        animator.SetTrigger("Show");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        image.raycastTarget = false;
        animator.SetTrigger("Hide");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        OnClick.Invoke();
    }
}
