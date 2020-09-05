using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //public Camera cameraMap, cameraAR;

    /*private void Awake()
    {
        cameraAR.enabled = false;
        cameraAR.gameObject.SetActive(false);
        cameraMap.enabled = true;
    }
    public void SwitchCameraMode()
    {
        if (cameraMap.enabled)
        {
            cameraMap.gameObject.SetActive(false);
            cameraMap.enabled = false;
            cameraAR.gameObject.SetActive(true);
            cameraAR.enabled = true;
        }
        else if (cameraAR.enabled)
        {
            cameraAR.gameObject.SetActive(false);
            cameraAR.enabled = false;
            cameraMap.gameObject.SetActive(true);
            cameraMap.enabled = true;
        }
    }*/
    public void LoadLevelWithIndex(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex, LoadSceneMode.Single);
    }
}
