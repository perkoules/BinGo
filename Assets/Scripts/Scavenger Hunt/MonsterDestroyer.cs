using Mapbox.Unity.Location;
using Mapbox.Utils;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;
using UnityEngine.SpatialTracking;

[RequireComponent(typeof(PlayerDataSaver))]
public class MonsterDestroyer : MonoBehaviour
{
    public static MonsterDestroyer Instance { get; set; }
    public DeviceLocationProvider locationProvider;
    private PlayerDataSaver playerDataSaver;
    public TextMeshProUGUI monstersText, amountText;
    private bool monsterGotHit = false;
    public int monstersKilled = 0;
    public GameObject treeImg, waterCan, battlePanel;
    public bool canRaycast = false;

    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        monstersText.text = "0";
        amountText.text = "0";
        GetMonstersFromCloud();
    }

    private void Start()
    {
        GetTreeLocationFromCloud();        
    }
    private void Update()
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

    public delegate void MonsterClicked(string rayTag, GameObject go);
    public static event MonsterClicked OnMonsterClicked;
    
    public void BattlePanelController(bool en)
    {
        battlePanel.SetActive(en);
    }

    private void Raycasting(Vector3 position)
    {
        if (canRaycast) 
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                OnMonsterClicked(hit.transform.gameObject.tag, hit.transform.gameObject);
            }
        }
    }

    public void ChangeMonsterText()
    {        
        monstersKilled++;
        monstersText.text = monstersKilled.ToString();
        if (waterCan.activeSelf)
        {
            amountText.text = Mathf.FloorToInt((monstersKilled / 50)).ToString();
        }
        else
        {
            amountText.text = "1";
        }
        SetMonstersStats();
    }

    public void SetMonstersStats()
    {
        playerDataSaver.SetMonstersKilled(monstersKilled);
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdateMonstersKilled",
            FunctionParameter = new
            {
                cloudMonstersKilled = monstersKilled
            }
        },
        result => GetMonstersFromCloud(),
        error => Debug.Log(error.GenerateErrorReport())); ;
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
                        monstersText.text = monstersKilled.ToString();
                    }
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }

    private void GetTreeLocationFromCloud()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { },
        result =>
        {
            if (result.Data == null) Debug.Log("No Data");
            else
            {                
                playerDataSaver.SetTreeLocation(result.Data["Tree Location"].Value);
                Debug.Log("Tree Location" + playerDataSaver.GetTreeLocation());
                if (playerDataSaver.GetTreeLocation() != "-")
                {
                    SpawnTreeOnMap(playerDataSaver.GetTreeLocation());
                }
                else
                {
                    if (monstersKilled >= 50)
                    {
                        amountText.text = "1";
                    }
                }
            }
        },
        error => Debug.Log(error.GenerateErrorReport()));
    }

    public void PlantTree()
    {
        if (monstersKilled >= 50)
        {
            if (treeImg.activeSelf)
            {
                Vector2d latlon = locationProvider.CurrentLocation.LatitudeLongitude;
                monstersKilled -= 50;
                PlantedTreeLocationToCloud(latlon);
            }
            else
            {
                Debug.Log("Water it");
                //monstersKilled -= 50;
                amountText.text = Mathf.FloorToInt((monstersKilled / 50)).ToString();
            }
            SetMonstersStats();
        }        
    }

    private void PlantedTreeLocationToCloud(Vector2d latlon)
    {
        string treeLoc = latlon.ToString();
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>() { { "Tree Location", treeLoc } },
                Permission = UserDataPermission.Public
            },
            result => Debug.Log("Successfully planted a tree at " + treeLoc + " location"),
            error => Debug.Log(error.GenerateErrorReport()));;

        SpawnTreeOnMap(treeLoc);
    }

    private void SpawnTreeOnMap(string loc)
    {
        
        treeImg.SetActive(false);
        waterCan.SetActive(true);
        string[] locArray = loc.Split(',');
        double x = double.Parse(locArray[0]);
        double y = double.Parse(locArray[1]);
        Vector2d location = new Vector2d(x, y);
        FindObjectOfType<SpawnOnMap>().Tree(location);
    }
}