using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TasksManager : MonoBehaviour
{
    private DateTime dateCreated, dateToday;
    private int daysPassed = 0;
    public RectTransform parent;
    public GameObject[] taskPrefabs;
    public Tasks t;
    private GameObject go;


    private void Awake()
    {
    }

    private void Start()
    {
        GetDateCreated();
    }
    private void GetDateCreated()
    {        
        PlayFabClientAPI.GetAccountInfo(
            new GetAccountInfoRequest { },
            result =>
            {
                dateCreated = result.AccountInfo.Created;
                dateToday = DateTime.Today;
                daysPassed = (dateToday - dateCreated).Days;
                Debug.Log("Passed " + daysPassed);
                StartCoroutine(ShowTask());
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }
    

    IEnumerator ShowTask()
    {
        yield return new WaitUntil(() => Time.timeSinceLevelLoad > 10);
        List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
        if (daysPassed >= 0 && daysPassed < 30)
        {
            for (int i = 0; i < taskPrefabs.Length; i++)
            {
                go = Instantiate(taskPrefabs[i], parent);
                TextMeshProUGUI goText = go.GetComponentInChildren<TextMeshProUGUI>();
                texts.Add(goText);
            }
        }
        t = new Tasks
        {
            Task1 = "Collect 2 waste",
            Task2 = "Recycle 2 rubbish",
            Task3 = "Use 2 bins"
        };
        t.Daily(texts[0], texts[1], texts[2]);
    }
}
