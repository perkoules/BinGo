using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Level Manager");
                go.AddComponent<LevelManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void LoadSceneByBuildIndex(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }

    public void LoadSceneByName(string name)
    {
        SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Debug.Log("Closing...");
        Application.Quit();
    }
}