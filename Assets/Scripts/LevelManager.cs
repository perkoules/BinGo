using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void SwitchCameraMode(Camera AR)
    {

    }
    public void LoadLevelWithIndex(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex, LoadSceneMode.Single);
    }
}
