using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneData sceneDataToLoad;

    private static SceneController sceneController;
    private static SaveData playerSaveData;

    private void Start()
    {
        if (sceneController == null)
            sceneController = SingletonManager.GetInstance<SceneController>();

        if (playerSaveData == null)
            playerSaveData = sceneController.PlayerSaveData;
    }

    public void LoadScene()
    {
        sceneController.FadeAndLoadScene(sceneDataToLoad.SceneName);
    }
}
