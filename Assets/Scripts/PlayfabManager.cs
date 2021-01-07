using GoogleARCore;
using Mapbox.Geocoding;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager Instance { get; private set; }

    private PlayerDataSaver playerDataSaver;
    private GetCurrentLocation currentLocation;

    public GameObject tutorialWindow, trackingModeObject;
    public DeviceLocationProvider locationProvider;

    private int progressLevel = 1;
    private int rubbishCollected = 0;
    private int wasteCollected = 0;
    private int recycleCollected = 0;
    private int coinsAvailable = 0;

    private string place, district, region, country;

    //<-----------------           Events             ------------------------------->
    public delegate void AdjustValues(int recycle, int waste, int coins, int level);
    public delegate void AdjustNames(string team, string user); 
    public delegate void AdjustImage(string player, string country, int level);

    public static event AdjustImage OnImageAdjusted;
    public static event AdjustValues OnValuesAdjusted;
    public static event AdjustNames OnNamesAdjusted;

    //<-----------------           Data holders       ------------------------------->
    public RubbishDataHandler dataHandler;

    private void OnEnable()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "F86EF";
        }
        if (locationProvider == null)
        {
            locationProvider = FindObjectOfType<DeviceLocationProvider>();
        }
        if(playerDataSaver.GetBookObtained() == 1)
        {
            trackingModeObject.SetActive(true);
        }
    }

    public void Start()
    {
        dataHandler = GetComponent<RubbishDataHandler>();
        if (playerDataSaver.GetIsGuest() == 0)
        {
            IsFirstTime();
            StartCoroutine(Initialization());
        }
    }

    public IEnumerator Initialization()
    {
        yield return new WaitForSeconds(1.5f);
        GetLocationData();
        yield return new WaitForSeconds(0.5f);
        GetPlayerStats();
        yield return new WaitForSeconds(0.5f);
        GetPlayerData();
    }

    public void GetLocationData()
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
            place = myResult.features[p].text;
            dataHandler.placeRubbishPair.Add(place, 0);
        }
        if (d >= 0)
        {
            district = myResult.features[d].text;
            dataHandler.districtRubbishPair.Add(district, 0);
        }
        if (r >= 0)
        {
            region = myResult.features[r].text;
            dataHandler.regionRubbishPair.Add(region, 0);
        }
        if (c >= 0)
        {
            country = myResult.features[c].text;
            dataHandler.countryRubbishPair.Add(country, 0);
        }
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
                            playerDataSaver.SetProgressLevel(progressLevel);
                            break;

                        case "WasteCollected":
                            wasteCollected = eachStat.Value;
                            playerDataSaver.SetWasteCollected(wasteCollected);
                            break;

                        case "RecycleCollected":
                            recycleCollected = eachStat.Value;
                            playerDataSaver.SetRecycleCollected(recycleCollected);
                            break;

                        case "RubbishCollected":
                            rubbishCollected = eachStat.Value;
                            playerDataSaver.SetRubbishCollected(rubbishCollected);
                            break;

                        case "CoinsAvailable":
                            coinsAvailable = eachStat.Value;
                            playerDataSaver.SetCoinsAvailable(coinsAvailable);
                            break;

                        default:
                            break;
                    }
                    if (place != null)
                    {
                        if (eachStat.StatisticName == (place + " isPlace"))
                        {
                            dataHandler.placeRubbishPair[place] = eachStat.Value;
                        }
                        else if (eachStat.StatisticName == district + " isDistrict")
                        {
                            dataHandler.districtRubbishPair[district] = eachStat.Value;
                        }
                        else if (eachStat.StatisticName == region + " isRegion")
                        {
                            dataHandler.regionRubbishPair[region] = eachStat.Value;
                        }
                        else if (eachStat.StatisticName == country + " isCountry")
                        {
                            dataHandler.countryRubbishPair[country] = eachStat.Value;
                        }
                    }
                    else
                    {
                        GetLocationData();
                        GetPlayerStats();
                    }
                }
                OnValuesAdjusted(recycleCollected, wasteCollected, coinsAvailable, progressLevel);
            }, 
            error => Debug.LogError(error.GenerateErrorReport()));
    }
    
    private void GetPlayerData()
    {
        string myCountry = "";
        string myAvatar = "";
        string myTeamname = "";
        string myTasks = "";
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { },
        result =>
        {
            if (result.Data == null) Debug.Log("No Data");
            else
            {
                myCountry = result.Data["Country"].Value;
                myAvatar = result.Data["Avatar"].Value;
                myTeamname = result.Data["TeamName"].Value;
                myTasks = result.Data["Achievements"].Value;
                playerDataSaver.SetCountry(myCountry);
                playerDataSaver.SetAvatar(myAvatar);
                playerDataSaver.SetTeamname(myTeamname);
                playerDataSaver.SetTasks(myTasks);
                OnNamesAdjusted(myTeamname, playerDataSaver.GetUsername());
                OnImageAdjusted(myAvatar, myCountry, progressLevel);
            }
        },
        error => Debug.Log(error.GenerateErrorReport()));        
    }
    /// <summary>
    /// Trigger by a button in the settings
    /// </summary>
    public void Logout()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        PlayerPrefs.DeleteKey("Autologin");
        PlayerPrefs.DeleteKey("EmailGiven");
        PlayerPrefs.DeleteKey("PasswordGiven");
        LevelManager.Instance.LoadSceneAsyncByName("LogInScreen");
        LevelManager.Instance.UnloadSceneAsync("MainScreen");
    }    
    /// <summary>
    /// Triggered By Button
    /// </summary>
    public void QuitApp()
    {
        LevelManager.Instance.QuitGame();
    }
    public void IsFirstTime()
    {        
        PlayFabClientAPI.GetAccountInfo(
            new GetAccountInfoRequest { },
            result =>
            {
                DateTime dateCreated = result.AccountInfo.Created;
                DateTime dateToday = DateTime.Now;
                int daysPassed = (dateToday - dateCreated).Minutes;
                if(daysPassed <= 1)
                {
                    tutorialWindow.SetActive(true);
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }
    /// <summary>
    /// Triggered By Button
    /// </summary>
    public void OnGuestRegistered()
    {
        playerDataSaver.SetIsGuest(0);
        IsFirstTime();
        StartCoroutine(Initialization());
    }
    public void DestroyIncomingObject(GameObject go)
    {
        Destroy(go);
    }
}