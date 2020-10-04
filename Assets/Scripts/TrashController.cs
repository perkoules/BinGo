using UnityEngine;

public class TrashController : MonoBehaviour
{
    public Camera cameraMap, cameraAR;
    public GameObject mapMainPanel, cameraPanel;

    private void OnEnable()
    {
        if (cameraMap.isActiveAndEnabled)
        {
            cameraMap.gameObject.SetActive(false);
            mapMainPanel.SetActive(false);
            cameraAR.gameObject.SetActive(true);
            cameraPanel.SetActive(true);
        }
    }

    private void OnDisable()
    {
        cameraAR.gameObject.SetActive(false);
        cameraPanel.SetActive(false);
        cameraMap.gameObject.SetActive(true);
        mapMainPanel.SetActive(true);
    }
}