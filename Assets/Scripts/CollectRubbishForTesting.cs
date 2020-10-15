using Mapbox.Geocoding;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerDataSaver))]
public class CollectRubbishForTesting : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public AchievementsController achievementsController;
    public PlayfabManager playfabManager;
    private PlayerDataSaver playerDataSaver;
    public PlayerInfo playerInfo;
    private GetCurrentLocation currentLocation;
    public DeviceLocationProvider locationProvider;

    private int progressLevel = 1;
    private int wasteCollected = 0;
    private int recycleCollected = 0;
    private int rubbishInPlace = 0;
    private int rubbishInDistrict = 0;
    private int rubbishInRegion = 0;
    private int rubbishInCountry = 0;
    private int coinsAvailable = 0;

    private Image fillerImage;
    private bool pointerDown = false;
    private void Awake()
    {
        fillerImage = GetComponent<Image>();
        playerDataSaver = GetComponent<PlayerDataSaver>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
        if (fillerImage.fillAmount >= 1)
        {
            Debug.Log("Congrats you helped the environment!!!");
        }
        fillerImage.fillAmount = 0;
    }

    private void Update()
    {
        if (pointerDown)
        {
            fillerImage.fillAmount += Time.deltaTime;
        }        
    }
    public void SetRubbishCollection(string option)
    {
        playfabManager.GetLocationDataOfRubbish();

        if (option == "c")
        {
            wasteCollected++;
            rubbishInPlace++;
            rubbishInDistrict++;
            rubbishInRegion++;
            rubbishInCountry++;
            coinsAvailable++;
            ProgressLevelCheck();
        }
        else if (option == "r")
        {
            recycleCollected++;
            rubbishInPlace++;
            rubbishInDistrict++;
            rubbishInRegion++;
            rubbishInCountry++;
            coinsAvailable += 2;
            ProgressLevelCheck();
        }
        achievementsController.rubbishToUnlockCounter = wasteCollected;
        achievementsController.recycleToUnlockCounter = recycleCollected;
        UpdatePlayerStats();
        //playfabManager.LevelBadgeDisplay();
        StartCoroutine(achievementsController.CheckAchievementUnlockability());
    }

    public void ProgressLevelCheck()
    {
        int allRubbish = playerDataSaver.GetWasteCollected() + playerDataSaver.GetRecycleCollected();
        Dictionary<int, int> progressByRubbish = new Dictionary<int, int>()
        {
            {1, 20},
            {2, 50},
            {3, 100},
            {4, 200},
            {5, 300},
            {6, 500},
            {7, 700},
            {8, 1000},
            {9, 1250},
            {10, 1500},
            {11, 1750},
            {12, 2000},
            {13, 2500},
            {14, 3000},
            {15, 5000}
        };
        for (int i = 1; i < progressByRubbish.Count; i++)
        {
            if (progressByRubbish.ElementAt(i - 1).Value <= allRubbish && allRubbish <= progressByRubbish.ElementAt(i).Value)
            {
                playerDataSaver.SetProgressLevel(progressByRubbish.ElementAt(i).Key);
            }
        }
    }

    public void UpdatePlayerStats()
    {
        if (playerInfo.RubbishDistrict == null)                     //For places with no districts
        {
            playerInfo.RubbishDistrict = "NullDistricts";
        }
        if (playerInfo.RubbishRegion == null)                       //For places with no regions
        {
            playerInfo.RubbishRegion = "NullRegions";
        }
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStats",
            FunctionParameter = new
            {
                cloudProgressLevel = progressLevel,
                cloudRubbishCollected = wasteCollected,
                cloudRecycleCollected = recycleCollected,
                cloudStatisticNamePlace = playerInfo.RubbishPlace + " isPlace",
                cloudStatisticNameDistrict = playerInfo.RubbishDistrict + " isDistrict",
                cloudStatisticNameRegion = playerInfo.RubbishRegion + " isRegion",
                cloudStatisticNameCountry = playerInfo.RubbishCountry + " isCountry",
                cloudRubbishCollectedInPlace = rubbishInPlace,
                cloudRubbishCollectedInDistrict = rubbishInDistrict,
                cloudRubbishCollectedInRegion = rubbishInRegion,
                cloudRubbishCollectedInCountry = rubbishInCountry,
                cloudCoinsAvailable = coinsAvailable
            },
            GeneratePlayStreamEvent = true,
        },
        result => GetPlayerStats(),
        error => Debug.Log(error.GenerateErrorReport()));
    }

    public void GetPlayerStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            result =>
            {
                foreach (var eachStat in result.Statistics)
                {
                    switch (eachStat.StatisticName)
                    {
                        case "ProgressLevel":
                            progressLevel = eachStat.Value;
                            playerInfo.PlayerCurrentLevel = progressLevel;
                            playerDataSaver.SetProgressLevel(progressLevel);
                            break;

                        case "RubbishCollected":
                            wasteCollected = eachStat.Value;
                            playerInfo.PlayerRubbish = wasteCollected;
                            playerDataSaver.SetWasteCollected(wasteCollected);
                            break;

                        case "RecycleCollected":
                            recycleCollected = eachStat.Value;
                            playerInfo.PlayerRecycle = recycleCollected;
                            playerDataSaver.SetRecycleCollected(recycleCollected);
                            break;

                        case "CoinsAvailable":
                            coinsAvailable = eachStat.Value;
                            playerInfo.PlayerCoins = coinsAvailable;
                            playerDataSaver.SetCoinsAvailable(coinsAvailable);
                            break;

                        default:
                            break;
                    }
                    if (playerInfo.RubbishPlace != null)
                    {
                        if (eachStat.StatisticName == (playerInfo.RubbishPlace + "Place"))
                        {
                            rubbishInPlace = eachStat.Value;
                            playerInfo.RubbishInPlace = eachStat.Value;
                        }
                        else if (eachStat.StatisticName == playerInfo.RubbishDistrict)
                        {
                            rubbishInDistrict = eachStat.Value;
                            playerInfo.RubbishInDistrict = eachStat.Value;
                        }
                        else if (eachStat.StatisticName == playerInfo.RubbishRegion)
                        {
                            rubbishInRegion = eachStat.Value;
                            playerInfo.RubbishInRegion = eachStat.Value;
                        }
                        else if (eachStat.StatisticName == (playerInfo.RubbishCountry))
                        {
                            rubbishInCountry = eachStat.Value;
                            playerInfo.RubbishInCountry = eachStat.Value;
                        }
                    }
                    else
                    {
                        GetLocationDataOfRubbish();
                        GetPlayerStats();
                    }
                }
            }, error => Debug.LogError(error.GenerateErrorReport()));
    }

    public void GetLocationDataOfRubbish()
    {
        Vector2d latlon = locationProvider.CurrentLocation.LatitudeLongitude;
        currentLocation = new GetCurrentLocation(latlon)
        {
            Types = new string[] { "country", "region", "district", "place" }       //What features to focus on
        };

        string locationUrl = currentLocation.GetUrl();                              //Get Api location url
        var jsonLocationData = new WebClient().DownloadString(locationUrl);         //Get json results from url

        MyResult myResult = JsonUtility.FromJson<MyResult>(jsonLocationData);       //Example of results:
        int p = myResult.features.FindIndex(f => f.id.Contains("place"));           //[0] = Bishop Auckland
        int d = myResult.features.FindIndex(f => f.id.Contains("district"));        //[1] = Durham
        int r = myResult.features.FindIndex(f => f.id.Contains("region"));          //[2] = England
        int c = myResult.features.FindIndex(f => f.id.Contains("country"));         //[3] = United Kingdom
        if (p >= 0)
        {
            playerInfo.RubbishPlace = myResult.features[p].text;
        }
        if (d >= 0)
        {
            playerInfo.RubbishDistrict = myResult.features[d].text;
        }
        if (r >= 0)
        {
            playerInfo.RubbishRegion = myResult.features[r].text;
        }
        if (c >= 0)
        {
            playerInfo.RubbishCountry = myResult.features[c].text;
        }
    }

    
}