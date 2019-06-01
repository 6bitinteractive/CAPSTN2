using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneData sceneDataToLoad;

    private static SceneController sceneController;

    private void Start()
    {
        if (sceneController == null)
            sceneController = SingletonManager.GetInstance<SceneController>();
    }

    public void LoadScene()
    {
        sceneController.FadeAndLoadScene(sceneDataToLoad.SceneName);
    }
}
