using Michsky.UI.ModernUIPack;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTrace : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public ModalWindowManager windowManager, logoMessage;
    public delegate void BookObtained();
    public static event BookObtained OnBookObtained;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        LogoPoints.OnLogoFound += LogoObtained;
    }

    private void LogoObtained()
    {
        logoMessage.OpenWindow();
        LogoPoints.OnLogoFound -= LogoObtained;
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
                OnBookObtained();
                windowManager.OpenWindow();
            }
        }
    }

    public delegate void AdjustValues(int coins);
    public static event AdjustValues OnValuesAdjusted;
    public PlayerDataSaver playerDataSaver;

    public void SendCoins(int coinsGained)
    {
        int newCoins = playerDataSaver.GetCoinsAvailable() - coinsGained;
        playerDataSaver.SetCoinsAvailable(newCoins);
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerCoins",
            FunctionParameter = new
            {
                cloudCoinsAvailable = newCoins
            },
            GeneratePlayStreamEvent = true,
        },
        result => Debug.Log("Sent " + playerDataSaver.GetCoinsAvailable() + " coins to cloudscript"),
        error => Debug.Log(error.GenerateErrorReport()));
        OnValuesAdjusted(newCoins);
    }
}
