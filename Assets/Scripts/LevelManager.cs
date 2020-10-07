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

    public void LoginScene()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    public void MainScene()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }

    public void CameraScene()
    {
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}