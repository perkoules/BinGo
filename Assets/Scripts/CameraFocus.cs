using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CameraFocus : MonoBehaviour
{
    public ARCameraManager cameraManager;

    private void OnEnable()
    {
        if (!cameraManager.autoFocusEnabled)
        {
            cameraManager.autoFocusRequested = true;
        }
    }
}