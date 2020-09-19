using PlayFab;
using PlayFab.AuthenticationModels;
using PlayFab.ClientModels;
using PlayFab.Internal;
using PlayFab.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayfabManager : MonoBehaviour
{
    private string userEmail;
    private string userPassword;
    private string username;
    private MessageController messageController;
    public static PlayfabManager Instance { get; private set; }
    private void OnEnable()
    {
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
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "F86EF"; // Please change this value to your own titleId from PlayFab Game Manager
        }
        messageController = FindObjectOfType<MessageController>();
        if (messageController != null)
        {
            AutoLogin();
        }
    }

    public TextMeshProUGUI rubbishCollectedDisplay;
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            rubbishCollectedDisplay = rubbishCollectedDisplay.GetComponent<TextMeshProUGUI>();
        }
    }

    #region Login
    private void AutoLogin()
    {
        if (PlayerPrefs.HasKey(EMAIL_GIVEN) && SceneManager.GetActiveScene().buildIndex == 0)
        {
            userEmail = GetUserEmail();
            userPassword = GetUserPassword();
            var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);        
        }
    }
    public void GuestMode()
    {
        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnAndroidID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginSuccess, OnLoginFailure);
    }
    public void ClickToLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }
    public static string ReturnAndroidID()
    {
        string device = SystemInfo.deviceUniqueIdentifier;
        return device;
    }
    private void OnLoginSuccess(LoginResult result)
    {
        GetUserEmail();
        GetUserPassword();
        SetGuestPlayerRegistered("YES");
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            messageController.messages[0].SetActive(true);
            StartCoroutine(LoggingProcessSucceeded());
        }
    }
    private void OnLoginFailure(PlayFabError error)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            messageController.messages[1].SetActive(true);
            StartCoroutine(LoggingProcessFailed());
        }
    }

    #endregion

    #region Register
    public void ClickToRegister()
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = username };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        SetUserEmail(userEmail);
        SetUserPassword(userPassword);
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = username }, OnDisplayName, OnRegisterFailure);
        SetCountry();
        SetAvatar();
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            messageController.messages[0].SetActive(true);
            StartCoroutine(LoggingProcessSucceeded());
        }
    }
    private void OnDisplayName(UpdateUserTitleDisplayNameResult obj)
    {
        Debug.Log(obj.DisplayName + " is your new display name");
    }
    private void OnRegisterFailure(PlayFabError error)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            messageController.messages[2].SetActive(true);
            StartCoroutine(LoggingProcessSucceeded());
        }
    }
    public void ClickToRegisterGuest()
    {
        var registerGuestRequest = new AddUsernamePasswordRequest { Email = userEmail, Password = userPassword, Username = username };
        PlayFabClientAPI.AddUsernamePassword(registerGuestRequest, OnRegisterGuestSuccess, OnRegisterGuestFailure);
    }
    private void OnRegisterGuestSuccess(AddUsernamePasswordResult result)
    {
        SetUserEmail(userEmail);
        SetUserPassword(userPassword);
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = username }, OnDisplayName, OnRegisterFailure);
        SetCountry();
        SetAvatar();
        SetGuestPlayerRegistered("YES");
        messageController.messages[0].SetActive(true);
    }
    private void OnRegisterGuestFailure(PlayFabError error)
    {
        messageController.messages[1].SetActive(true);
    }
    #endregion
        
    IEnumerator LoggingProcessSucceeded()
    {
        yield return new WaitForSeconds(3);
        SetPlayerData();
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (messageController.messages[0].activeSelf)
            {
                messageController.messages[0].SetActive(false);
                SceneManager.LoadScene(1);
            }
        }
    }
    IEnumerator LoggingProcessFailed()
    {
        yield return new WaitForSeconds(3);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            
        }
    }



    #region Global Getters and Setters
    public TMP_Dropdown countryDropdown, avatarDropdown;
    public void GetUsername(string usernameIn)
    {
        username = usernameIn;
    }
    public void GetUserPassword(string passIn)
    {
        userPassword = passIn;
    }
    public void GetUserEmail(string emailIn)
    {
        userEmail = emailIn;
    }
    public void SetCountry()
    {
        country = countryDropdown.captionText.text;
    }
    public void SetAvatar()
    {
        avatar = avatarDropdown.captionText.text;
    }
    
    /*-----------------------Player Prefs-----------------------*/
    private const string REGISTER_FROM_GUEST = "RegisterFromGuest";
    private const string EMAIL_GIVEN = "EmailGiven";
    private const string PASSWORD_GIVEN = "PasswordGiven";
    public void SetGuestPlayerRegistered(string reg)
    {
        PlayerPrefs.SetString(REGISTER_FROM_GUEST, reg);
    }
    public string GetGuestPlayerRegistered()
    {
        return PlayerPrefs.GetString(REGISTER_FROM_GUEST);
    }
    public void SetUserEmail(string usrEmail)
    {
        PlayerPrefs.SetString(EMAIL_GIVEN, usrEmail);
    }
    public string GetUserEmail()
    {
        return PlayerPrefs.GetString(EMAIL_GIVEN);
    }
    public void SetUserPassword(string usrPassword)
    {
        PlayerPrefs.SetString(PASSWORD_GIVEN, usrPassword);
    }
    public string GetUserPassword()
    {
        return PlayerPrefs.GetString(PASSWORD_GIVEN);
    }
    
    #endregion


    /*--------------------------------------------------------------------------------------------------*/
    public void LogOut()
    {
        PlayFabAuthenticationAPI.ForgetAllCredentials();
        PlayerPrefs.DeleteKey(EMAIL_GIVEN);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    

    #region PlayerStats    
    private int progressLevel = 1;
    private int rubbishCollected = 0;
    private int coinsAvailable = 0;

    public void UpdatePlayerStatisticsOnCloud()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStats", 
            FunctionParameter = new { cloudProgressLevel = progressLevel, cloudRubbishCollected =rubbishCollected, cloudCoinsAvailable =coinsAvailable }, 
            GeneratePlayStreamEvent = true,
        }, UpdatePlayerStatisticsOnCloudResults, OnErrorShared);
    }
    private static void UpdatePlayerStatisticsOnCloudResults(ExecuteCloudScriptResult result)
    {
        Debug.Log(PlayFabSimpleJson.SerializeObject(result.FunctionResult));
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        jsonResult.TryGetValue("messageValue", out object messageValue);
        Debug.Log((string)messageValue);
    }
    private static void OnErrorShared(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    #endregion

    #region PlayerData
    private string country = "Australia";
    private string avatar = "Avatar 1";
    private string teamname = "no team";
    void SetPlayerData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
            {"Country", country},
            {"Avatar", avatar},
            {"TeamName", teamname}
        }
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });        
    }
    void GetPlayerData(string myPlayFabeId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabeId,
            Keys = null
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data == null) Debug.Log("No Data");
            else
            {
                Debug.Log("Country: " + result.Data["Country"].Value);
                Debug.Log("Avatar: " + result.Data["Avatar"].Value);
                Debug.Log("TeamName: " + result.Data["TeamName"].Value);
            }
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    public void OnPlayerDataLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        SetPlayerData();
        GetPlayerData(result.PlayFabId);
    }
    #endregion

    #region PlayerLeaderboard



    public void GetLeaderboard()
    {
        var requestLeaderboard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "RubbishCollected", MaxResultsCount = 20 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeaderboard, OnErrorLeaderboard);
    }
    private void OnGetLeaderboard(GetLeaderboardResult result)
    {
        //Debug.Log(result.Leaderboard[0].StatValue);
        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            Debug.Log(player.DisplayName + " : " + player.StatValue);
        }
    }
    private void OnErrorLeaderboard(PlayFabError error)
    {
        Debug.LogError("Error Loading Leadderboard");
    }
    #endregion

    
    public void SetRubbishCollection(int rubbish)
    {
        rubbishCollected = rubbish;
    }
    public void GetPlayerStatisticsFromCloud()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "GetPlayerProgressLevel",
            FunctionParameter = new
            {
                cloudProgressLevel = progressLevel
            },
            GeneratePlayStreamEvent = true,
        }, GetPlayerStatisticsResults, GetPlayerStatisticsResultsError);
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "GetPlayerRubbish",
            FunctionParameter = new
            {
                cloudRubbishCollected = rubbishCollected
            },
            GeneratePlayStreamEvent = true,
        }, GetPlayerStatisticsResults, GetPlayerStatisticsResultsError);
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "GetPlayerCoins",
            FunctionParameter = new
            {
                cloudCoinsAvailable = coinsAvailable
            },
            GeneratePlayStreamEvent = true,
        }, GetPlayerStatisticsResults, GetPlayerStatisticsResultsError);
    }
    private void GetPlayerStatisticsResults(ExecuteCloudScriptResult result)
    {
        /*Debug.Log(PlayFabSimpleJson.SerializeObject(result.FunctionResult));
        int k = Convert.ToInt32(result.FunctionResult);
        Debug.LogError(k);*/
        string functionName = result.FunctionName;
        switch (functionName)
        {
            case "GetPlayerProgressLevel":
                Debug.Log("Progress: " + Convert.ToInt32(result.FunctionResult));
                break;
            case "GetPlayerRubbish":
                Debug.Log("Rubbish: " + Convert.ToInt32(result.FunctionResult));
                break;
            case "GetPlayerCoins":
                Debug.Log("Coins: " + Convert.ToInt32(result.FunctionResult));
                break;
            default:
                break;
        }
    }
    private void GetPlayerStatisticsResultsError(PlayFabError error)
    {
        Debug.Log("Cloud Script call failed");
        Debug.Log(error.GenerateErrorReport());
    }

    #region Unused Playfab
    public void SetStats()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate { StatisticName = "ProgressLevel", Value = progressLevel },
                new StatisticUpdate { StatisticName = "RubbishCollected", Value = rubbishCollected },
                new StatisticUpdate { StatisticName = "CoinsAvailable", Value = coinsAvailable },

            }
        },
        result => { Debug.Log("User statistics updated"); },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }
    void GetStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStats,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }
    void OnGetStats(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
        }
    }
    #endregion
}