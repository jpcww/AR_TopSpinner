using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : Singleton<SceneLoader>
{
    private string sceneNameToBeLoaded;

    public void LoadScene(string _sceneName)
    {
        sceneNameToBeLoaded = _sceneName;

        StartCoroutine(InitializeSceneLoading());                   // Review : 'StartCoroutine()' is required to use 'Couroutine' type methods
    }

    IEnumerator InitializeSceneLoading()                            // Review : 'IEnumertator' is put to create 'Coroutine' method
    {
        // First, load the loading scene
        yield return SceneManager.LoadSceneAsync("Scene_Loading");

        // Load the actual scene
        StartCoroutine(LoadActualScene());
    }

    IEnumerator LoadActualScene()
    {
        var asyncSceneLoading = SceneManager.LoadSceneAsync(sceneNameToBeLoaded);       // loading the actual scene process starts

        asyncSceneLoading.allowSceneActivation = false;            // it stops the scene from displaying when it is still loading, instead the loading screen 

        while (!asyncSceneLoading.isDone)
        {
            Debug.Log(asyncSceneLoading.progress);

            if (asyncSceneLoading.progress >=0.9f)                  // Chekcing the progress of loading the actualy scene is more than 90%
            {
                asyncSceneLoading.allowSceneActivation = true;      // then, show the actual scene
            }

            yield return null;
        }

    }


}
