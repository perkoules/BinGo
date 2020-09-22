using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayfabManager : MonoBehaviour
{
    public PlayerStats stats;
    public TMP_Dropdown countryDropdown, avatarDropdown;
    private string userEmail;
    private string userPassword;
    private string username;
    private bool autologin = false;
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
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                AutoLogin();
            }
            else if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                GetPlayerStats();
                GetPlayerData();
                GetDisplayName();
            }
        }
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(Initialization());
        }
    }
    #region Login
    private void AutoLogin()
    {
        if (PlayerPrefs.HasKey(EMAIL_GIVEN) && SceneManager.GetActiveScene().buildIndex == 0)
        {
            autologin = true;
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
        SetUserName("Anonymous");
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
    private void OnDisplayName(UpdateUserTitleDisplayNameResult result)
    {
        SetUserName(result.DisplayName);
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
        SetPlayerData();
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
    
    /*-----------------------Player Prefs-----------------------*/
    private const string REGISTER_FROM_GUEST = "RegisterFromGuest";
    private const string EMAIL_GIVEN = "EmailGiven";
    private const string PASSWORD_GIVEN = "PasswordGiven";
    private const string USERNAME_GIVEN = "UsernameGiven";
    public const string COUNTRY_GIVEN = "CountryGiven";
    public const string AVATAR_GIVEN = "AvatarGiven";

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
    public void SetUserName(string usrnm)
    {
        PlayerPrefs.SetString(USERNAME_GIVEN, usrnm);
    }
    public string GetUserName()
    {
        return PlayerPrefs.GetString(USERNAME_GIVEN);
    }
    public void SetCountry()
    {
        PlayerPrefs.SetString(COUNTRY_GIVEN, countryDropdown.captionText.text);
    }
    public void SetCountry(string coun)
    {
        PlayerPrefs.SetString(COUNTRY_GIVEN, coun);
    }
    public string GetCountry()
    {
        return PlayerPrefs.GetString(COUNTRY_GIVEN);
    }
    public void SetAvatar()
    {
        PlayerPrefs.SetString(AVATAR_GIVEN, avatarDropdown.captionText.text);
    }
    public void SetAvatar(string avtr)
    {
        PlayerPrefs.SetString(AVATAR_GIVEN, avtr);
    }
    public string GetAvatar()
    {
        return PlayerPrefs.GetString(AVATAR_GIVEN);
    }
    #endregion


    #region PlayerData    
    /*--------- Stats and data defaults ---------------------*/
    private int progressLevel = 1;
    public int rubbishCollected = 0;
    private int coinsAvailable = 0;
    private string country = "Australia";
    private string avatar = "Avatar 1";
    private string teamname = "no team";

    public void UpdatePlayerStats()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStats",
            FunctionParameter = new { cloudProgressLevel = progressLevel, cloudRubbishCollected = rubbishCollected, cloudCoinsAvailable = coinsAvailable },
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
                            LevelDisplay();
                            break;
                        case "RubbishCollected":
                            rubbishCollected = eachStat.Value;
                            RubbishDisplay();
                            break;
                        case "CoinsAvailable":
                            coinsAvailable = eachStat.Value;
                            CoinsDisplay();
                            break;
                        default:
                            break;
                    }
                }
            },error => Debug.LogError(error.GenerateErrorReport()));

    }
    private void SetPlayerData()
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
        error =>
        {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    private void GetPlayerData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { },
        result =>
        {
            if (result.Data == null) Debug.Log("No Data");
            else
            {
                if (PlayerPrefs.GetString(COUNTRY_GIVEN) == null || PlayerPrefs.GetString(AVATAR_GIVEN) == null)
                {
                    SetCountry(result.Data["Country"].Value);
                    SetAvatar(result.Data["Avatar"].Value);
                }
            }
        }, 
        error =>Debug.Log(error.GenerateErrorReport()));
    }
    public void GetDisplayName()
    {
        if (PlayerPrefs.GetString(USERNAME_GIVEN) == null)
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
            result => SetUserName(result.AccountInfo.Username),
            error => Debug.LogError(error.GenerateErrorReport()));
        }
    }
    #endregion

    #region PlayerLeaderboard
    public GameObject leaderboardPanel, listingPrefab;
    public void GetLeaderboardRubbishCollected()
    {
        var requestLeaderboard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "RubbishCollected", MaxResultsCount = 10 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeaderboardResults, error => Debug.LogError(error.GenerateErrorReport()));
    }
    private void OnGetLeaderboardResults(GetLeaderboardResult result)
    {
        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            GameObject obj = Instantiate(listingPrefab, leaderboardPanel.transform);
            LeaderboardListing leaderboardListing = obj.GetComponent<LeaderboardListing>();
            leaderboardListing.positionText.text = player.Position.ToString();
            leaderboardListing.playerNameText.text = player.DisplayName;
            leaderboardListing.rubbishText.text = player.StatValue.ToString();
            //Debug.Log(player.DisplayName + " : " + player.StatValue);
        }
    }
    #endregion

    IEnumerator Initialization()
    {
        yield return new WaitForSeconds(1f);
        LevelDisplay();
        RubbishDisplay();
        CoinsDisplay();
        UsernameDisplay();
        FlagDisplay();
        AvatarDisplay();
    }

    #region Displayers
    private void AvatarDisplay()
    {
        foreach (var avtr in stats.avatarImageDisplay)
        {
            avtr.sprite = stats.avatarSelection.AssignImage(GetAvatar());
        }
    }
    private void FlagDisplay()
    {
        foreach (var flg in stats.flagImageDisplay)
        {
            flg.sprite = stats.flagSelection.AssignImage(GetCountry());
        }
    }
    private void UsernameDisplay()
    {
        foreach (var usrnm in stats.usernameTextDisplay)
        {
            usrnm.text = GetUserName();
        }
    }
    private void LevelDisplay()
    {
        foreach (var lvltxt in stats.levelTextDisplay)
        {
            if (lvltxt.name.EndsWith("Next"))
            {
                int next = progressLevel + 1;
                lvltxt.text = next.ToString();
            }
            else
            {
                lvltxt.text = progressLevel.ToString();
            }
        }
    }
    private void CoinsDisplay()
    {
        foreach (var cointxt in stats.coinsTextDisplay)
        {
            cointxt.text = coinsAvailable.ToString();
        }
    }
    private void RubbishDisplay()
    {
        foreach (var rubtxt in stats.rubbishTextDisplay)
        {
            rubtxt.text = rubbishCollected.ToString();
        }
    }
    #endregion

    public void LogOut()
    {
        PlayFabAuthenticationAPI.ForgetAllCredentials();
        PlayerPrefs.DeleteKey(EMAIL_GIVEN);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public void SetRubbishCollection()
    {
        rubbishCollected ++;
        coinsAvailable++;
        UpdatePlayerStats();
    }    
}