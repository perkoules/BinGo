using UnityEngine;
using UnityEngine.UI;


public class Flashlight : MonoBehaviour
{
    public GameObject flashOnImage, flashOffImage;
    private Toggle flashlightButton;
    private AndroidJavaObject cam = null;
    public Button button;

    private void Awake()
    {
        flashlightButton = GetComponent<Toggle>();
        flashlightButton.onValueChanged.AddListener(ToggleFlashlight);
    }

    private void ToggleFlashlight(bool isTorchOn)
    {
        if (isTorchOn)
        {
            FlashOn();
        }
        else
        {
            FlashOff();
        }
    }

    private void FlashOn()
    {
        Debug.LogWarning("Flash ON");

        if (cam == null)
        {
            AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera");
            cam = cameraClass.CallStatic<AndroidJavaObject>("open");
        }
        if (cam != null)
        {
            AndroidJavaObject camParameters = cam.Call<AndroidJavaObject>("getParameters");
            camParameters.Call("setFlashMode", "torch");
            cam.Call("setParameters", camParameters);
            cam.Call("startPreview");
            flashOnImage.SetActive(true);
            flashOffImage.SetActive(false);
        }
        button.onClick.Invoke();
    }

    private void FlashOff()
    {
        Debug.LogWarning("Flash OFF");
        if (cam == null)
        {
            AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera");
            cam = cameraClass.CallStatic<AndroidJavaObject>("open");
        }
        if (cam != null)
        {
            AndroidJavaObject camParameters = cam.Call<AndroidJavaObject>("getParameters");
            camParameters.Call("setFlashMode", "off");
            cam.Call("setParameters", camParameters);
            cam.Call("stopPreview");
            flashOnImage.SetActive(false);
            flashOffImage.SetActive(true);
        }
        button.onClick.Invoke();
    }
}