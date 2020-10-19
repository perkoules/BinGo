using System;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    public GameObject flashOnImage, flashOffImage;
    private Toggle flashlightButton;
    private AndroidJavaObject cam = null;

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
            //cam.Call("startPreview");
            flashOnImage.SetActive(true);
            flashOffImage.SetActive(false);
        }
    }
    private void FlashOff()
    {
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
            //cam.Call("startPreview");
            flashOnImage.SetActive(false);
            flashOffImage.SetActive(true);
        }
    }
}