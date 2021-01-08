using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance { get; set; }

    private void OnEnable()
    {
        if(Instance !=null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public AsyncOperation asyncOperation;
    public void LoadSceneAsyncAdditive(string name)
    {
        StartCoroutine(AsyncLoading(name));
    }
    IEnumerator AsyncLoading(string name)
    {
        asyncOperation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        yield return new WaitUntil(() => asyncOperation.isDone && PlayfabManager.Instance.dataLoaded);
        Destroy(GameObject.FindGameObjectWithTag("TempCanvas"));
    }
    public void UnloadSceneAsync(string name)
    {
        SceneManager.UnloadSceneAsync(name);
    }
    public void LoadSceneAsyncByName(string name)
    {
        SceneManager.LoadSceneAsync(name);
    }
    /// <summary>
    /// Triggered by Button
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Closing...");
        StartCoroutine(ClosingDelay());        
    }

    private IEnumerator ClosingDelay()
    {
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
}