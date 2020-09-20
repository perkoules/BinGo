using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayfabManager : MonoBehaviour
{
    public PlayerStats stats;
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

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            GetPlayerStats();            
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
        /*SetCountry();
        SetAvatar();*/
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
        /*SetCountry();
        SetAvatar();*/
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
    //public TMP_Dropdown countryDropdown, avatarDropdown;
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
    /*public void SetCountry()
    {
        country = countryDropdown.captionText.text;
    }
    public void SetAvatar()
    {
        avatar = avatarDropdown.captionText.text;
    }*/
    
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
        }, 
        result => GetPlayerStats(),
        error => Debug.Log(error.GenerateErrorReport()));
    }
    private static void UpdatePlayerStatisticsOnCloudResults(ExecuteCloudScriptResult result)
    {
        /*Debug.Log(PlayFabSimpleJson.SerializeObject(result.FunctionResult));
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        jsonResult.TryGetValue("messageValue", out object messageValue);
        Debug.Log((string)messageValue);*/
    }
    
    #endregion

    #region PlayerData
    /*private string country = "Australia";
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
    }*/
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
    
    public void Initialization()
    {
        foreach (var item in stats.levelTextDisplay)
        {
            item.text = progressLevel.ToString();
        }
        foreach (var item in stats.rubbishTextDisplay)
        {
            item.text = rubbishCollected.ToString();
        }
    }
    public void LogOut()
    {
        PlayFabAuthenticationAPI.ForgetAllCredentials();
        PlayerPrefs.DeleteKey(EMAIL_GIVEN);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public void SetRubbishCollection()
    {
        rubbishCollected ++;
        UpdatePlayerStatisticsOnCloud();
    }

    #region Get Player Statistics
    public void GetPlayerStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetPlayerStatsSuccess,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }
    void OnGetPlayerStatsSuccess(GetPlayerStatisticsResult result)
    {
        foreach (var eachStat in result.Statistics)
        {
            switch (eachStat.StatisticName)
            {
                case "ProgressLevel":
                    Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
                    progressLevel = eachStat.Value;
                    break;
                case "RubbishCollected":
                    Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
                    rubbishCollected = eachStat.Value;
                    break;
                case "CoinsAvailable":
                    Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
                    coinsAvailable = eachStat.Value;
                    break;
                default:
                    break;
            }
        }
        Initialization();
    }
    #endregion
}