using UnityEngine;
using UnityEngine.UI;

public class CameraChanger : MonoBehaviour
{
    public Camera mapCamera;
    public Camera mainCamera;

    public GameObject mapImage, arImage;

    private Button cameraChangerButton;

    private void Start()
    {
        cameraChangerButton = GetComponent<Button>();
        cameraChangerButton.onClick.AddListener(ChangeFocusedCamera);
    }

    private void ChangeFocusedCamera()
    {
        if (mapImage.activeSelf)
        {
            arImage.SetActive(true);
            mapImage.SetActive(false);
            mainCamera.enabled = false;
            mapCamera.enabled = true;
        }
        else if (arImage.activeSelf)           //AR Image is active so disable it and open AR Camera
        {
            arImage.SetActive(false);          //Disable AR Image
            mapImage.SetActive(true);          //Enable Map Image
            mainCamera.enabled = true;         //AR Canera enabled
            mapCamera.enabled = false;         //Map camera disabled
        }
    }
}