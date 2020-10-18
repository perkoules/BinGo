using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Camera mapCamera;
    public Camera arCamera;
    public Camera rubbishCamera;
    public GameObject mapImage, arImage, arSession;
    public Button aR_MapCameraSelection, rubbishCameraButton;


    private void Start()
    {
        aR_MapCameraSelection.onClick.AddListener(ChangeCameraMode);
        rubbishCameraButton.onClick.AddListener(OpenRubbishCamera);
    }

    private void OpenRubbishCamera()
    {
        arImage.SetActive(false);
        mapImage.SetActive(false);
        ChangeCameraMode();
    }

    private void ChangeCameraMode()
    {
        if (mapImage.activeSelf)
        {
            arImage.SetActive(true);
            mapImage.SetActive(false);
            mapCamera.enabled = true;
            arCamera.enabled = false;

            arSession.SetActive(false);
            rubbishCamera.enabled = false;
            rubbishCamera.GetComponent<SimpleDemo>().enabled = false;
        }
        else if (arImage.activeSelf)           //AR Image is active so disable it and open AR Camera
        {
            arImage.SetActive(false);          //Disable AR Image
            mapImage.SetActive(true);          //Enable Map Image
            arCamera.enabled = true;           //AR Camera enabled
            mapCamera.enabled = false;         //Map camera disabled

            arSession.SetActive(true);
            rubbishCamera.enabled = false;
            rubbishCamera.GetComponent<SimpleDemo>().enabled = false;
        }
        else if (!mapImage.activeSelf && !arImage.activeSelf)
        {
            arCamera.enabled = false;         //AR Canera enabled
            mapCamera.enabled = false;         //Map camera disabled

            arSession.SetActive(false);
            rubbishCamera.enabled = true;
            rubbishCamera.GetComponent<SimpleDemo>().enabled = true;
            rubbishCamera.GetComponent<SimpleDemo>().ClickStart();
        }
    }

    public void CameraResetter()
    {
        mapImage.SetActive(true);
        arImage.SetActive(false);
        ChangeCameraMode();
    }
}