using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEnterScene;
    [SerializeField] private UnityEvent OnFadeScene;
    [SerializeField] private CanvasGroup faderCanvasGroup;
    private bool isFading;
    [SerializeField] private float fadeDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        OnEnterScene.Invoke();
    }

    public void FadeCurrentScene()
    {
        // If a fade isn't happening then start fading and switching scenes.
        if (!isFading)
        {
            StartCoroutine(FadeScene());
        }
    }

    private IEnumerator FadeScene()
    {
        StartCoroutine(Fade(0f)); // Makes sure that the display isn't just black
   
        // Start fading to black and wait for it to finish before continuing
        yield return StartCoroutine(Fade(1f));

        OnFadeScene.Invoke();

        // Start fading back in and wait for it to finish before exiting the function
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float finalAlpha)
    {
        // Set the fading flag to true so the FadeAndSwitchScenes coroutine won't be called again
        isFading = true;

        // Make sure the CanvasGroup blocks raycasts into the scene so no more input can be accepted
        faderCanvasGroup.blocksRaycasts = true;

        // Calculate how fast the CanvasGroup should fade based on its current alpha, its final alpha, and how long it has to change between the two
        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

        // While the CanvasGroup hasn't reached the final alpha yet...
        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            // ... move the alpha towards its target alpha
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

            // Enable input sooner (so that UX doesn't feel like there's a delay in reading the input)
            if (faderCanvasGroup.alpha <= 0.7f)
                faderCanvasGroup.blocksRaycasts = false;

            // Wait for a frame then continue
           
            yield return null;
        }

        // Set the flag to false since the fade has finished
        isFading = false;

    }
}