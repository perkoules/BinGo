using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

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
