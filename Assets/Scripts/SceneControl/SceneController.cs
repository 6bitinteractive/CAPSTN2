using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public delegate void SceneLoadEvent();
    public static event SceneLoadEvent BeforeSceneUnload, AfterSceneLoad;

    [Header("Data")]
    [SerializeField] private SceneData sceneDataToLoad;
    [SerializeField] private SaveData playerSaveData;

    public SaveData PlayerSaveData { get { return playerSaveData; } }

    [Header("Fading")]
    [SerializeField] private CanvasGroup faderCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    private bool isFading;

    private void Awake()
    {
        SingletonManager.Register<SceneController>(this);
    }

    private IEnumerator Start()
    {
        // Start off with a black screen
        faderCanvasGroup.alpha = 1f;

        // Write the initial starting position to playerSaveData so it can be loaded by the player when the first scene is loaded
        //playerSaveData.Save(Player.StartingPositionKey, sceneDataToLoad.StartingPointName);

        // Start the first scene loading and wait for it to finish
        //yield return StartCoroutine(LoadSceneAndSetActive(sceneDataToLoad.SceneName));

        // Once the scene is finished loading, start fading in
        //StartCoroutine(Fade(0f));

        yield return StartCoroutine(FadeAndSwitchScenes(sceneDataToLoad.SceneName));
    }


    public void FadeAndLoadScene(string sceneName)
    {
        // If a fade isn't happening then start fading and switching scenes.
        if (!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName));
        }
    }

    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        Debug.Log("Loading scene " + sceneName);

        // Don't load the scene again if it has been loaded in the editor
        if (Application.isEditor)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.isLoaded)
            {
                Debug.Log(sceneName + " is already loaded.");
                StartCoroutine(Fade(0f)); // Makes sure that the display isn't just black
                SceneManager.SetActiveScene(scene); // Set it as the active scene, the one to be unloaded next
                yield break;
            }
        }

        // Start fading to black and wait for it to finish before continuing
        yield return StartCoroutine(Fade(1f));

        // If this event has any subscribers, call it
        if (BeforeSceneUnload != null)
            BeforeSceneUnload();

        // Unload the current active scene
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // Start loading the given scene and wait for it to finish
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        // If this event has any subscribers, call it
        if (AfterSceneLoad != null)
            AfterSceneLoad();

        // Start fading back in and wait for it to finish before exiting the function
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        // Allow the given scene to load over several frames and add it to the already loaded scenes
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Find the scene that was most recently loaded (the one at the last index of the laoded scenes)
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        // Set the newly loaded scene as the active scene
        // This also marks it as the one to be unloaded next
        SceneManager.SetActiveScene(newlyLoadedScene);
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

        // Stop the CanvasGroup from blocking raycasts so input is no longer ignored
        //faderCanvasGroup.blocksRaycasts = false;
    }
}

// Reference: https://unity3d.com/learn/tutorials/projects/adventure-game-tutorial
