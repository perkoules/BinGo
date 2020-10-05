using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoginScene()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    public void MainGame()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }
}