using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PortaPrompter : MonoBehaviour
{
    public GameObject book, canvas;

    private void Start()
    {
        MapCameraZoom.OnZoomChanged += MapCameraZoom_OnZoomChanged;
    }

    private void MapCameraZoom_OnZoomChanged(float currentZoom)
    {
        canvas.GetComponent<Canvas>().transform.localScale = Vector3.one * ((currentZoom / 300) + 0.2f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera") || other.gameObject.CompareTag("Player"))
        {
            if (!book.activeSelf)
            {
                book.SetActive(true);
            }
            if (!canvas.activeSelf)
            {
                canvas.SetActive(true); 
            }
            if (Camera.main.enabled && GetComponent<CapsuleCollider>().enabled)
            {
                GetComponent<CapsuleCollider>().enabled = false;
            }            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera") || other.gameObject.CompareTag("Player"))
        {
            canvas.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        MapCameraZoom.OnZoomChanged -= MapCameraZoom_OnZoomChanged;
    }
}
