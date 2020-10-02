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
    public TMP_Dropdown countryDropdown, avatarDropdown;
    private string userEmail = "";
    private string userPassword = "";
    private string username = "";
    private string myID = "";
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
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "F86EF"; // Please change this value to your own titleId from PlayFab Game Manager
        }
        messageController = FindObjectOfType<MessageController>();
        if (messageController != null)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                PlayerPrefs.DeleteKey(REGISTER_FROM_GUEST);
                PlayerPrefs.DeleteKey(IS_GUEST);
                SetGuestPlayerRegistered("NO");
                SetIsGuest(0);
                emailInput.text = GetEmail();
                passwordInput.text = GetPassword();
            }
            if (SceneManager.GetActiveScene().buildIndex == 1 && GetIsGuest() == 0)
            {
                GetDisplayName(myID);
                GetPlayerStats();
                GetPlayerData();
            }
            else if (SceneManager.GetActiveScene().buildIndex == 1 && GetIsGuest() == 1)
            {
                foreach (var avtr in playerStats.avatarImageDisplay)
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
                }
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

    #region Login

    public void GuestMode()
    {
        SetIsGuest(1);
        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnAndroidID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginSuccess, OnLoginFailure);
    }

    public void ClickToLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = GetEmail(), Password = GetPassword() };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    public static string ReturnAndroidID()
    {
        string device = SystemInfo.deviceUniqueIdentifier;
        return device;
    }

    private void OnLoginSuccess(LoginResult result)
    {
        myID = result.PlayFabId;
        GetDisplayName(myID);
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

    #endregion Login

    #region Register

    public void ClickToRegister()
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = username };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }

    private void OnDisplayName(UpdateUserTitleDisplayNameResult result)
    {
        string capitalFirst = result.DisplayName.Replace(result.DisplayName.First(), char.ToUpper(result.DisplayName.First()));
        SetUsername(capitalFirst);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        SetEmail(userEmail);
        SetPassword(userPassword);
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = GetUsername() }, OnDisplayName, OnRegisterFailure);
        SetCountry(countryDropdown.captionText.text);
        SetAvatar(avatarDropdown.captionText.text);
        SetPlayerData();
        myID = result.PlayFabId;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            messageController.messages[0].SetActive(true);
            StartCoroutine(LoggingProcessSucceeded());
        }
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
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
        SetEmail(userEmail);
        SetPassword(userPassword);
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = username }, OnDisplayName, OnRegisterFailure);
        SetCountry(countryDropdown.captionText.text);
        SetAvatar(avatarDropdown.captionText.text);
        SetPlayerData();
        SetGuestPlayerRegistered("YES");
        messageController.messages[0].SetActive(true);
    }

    private void OnRegisterGuestFailure(PlayFabError error)
    {
        messageController.messages[1].SetActive(true);
    }

    #endregion Register

    private IEnumerator LoggingProcessSucceeded()
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

    private IEnumerator LoggingProcessFailed()
    {
        yield return new WaitForSeconds(3);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
        }
    }

    public void LogOut()
    {
        PlayFabAuthenticationAPI.ForgetAllCredentials();
        PlayerPrefs.DeleteKey(EMAIL_GIVEN);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    #region Playerprefs

    private const string REGISTER_FROM_GUEST = "RegisterFromGuest";
    private const string IS_GUEST = "isGuest";
    private const string EMAIL_GIVEN = "EmailGiven";
    private const string PASSWORD_GIVEN = "PasswordGiven";
    private const string USERNAME_GIVEN = "UsernameGiven";
    private const string COUNTRY_GIVEN = "CountryGiven";
    private const string AVATAR_GIVEN = "AvatarGiven";
    private const string TEAMNAME_GIVEN = "TeamnameGiven";
    private const string TASK_BADGES = "TaskBadges";

    public void SetIsGuest(int guest)
    {
        PlayerPrefs.SetInt(IS_GUEST, guest);
    }

    public int GetIsGuest()
    {
        return PlayerPrefs.GetInt(IS_GUEST);
    }

    public void SetGuestPlayerRegistered(string reg)
    {
        PlayerPrefs.SetString(REGISTER_FROM_GUEST, reg);
    }

    public string GetGuestPlayerRegistered()
    {
        return PlayerPrefs.GetString(REGISTER_FROM_GUEST);
    }

    public void SetEmail(string usrEmail)
    {
        userEmail = usrEmail;
        PlayerPrefs.SetString(EMAIL_GIVEN, usrEmail);
    }

    public string GetEmail()
    {
        return PlayerPrefs.GetString(EMAIL_GIVEN);
    }

    public void SetPassword(string usrPassword)
    {
        userPassword = usrPassword;
        PlayerPrefs.SetString(PASSWORD_GIVEN, usrPassword);
    }

    public string GetPassword()
    {
        return PlayerPrefs.GetString(PASSWORD_GIVEN);
    }

    public void SetUsername(string usrnm)
    {
        username = usrnm;
        PlayerPrefs.SetString(USERNAME_GIVEN, usrnm);
    }

    public string GetUsername()
    {
        return PlayerPrefs.GetString(USERNAME_GIVEN);
    }

    public void SetCountry(string coun)
    {
        PlayerPrefs.SetString(COUNTRY_GIVEN, coun);
    }

    public string GetCountry()
    {
        return PlayerPrefs.GetString(COUNTRY_GIVEN);
    }

    public void SetAvatar(string avtr)
    {
        PlayerPrefs.SetString(AVATAR_GIVEN, avtr);
    }

    public string GetAvatar()
    {
        return PlayerPrefs.GetString(AVATAR_GIVEN);
    }

    public void SetTeamname(string tmnm)
    {
        PlayerPrefs.SetString(TEAMNAME_GIVEN, tmnm);
    }

    public string GetTeamname()
    {
        return PlayerPrefs.GetString(TEAMNAME_GIVEN);
    }

    public void SetTasks(string tsk)
    {
        PlayerPrefs.SetString(TASK_BADGES, tsk);
    }

    public string GetTasks()
    {
        return PlayerPrefs.GetString(TASK_BADGES);
    }

    #endregion Playerprefs

    #region PlayerData

    /*--------- Stats and data defaults ---------------------*/
    public int progressLevel = 1;
    public int wasteCollected = 0;
    public int recycleCollected = 0;
    public int rubbishInPlace = 0;
    public int rubbishInDistrict = 0;
    public int rubbishInRegion = 0;
    public int rubbishInCountry = 0;
    public int coinsAvailable = 0;
    private string country = "Australia";
    private string avatar = "Avatar 1";
    private string teamname = "no team";

    public void UpdatePlayerStats()
    {
        if (playerStats.playerInfo.RubbishDistrict == null)
        {
            playerStats.playerInfo.RubbishDistrict = "NullDistricts";
        }
        if (playerStats.playerInfo.RubbishRegion == null)
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
                            LevelDisplay();
                            break;

                        case "RubbishCollected":
                            wasteCollected = eachStat.Value;
                            playerStats.playerInfo.PlayerRubbish = wasteCollected;
                            RubbishDisplay();
                            break;

                        case "RecycleCollected":
                            recycleCollected = eachStat.Value;
                            playerStats.playerInfo.PlayerRecycle = recycleCollected;
                            RubbishDisplay();
                            break;

                        case "CoinsAvailable":
                            coinsAvailable = eachStat.Value;
                            playerStats.playerInfo.PlayerCoins = coinsAvailable;
                            CoinsDisplay();
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

    public void SetPlayerData()
    {
        country = GetCountry();
        avatar = GetAvatar();
        teamname = GetTeamname();
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
            {"Country", country},
            {"Avatar", avatar},
            {"Achievements", "0"},
            {"TeamName", teamname} }
        },
        result => Debug.Log("Successfully updated user data"),
        error =>
        {
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
                SetCountry(result.Data["Country"].Value);
                SetAvatar(result.Data["Avatar"].Value);
                SetTeamname(result.Data["TeamName"].Value);
                SetTasks(result.Data["Achievements"].Value);
            }
        },
        error => Debug.Log(error.GenerateErrorReport()));
    }

    public void GetDisplayName(string playerId)
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest() { PlayFabId = playerId },
          result =>
          {
              SetUsername(result.AccountInfo.Username);
          },
          error => Debug.LogError(error.GenerateErrorReport()));
    }

    #endregion PlayerData

    public IEnumerator InitialDisplay()
    {
        yield return new WaitForSeconds(1f);
        LevelDisplay();
        RubbishDisplay();
        CoinsDisplay();
        UsernameDisplay();
        FlagDisplay();
        AvatarDisplay();
        TeamnameDisplay();
        LevelBadgeDisplay();
        playerStats.playerInfo = new PlayerInfo
        {
            PlayerUsername = GetUsername(),
            PlayerPassword = GetPassword(),
            PlayerEmail = GetEmail(),
            PlayerRubbish = wasteCollected,
            PlayerRecycle = recycleCollected,
            PlayerTeamName = GetTeamname(),
            PlayerCoins = coinsAvailable,
            PlayerCurrentLevel = progressLevel
        };
    }

    #region Displayers

    private void TeamnameDisplay()
    {
        playerStats.teamnameDisplay.text = GetTeamname();
    }

    private void AvatarDisplay()
    {
        foreach (var avtr in playerStats.avatarImageDisplay)
        {
            avtr.sprite = playerStats.avatarSelection.AssignImage(GetAvatar());
        }
    }

    private void FlagDisplay()
    {
        foreach (var flg in playerStats.flagImageDisplay)
        {
            flg.sprite = playerStats.flagSelection.AssignImage(GetCountry());
        }
    }

    private void UsernameDisplay()
    {
        foreach (var usrnm in playerStats.usernameTextDisplay)
        {
            usrnm.text = GetUsername().Replace(GetUsername().First(), char.ToUpper(GetUsername().First()));
        }
    }

    private void LevelDisplay()
    {
        foreach (var lvltxt in playerStats.levelTextDisplay)
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
        foreach (var cointxt in playerStats.coinsTextDisplay)
        {
            cointxt.text = coinsAvailable.ToString();
        }
        foreach (var vouch in playerStats.voucherTextDisplay)
        {
            float voucher = (coinsAvailable / 100.0f);
            vouch.text = voucher.ToString(("F2")) + " £";
        }
    }

    private void RubbishDisplay()
    {
        int allRubbish = wasteCollected + recycleCollected;
        foreach (var rubtxt in playerStats.rubbishTextDisplay)
        {
            rubtxt.text = allRubbish.ToString();
        }
    }

    public void LevelBadgeDisplay()
    {
        foreach (var lvlbd in playerStats.lvlBadgeDisplay)
        {
            lvlbd.sprite = playerStats.badgeController.allBadges[progressLevel - 1].sprite;
        }
    }

    #endregion Displayers
}