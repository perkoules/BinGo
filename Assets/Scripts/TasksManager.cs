using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TasksManager : MonoBehaviour
{
    private DateTime dateCreated, dateToday;
    private int daysPassed = 0;
    public RectTransform parent;
    public GameObject[] taskPrefabs;
    private Dictionary<int, string> tasks;

    private void Awake()
    {
        tasks = new Dictionary<int, string>()
        {
            {1, "Collect 2 waste" },
            {2, "Collect 2 recycle" },
            {3, "Collect 2 recycle" },
        };
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
        yield return new WaitUntil(() => Time.timeSinceLevelLoad > 30);
        if(daysPassed >=0 && daysPassed < 30)
        {
            for (int i = 0; i < taskPrefabs.Length; i++)
            {
                GameObject go = Instantiate(taskPrefabs[i], parent);
                TextMeshProUGUI goText = go.GetComponentInChildren<TextMeshProUGUI>();
                goText.text = tasks.ElementAt(i).Value;
            }
        }
    }


}