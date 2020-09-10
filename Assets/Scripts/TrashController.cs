using Mapbox.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : MonoBehaviour
{
    public Camera cameraMap, cameraAR;
    public GameObject mapMainPanel, cameraPanel;
    public SpawnOnMap spawn;

    private void OnEnable()
    {
        if (cameraMap.isActiveAndEnabled)
        {
            cameraMap.gameObject.SetActive(false);
            mapMainPanel.SetActive(false);
            cameraAR.gameObject.SetActive(true);
            cameraPanel.SetActive(true);
        }
        SpawnPins();
    }

    private void OnDisable()
    {
        cameraAR.gameObject.SetActive(false);
        cameraPanel.SetActive(false);
        cameraMap.gameObject.SetActive(true);
        mapMainPanel.SetActive(true);
    }


    public void SpawnPins()
    {
        //
    }
}
