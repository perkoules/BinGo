using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadLevelWithIndex(int levelIndex)
    {
        SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Single);
    }
}
