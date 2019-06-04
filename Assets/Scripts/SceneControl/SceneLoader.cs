using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    private static SceneController sceneController;

    private void Start()
    {
        if (sceneController == null)
            sceneController = SingletonManager.GetInstance<SceneController>();
    }

    public void LoadScene(SceneData sceneDataToLoad)
    {
        sceneController.FadeAndLoadScene(sceneDataToLoad.SceneName);
    }
}
