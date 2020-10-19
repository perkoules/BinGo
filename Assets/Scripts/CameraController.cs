using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Camera mapCamera;
    public Camera arCamera;
    public GameObject mapImage, arImage, arSession;
    public Button aR_MapCameraSelection;

    private void Start()
    {
        aR_MapCameraSelection.onClick.AddListener(ChangeCameraMode);
    }

    private void ChangeCameraMode()
    {
        if (mapImage.activeSelf)
        {
            arImage.SetActive(true);
            mapImage.SetActive(false);
            mapCamera.enabled = true;
            arCamera.enabled = false;
        }
        else if (arImage.activeSelf)           //AR Image is active so disable it and open AR Camera
        {
            arImage.SetActive(false);          //Disable AR Image
            mapImage.SetActive(true);          //Enable Map Image
            mapCamera.enabled = false;         //Map camera disabled
            StartCoroutine(CheckIfEnabled(arSession)); //AR Camera enabled
        }
    }

    public void CameraResetter()
    {
        mapImage.SetActive(true);
        arImage.SetActive(false);
        arSession.SetActive(true);
        ChangeCameraMode();
    }

    private IEnumerator CheckIfEnabled(GameObject gameObject)
    {
        if (!gameObject.activeSelf)
        {
            Debug.Log("Waiting for AR Session to be enabled...");
            yield return new WaitUntil(() => gameObject.activeSelf);
        }
        Debug.Log("AR Session ENABLED");
        arCamera.enabled = true;           
    }
}