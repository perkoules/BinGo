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
    public List<Tasks> tasks;
    private GameObject go;


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
                StartCoroutine(ShowTask());
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }    

    IEnumerator ShowTask()
    {
        yield return new WaitUntil(() => Time.timeSinceLevelLoad > 10);
        List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < taskPrefabs.Length; i++)
        {
            go = Instantiate(taskPrefabs[i], parent);
            objs.Add(go);
            TextMeshProUGUI goText = go.GetComponentInChildren<TextMeshProUGUI>();
            texts.Add(goText);
        }
        tasks[daysPassed].Daily(texts[0], texts[1], texts[2]);
        tasks[daysPassed].SetName(objs[0], objs[1], objs[2]);
    }
}
