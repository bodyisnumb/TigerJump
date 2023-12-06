using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;

    public void LoadGameScene()
    {
        StartCoroutine(LoadGameSceneAsync());
    }

    public void LoadMenuScene()
    {
        StartCoroutine(LoadMenuSceneAsync());
    }

    IEnumerator LoadGameSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Game");
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            yield return null;
        }

        loadingScreen.SetActive(false);
    }

    IEnumerator LoadMenuSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("MainMenu");
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            yield return null;
        }

        loadingScreen.SetActive(false);
    }
}

