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
    private string userEmail;
    private string userPassword;
    private string username;
    private MessageController messageController;
    public static PlayfabManager Instance { get; private set; }
    private void Awake()
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
    public void Start()
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

    #region Login
    private void AutoLogin()
    {
        if (PlayerPrefs.HasKey("EMAIL"))
        {
            userEmail = PlayerPrefs.GetString("EMAIL");
            userPassword = PlayerPrefs.GetString("PASSWORD");
            var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        }
        else
        {
            /*if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                StartCoroutine(LoggingProcessSucceededFailed());
            }*/
        }
    }
    public void GuestMode()
    {
        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnAndroidID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginAndroidSuccess, OnLoginAndroidFailure);
    }
    public void OnClickLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }
    public static string ReturnAndroidID()
    {
        string device = SystemInfo.deviceUniqueIdentifier;
        return device;
    }
    private void OnLoginAndroidSuccess(LoginResult result)
    {
        /*if (SceneManager.GetActiveScene().buildIndex == 0){SceneManager.LoadScene(1);}*/
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        //SetGuestPlayerRegistered("YES");
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            messageController.messages[0].SetActive(true);
            StartCoroutine(LoggingProcessSucceeded());
        }
    }
    private void OnLoginAndroidFailure(PlayFabError error)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            messageController.messages[1].SetActive(true);
            StartCoroutine(LoggingProcessSucceededFailed());
        }
    }
    private void OnLoginSuccess(LoginResult result)
    {
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
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
            StartCoroutine(LoggingProcessSucceededFailed());
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
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        SetCountry();
        SetAvatar();
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            messageController.messages[0].SetActive(true);
            StartCoroutine(LoggingProcessSucceeded());
        }
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
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
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
    IEnumerator LoggingProcessSucceededFailed()
    {
        yield return new WaitForSeconds(3);
        if (messageController.messages[0].activeSelf)
        {
            messageController.messages[0].SetActive(false);
        }
        if (messageController.messages[2].activeSelf)
        {
            messageController.messages[2].SetActive(false);
        }
    }

    #region Getter/Setters
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
    public TMP_Dropdown countryDropdown, avatarDropdown;
    public void SetCountry()
    {
        country = countryDropdown.captionText.text;
    }
    public void SetAvatar()
    {
        avatar = avatarDropdown.captionText.text;
    }
    private const string REGISTER_FROM_GUEST = "RegisterFromGuest";
    public void SetGuestPlayerRegistered(string reg)
    {
        PlayerPrefs.SetString(REGISTER_FROM_GUEST, reg);
    }
    public string GetGuestPlayerRegistered()
    {
        return PlayerPrefs.GetString(REGISTER_FROM_GUEST);
    }
    #endregion


    /*--------------------------------------------------------------------------------------------------*/
    public void LogOut()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        //PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    #region Unused
    /*public void SetStats()
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
    } */
    #endregion

    #region PlayerStats    
    private int progressLevel = 1;
    private int rubbishCollected = 0;
    private int coinsAvailable = 0;

    public void StartCloudUpdatePlayerStats()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
            FunctionParameter = new { cloudProgressLevel = progressLevel, cloudRubbishCollected =rubbishCollected, cloudCoinsAvailable =coinsAvailable }, // The parameter provided to your function
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        }, OnCloudUpdatePlayerStats, OnErrorShared);
    }
    private static void OnCloudUpdatePlayerStats(ExecuteCloudScriptResult result)
    {
        Debug.Log(PlayFabSimpleJson.SerializeObject(result.FunctionResult));
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        jsonResult.TryGetValue("messageValue", out object messageValue); // note how "messageValue" directly corresponds to the JSON values set in CloudScript
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

    


    public void SetRubbishCollection(int rubbish)
    {
        rubbishCollected = rubbish;
    }



    
}
