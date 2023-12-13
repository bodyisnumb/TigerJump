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
        loadingScreen.SetActive(true);

        yield return new WaitForSeconds(1);

        AsyncOperation operation = SceneManager.LoadSceneAsync("Game");

        while (!operation.isDone)
        {
            yield return null;
        }

        loadingScreen.SetActive(false);
    }

    IEnumerator LoadMenuSceneAsync()
    {
        loadingScreen.SetActive(true);

        yield return new WaitForSeconds(1);

        AsyncOperation operation = SceneManager.LoadSceneAsync("MainMenu");

        while (!operation.isDone)
        {
            yield return null;
        }

        loadingScreen.SetActive(false);
    }
}

