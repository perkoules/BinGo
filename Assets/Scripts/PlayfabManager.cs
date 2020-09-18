using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayfabManager : MonoBehaviour
{
    private string userEmail;
    private string userPassword;
    private string username;
    
    public GameObject logginMessage;
    public PlayerPrefsManager prefsManager;

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
        AutoLogin();
    }

    #region Login
    private void AutoLogin()
    {
        /*if (PlayerPrefs.HasKey("EMAIL"))
        {
            print("has");
            userEmail = PlayerPrefs.GetString("EMAIL");
            userPassword = PlayerPrefs.GetString("PASSWORD");
            var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        }
        else
        {*/
            if (SceneManager.GetActiveScene().buildIndex != 1)
            {
                StartCoroutine(LogginFailed());
            }
        //}
    }
    public void GuestMode()
    {
#if UNITY_ANDROID
        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnAndroidID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginAndroidSuccess, OnLoginAndroidFailure);
#endif
    }
    public static string ReturnAndroidID()
    {
        string device = SystemInfo.deviceUniqueIdentifier;
        return device;
    }
    private void OnLoginAndroidSuccess(LoginResult result)
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            SceneManager.LoadScene(1);
        }
    }
    private void OnLoginAndroidFailure(PlayFabError error)
    {
        StartCoroutine(LogginFailed());
    }
    private void OnLoginSuccess(LoginResult result)
    {
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        prefsManager.SetWasRegistered("YES");
        logginMessage.SetActive(true);
        StartCoroutine(Loggin());       
    }
    private void OnLoginFailure(PlayFabError error)
    {
        StartCoroutine(LogginFailed());
    }
    public void OnClickLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        //StartCoroutine(Loggin());
    }

#endregion

#region Register
    public void OnClickRegister()
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = username };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            SceneManager.LoadScene(1);
        }
    }
    private void OnRegisterFailure(PlayFabError error)
    {
        
    }
    public void OnClickRegisterTemp()
    {
        var registerTempRequest = new AddUsernamePasswordRequest { Email = userEmail, Password = userPassword, Username = username };
        PlayFabClientAPI.AddUsernamePassword(registerTempRequest, OnRegisterTempSuccess, OnRegisterTempFailure);
    }
    private void OnRegisterTempSuccess(AddUsernamePasswordResult result)
    {
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        prefsManager.SetWasRegistered("YES");
        logginMessage.SetActive(true);
        logginMessage.GetComponentInChildren<TextMeshProUGUI>().text = "Registration was successful";
    }
    private void OnRegisterTempFailure(PlayFabError error)
    {
        logginMessage.SetActive(true);
        logginMessage.GetComponentInChildren<TextMeshProUGUI>().text = "Failed to login in. Username or email exists. Try again!";
    }
#endregion

#region InfoGetters
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
#endregion

    public void LogOut()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        //PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    IEnumerator Loggin()
    {
        yield return new WaitForSeconds(3);
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            if (logginMessage.activeSelf)
            {
                logginMessage.SetActive(false);
                SceneManager.LoadScene(1);
            }            
        }
    }
    IEnumerator LogginFailed()
    {
        yield return new WaitForSeconds(3);
        if (logginMessage.activeSelf)
        {
            logginMessage.SetActive(false);
        }
    }
}
