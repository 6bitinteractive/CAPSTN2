using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapizWindow))]

public class CapizWindowTitle : MonoBehaviour
{
    [SerializeField] private float delayOpen = 2f;
    [SerializeField] private Animator act;
    [SerializeField] private Animator title;

    private CanvasGroup canvasGroup;
    private CapizWindow capizWindow;

    private void Awake()
    {
        capizWindow = GetComponent<CapizWindow>();
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
    }
}
