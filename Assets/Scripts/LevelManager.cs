using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
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
}