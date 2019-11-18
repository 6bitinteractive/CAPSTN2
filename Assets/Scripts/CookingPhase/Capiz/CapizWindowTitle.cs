using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapizWindow))]

public class CapizWindowTitle : MonoBehaviour
{
    [SerializeField] private float delayOpen = 2f;
    [SerializeField] private Animator act;
    [SerializeField] private Animator title;
    [SerializeField] private UnityEvent OnCapizEnd;

    private CanvasGroup canvasGroup;
    private CapizWindow capizWindow;
    private float delayAfterFadeOut;

    private void Awake()
    {
        capizWindow = GetComponent<CapizWindow>();
        delayAfterFadeOut = delayOpen - 2.5f;
    }

    private void Start()
    {
        StartCoroutine(OpenScene());
    }

    private IEnumerator OpenScene()
    {
        yield return new WaitForSeconds(delayOpen);
        capizWindow.Display(false);
        act.SetTrigger("FadeOut");
        title.SetTrigger("FadeOut");
        yield return new WaitForSeconds(delayAfterFadeOut);
        OnCapizEnd.Invoke();
    }
}
