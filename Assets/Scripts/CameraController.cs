using System;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Camera mapCamera;
    public Camera arCamera;
    public Camera rubbihsCamera;

    public GameObject mapImage, arImage, rubbishCameraPlane;

    public Button aR_MapCameraSelection, rubbishCameraButton;

    private void Start()
    {
        aR_MapCameraSelection.onClick.AddListener(ChangeFocusedCamera);
        rubbishCameraButton.onClick.AddListener(OpenRubbishCamera);
    }

    private void OpenRubbishCamera()
    {
        arImage.SetActive(false);
        mapImage.SetActive(false);
        ChangeFocusedCamera();
    }

    private void ChangeFocusedCamera()
    {
        if (mapImage.activeSelf)
        {
            arImage.SetActive(true);
            mapImage.SetActive(false);
            arCamera.enabled = false;
            mapCamera.enabled = true;
            rubbihsCamera.enabled = false;
            rubbishCameraPlane.SetActive(false);
        }
        else if (arImage.activeSelf)           //AR Image is active so disable it and open AR Camera
        {
            arImage.SetActive(false);          //Disable AR Image
            mapImage.SetActive(true);          //Enable Map Image
            arCamera.enabled = true;         //AR Canera enabled
            mapCamera.enabled = false;         //Map camera disabled
            rubbihsCamera.enabled = false;
            rubbishCameraPlane.SetActive(false);
        }
        else if (!mapImage.activeSelf && !arImage.activeSelf)
        {
            arCamera.enabled = true;         //AR Canera enabled
            mapCamera.enabled = false;         //Map camera disabled
            rubbihsCamera.enabled = true;
            rubbishCameraPlane.SetActive(true);
        }
    }


    public void CameraResetter()
    {
        mapImage.SetActive(true);
        arImage.SetActive(false);
        ChangeFocusedCamera();
    }

    public void RubbishCamera()
    {
        mapImage.SetActive(false);
        arImage.SetActive(false);
        ChangeFocusedCamera();
    }
}