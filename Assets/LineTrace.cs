using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTrace : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public delegate void BookObtained();
    public static event BookObtained OnBookObtained;
    private float timeToHold = 2f;
    public ModalWindowManager windowManager;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Update()
    {

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButton(0))
        {
            LineTracing(Input.mousePosition);
#elif UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            LineTracing(Input.GetTouch(0).position);        
#endif            
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void LineTracing(Vector3 pos)
    {        
        Ray ray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out RaycastHit outHit, Mathf.Infinity))
        {
            if (!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
            }
            if (outHit.transform.gameObject.CompareTag("BookTag"))
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, outHit.transform.position);
                timeToHold -= Time.deltaTime;
                if (timeToHold <= 0)
                {
                    timeToHold = 2f;
                    OnBookObtained();
                    windowManager.OpenWindow();
                }
            }
        }
    }
}
