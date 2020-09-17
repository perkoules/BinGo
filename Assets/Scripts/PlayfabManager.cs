using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayfabManager : MonoBehaviour
{
    private string userEmail;
    private string userPassword;
    private string username;
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
        //PlayerPrefs.DeleteAll();
        AutoLogin();
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
#if UNITY_ANDROID
            var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnAndroidID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginAndroidSuccess, OnLoginAndroidFailure);
#endif
        }
    }
    public void GuestMode()
    {
        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnAndroidID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginSuccess, OnLoginFailure);
    }

    public static string ReturnAndroidID()
    {
        string device = SystemInfo.deviceUniqueIdentifier;
        return device;
    }
    private void OnLoginAndroidSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            SceneManager.LoadScene(1);
        }
    }
    private void OnLoginAndroidFailure(PlayFabError error)
    {
        Debug.LogError(error.ErrorDetails);
        Debug.LogError("User does NOT exist. Please Register");
    }
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            SceneManager.LoadScene(1);
        }
    }
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError(error.ErrorDetails);
        Debug.LogError("User does NOT exist. Please Register");        
    }
    public void OnClickLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

#endregion

#region Register
    public void OnClickRegister()
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = username };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }

    public void OnClickRegisterTemp()
    {
        var registerTempRequest = new AddUsernamePasswordRequest { Email = userEmail, Password = userPassword, Username = username };
        PlayFabClientAPI.AddUsernamePassword(registerTempRequest, OnRegisterTempSuccess, OnRegisterTempFailure);
    }
    private void OnRegisterTempSuccess(AddUsernamePasswordResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
    }
    private void OnRegisterTempFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        SceneManager.LoadScene(1);
    }
    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
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

}
