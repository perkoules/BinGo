using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerDataSaver))]
public class MonsterDestroyer : MonoBehaviour
{
    private PlayerDataSaver playerDataSaver;
    private bool monsterGotHit = false;
    public int monstersKilled = 0;

    private void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        GetMonstersFromCloud();
    }


    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            Raycasting(Input.mousePosition);
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log("Android");
            Raycasting(Input.GetTouch(0).position);
        }
#endif
    }

    private void Raycasting(Vector3 position)
    {
        if (Camera.main.isActiveAndEnabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                GameObject go = hit.transform.gameObject;
                if (go.name.Contains("Monster") && !monsterGotHit)
                {
                    monsterGotHit = true;
                    go.GetComponent<Animator>().SetTrigger("IsDead");
                    StartCoroutine(Death(go));
                }
            }
        }
    }

    IEnumerator Death(GameObject go)
    {
        yield return new WaitForSeconds(4f);
        monsterGotHit = false;
        Destroy(go);
        monstersKilled++;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("Sent Monsters To Cloud");
            SetMonstersStats();
        }
    }

    public void SetMonstersStats()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStats",
            FunctionParameter = new
            {
                cloudMonstersKilled = monstersKilled
            },
            GeneratePlayStreamEvent = true,
        },
        result => GetMonstersFromCloud(),
        error => Debug.Log(error.GenerateErrorReport()));;
    }

    private void GetMonstersFromCloud()
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(),
            result =>
            {
                foreach (var stat in result.Statistics)
                {
                    if (stat.StatisticName == "MonstersKilled")
                    {
                        playerDataSaver.SetMonstersKilled(stat.Value);
                        monstersKilled = playerDataSaver.GetMonstersKilled();
                    }
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }
}
