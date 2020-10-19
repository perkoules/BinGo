using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    private Button flashlightButton;
    private AndroidJavaObject cam = null;

    private void Awake()
    {
        flashlightButton = GetComponent<Button>();
        flashlightButton.onClick.AddListener(ToggleFlashlight);
    }

    private void ToggleFlashlight()
    {
        if (cam == null)
        {
			AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera");
			cam = cameraClass.CallStatic<AndroidJavaObject>("open");            
        }
        if(cam != null)
        {
            AndroidJavaObject camParameters = cam.Call<AndroidJavaObject>("getParameters");
            camParameters.Call("setFlashMode", "torch"); //"off"
            cam.Call("setParameters", camParameters);
            cam.Call("startPreview");
        }
    }
}