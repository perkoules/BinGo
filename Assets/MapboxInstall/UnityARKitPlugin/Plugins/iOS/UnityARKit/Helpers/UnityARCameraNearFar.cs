using UnityEngine;
using UnityEngine.XR.iOS;

[RequireComponent(typeof(Camera))]
public class UnityARCameraNearFar : MonoBehaviour
{
    private Camera attachedCamera;
    private float currentNearZ;
    private float currentFarZ;

    // Use this for initialization
    private void Start()
    {
        attachedCamera = GetComponent<Camera>();
        UpdateCameraClipPlanes();
    }

    private void UpdateCameraClipPlanes()
    {
        currentNearZ = attachedCamera.nearClipPlane;
        currentFarZ = attachedCamera.farClipPlane;
        UnityARSessionNativeInterface.GetARSessionNativeInterface().SetCameraClipPlanes(currentNearZ, currentFarZ);
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentNearZ != attachedCamera.nearClipPlane || currentFarZ != attachedCamera.farClipPlane)
        {
            UpdateCameraClipPlanes();
        }
    }
}