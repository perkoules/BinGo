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

    public void LoadSceneAdditive(string name)
    {
        SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
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