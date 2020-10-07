using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayfabManager : MonoBehaviour
{
    public PlayerStats playerStats;
    private PlayerDataSaver playerDataSaver;

    private MessageController messageController;
    public static PlayfabManager Instance { get; private set; }

    public TMP_InputField emailInput, passwordInput;
    public Sprite guest;

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
            PlayFabSettings.TitleId = "F86EF"; // Please change this value to your own titleId from PlayFab Game Manager
        }
        messageController = FindObjectOfType<MessageController>();
        if (messageController != null)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                PlayerPrefs.DeleteKey(playerDataSaver.GetGuestPlayerRegistered());
                PlayerPrefs.DeleteKey(playerDataSaver.GetIsGuest().ToString());
                playerDataSaver.SetGuestPlayerRegistered("NO");
                playerDataSaver.SetIsGuest(0);
                emailInput.text = playerDataSaver.GetEmail();
                passwordInput.text = playerDataSaver.GetPassword();
            }
            if (SceneManager.GetActiveScene().buildIndex == 1 && playerDataSaver.GetIsGuest() == 0)
            {
                //GetDisplayName(myID);
                GetPlayerStats();
                GetPlayerData();
            }
            else if (SceneManager.GetActiveScene().buildIndex == 1 && playerDataSaver.GetIsGuest() == 1)
            {
                /*foreach (var avtr in playerStats.avatarImageDisplay)
                {
                    avtr.sprite = guest;
                }
                foreach (var flg in playerStats.flagImageDisplay)
                {
                    flg.sprite = guest;
                }
                foreach (var usrnm in playerStats.usernameTextDisplay)
                {
                    usrnm.text = "Guest" + UnityEngine.Random.Range(5000, 50000).ToString();
                }*/
            }
        }
    }

    public void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(InitialDisplay());
        }
    }

    /*--------- Stats and data defaults ---------------------*/
    public int progressLevel = 1;
    public int wasteCollected = 0;
    public int recycleCollected = 0;
    public int rubbishInPlace = 0;
    public int rubbishInDistrict = 0;
    public int rubbishInRegion = 0;
    public int rubbishInCountry = 0;
    public int coinsAvailable = 0;

    public void UpdatePlayerStats()
    {
        if (playerStats.playerInfo.RubbishDistrict == null)                     //For places with no districts
        {
            playerStats.playerInfo.RubbishDistrict = "NullDistricts";
        }
        if (playerStats.playerInfo.RubbishRegion == null)                       //For places with no regions
        {
            playerStats.playerInfo.RubbishRegion = "NullRegions";
        }
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStats",
            FunctionParameter = new
            {
                cloudProgressLevel = progressLevel,
                cloudRubbishCollected = wasteCollected,
                cloudRecycleCollected = recycleCollected,
                cloudStatisticNamePlace = playerStats.playerInfo.RubbishPlace + " isPlace",
                cloudStatisticNameDistrict = playerStats.playerInfo.RubbishDistrict + " isDistrict",
                cloudStatisticNameRegion = playerStats.playerInfo.RubbishRegion + " isRegion",
                cloudStatisticNameCountry = playerStats.playerInfo.RubbishCountry + " isCountry",
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

    public void ProgressLevelCheck()
    {
        int allRubbish = wasteCollected + recycleCollected;
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
                progressLevel = progressByRubbish.ElementAt(i).Key;
            }
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
                            playerStats.playerInfo.PlayerCurrentLevel = progressLevel;
                            playerDataSaver.SetProgressLevel(progressLevel);
                            break;

                        case "RubbishCollected":
                            wasteCollected = eachStat.Value;
                            playerStats.playerInfo.PlayerRubbish = wasteCollected;
                            playerDataSaver.SetWasteCollected(wasteCollected);
                            break;

                        case "RecycleCollected":
                            recycleCollected = eachStat.Value;
                            playerStats.playerInfo.PlayerRecycle = recycleCollected;
                            playerDataSaver.SetRecycleCollected(recycleCollected);
                            break;

                        case "CoinsAvailable":
                            coinsAvailable = eachStat.Value;
                            playerStats.playerInfo.PlayerCoins = coinsAvailable;
                            playerDataSaver.SetCoinsAvailable(coinsAvailable);
                            break;

                        default:
                            break;
                    }
                    if (playerStats.playerInfo.RubbishPlace != null)
                    {
                        if (eachStat.StatisticName == (playerStats.playerInfo.RubbishPlace + "Place"))
                        {
                            rubbishInPlace = eachStat.Value;
                            playerStats.playerInfo.RubbishInPlace = eachStat.Value;
                        }
                        else if (eachStat.StatisticName == playerStats.playerInfo.RubbishDistrict)
                        {
                            rubbishInDistrict = eachStat.Value;
                            playerStats.playerInfo.RubbishInDistrict = eachStat.Value;
                        }
                        else if (eachStat.StatisticName == playerStats.playerInfo.RubbishRegion)
                        {
                            rubbishInRegion = eachStat.Value;
                            playerStats.playerInfo.RubbishInRegion = eachStat.Value;
                        }
                        else if (eachStat.StatisticName == (playerStats.playerInfo.RubbishCountry))
                        {
                            rubbishInCountry = eachStat.Value;
                            playerStats.playerInfo.RubbishInCountry = eachStat.Value;
                        }
                    }
                    else
                    {
                        playerStats.GetLocationDataOfRubbish();
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
            }
        },
        error => Debug.Log(error.GenerateErrorReport()));
    }

    public IEnumerator InitialDisplay()
    {
        yield return new WaitForSeconds(1f);
        playerStats.playerInfo = new PlayerInfo
        {
            PlayerUsername = playerDataSaver.GetUsername(),
            PlayerPassword = playerDataSaver.GetPassword(),
            PlayerEmail = playerDataSaver.GetEmail(),
            PlayerRubbish = wasteCollected,
            PlayerRecycle = recycleCollected,
            PlayerTeamName = playerDataSaver.GetTeamname(),
            PlayerCoins = coinsAvailable,
            PlayerCurrentLevel = progressLevel
        };
    }
}