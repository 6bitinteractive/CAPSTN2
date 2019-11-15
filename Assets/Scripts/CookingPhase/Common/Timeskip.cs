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
    [SerializeField] private GameObject tapPrompt;
    [SerializeField] private float delay = 1.5f;
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
        animator.SetTrigger("Show");
        StartCoroutine(DelayInteraction());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(Hide());
    }

    private IEnumerator DelayInteraction()
    {
        yield return new WaitForSeconds(delay);
        image.raycastTarget = true;
        tapPrompt.SetActive(true);
    }

    private IEnumerator Hide()
    {
        image.raycastTarget = false;
        tapPrompt.SetActive(false);
        animator.SetTrigger("Hide");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        OnClick.Invoke();
    }
}
