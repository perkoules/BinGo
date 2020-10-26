using Mapbox.Geocoding;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayfabManager : MonoBehaviour
{
    private PlayerDataSaver playerDataSaver;
    private MessageController messageController;
    private GetCurrentLocation currentLocation;
    private int currentBuildIndex = -1;

    public PlayerInfo playerInfo;
    public DeviceLocationProvider locationProvider;
    public TMP_InputField emailInput, passwordInput;

    public static PlayfabManager Instance { get; private set; }

    private int progressLevel = 1;
    private int wasteCollected = 0;
    private int recycleCollected = 0;
    private int rubbishInPlace = 0;
    private int rubbishInDistrict = 0;
    private int rubbishInRegion = 0;
    private int rubbishInCountry = 0;
    private int coinsAvailable = 0;
    private string place, district, region, country;

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
        messageController = FindObjectOfType<MessageController>();
        currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "F86EF";
        }
        if (locationProvider == null)
        {
            locationProvider = FindObjectOfType<DeviceLocationProvider>();
        }
    }

    public void Start()
    {
        if (messageController != null)
        {
            if (currentBuildIndex == 0)
            {
                PlayerPrefs.DeleteKey(playerDataSaver.GetGuestPlayerRegistered());
                PlayerPrefs.DeleteKey(playerDataSaver.GetIsGuest().ToString());
                playerDataSaver.SetGuestPlayerRegistered("NO");
                playerDataSaver.SetIsGuest(0);
                emailInput.text = playerDataSaver.GetEmail();
                passwordInput.text = playerDataSaver.GetPassword();
            }
            if (currentBuildIndex == 1 && playerDataSaver.GetIsGuest() == 0)
            {
                StartCoroutine(Initialization());
            }
        }
    }

    private IEnumerator Initialization()
    {
        yield return new WaitForSeconds(1.5f);
        GetLocationDataOfRubbish();
        yield return new WaitForSeconds(0.5f);
        playerInfo = new PlayerInfo
        {
            PlayerUsername = playerDataSaver.GetUsername(),
            PlayerPassword = playerDataSaver.GetPassword(),
            PlayerEmail = playerDataSaver.GetEmail(),
            PlayerRubbish = playerDataSaver.GetWasteCollected(),
            PlayerRecycle = playerDataSaver.GetRecycleCollected(),
            PlayerTeamName = playerDataSaver.GetTeamname(),
            PlayerCoins = playerDataSaver.GetCoinsAvailable(),
            PlayerCurrentLevel = playerDataSaver.GetProgressLevel(),
            RubbishPlace = place,
            RubbishDistrict = district,
            RubbishRegion = region,
            RubbishCountry = country
        };
        GetPlayerStats();
        yield return new WaitForSeconds(0.5f);
        GetPlayerData();
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
            place = myResult.features[p].text;
        }
        if (d >= 0)
        {
            district = myResult.features[d].text;
        }
        if (r >= 0)
        {
            region = myResult.features[r].text;
        }
        if (c >= 0)
        {
            country = myResult.features[c].text;
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
                        if (eachStat.StatisticName == (playerInfo.RubbishPlace + " isPlace"))
                        {
                            rubbishInPlace = eachStat.Value;
                            playerInfo.RubbishInPlace = eachStat.Value;
                        }
                        else if (eachStat.StatisticName == playerInfo.RubbishDistrict + " isDistrict")
                        {
                            rubbishInDistrict = eachStat.Value;
                            playerInfo.RubbishInDistrict = eachStat.Value;
                        }
                        else if (eachStat.StatisticName == playerInfo.RubbishRegion + " isRegion")
                        {
                            rubbishInRegion = eachStat.Value;
                            playerInfo.RubbishInRegion = eachStat.Value;
                        }
                        else if (eachStat.StatisticName == (playerInfo.RubbishCountry) + " isCountry")
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

    private void GetPlayerData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { },
        result =>
        {
            if (result.Data == null) Debug.Log("No Data");
            else
            {
                playerDataSaver.SetCountry(result.Data["Country"].Value);
                playerDataSaver.SetAvatar(result.Data["Avatar"].Value);
                playerDataSaver.SetTeamname(result.Data["TeamName"].Value);
                playerDataSaver.SetTasks(result.Data["Achievements"].Value);
                playerDataSaver.SetTreeLocation(result.Data["Tree Location"].Value);
            }
        },
        error => Debug.Log(error.GenerateErrorReport()));
    }
}