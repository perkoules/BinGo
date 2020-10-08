using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerDataSaver))]
public class LoginManager : MonoBehaviour
{
    public MessageController messageController;
    public TMP_InputField email, password;

    private PlayerDataSaver playerDataSaver;
    private string myID = "";

    public static LoginManager LM;

    private void OnEnable()
    {
        if (LM == null)
        {
            LM = this;
        }
        else
        {
            Destroy(LM);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Awake()
    {        
        playerDataSaver = GetComponent<PlayerDataSaver>();
    }

    public void GuestMode()
    {
        playerDataSaver.SetIsGuest(1);
        PlayFabClientAPI.LoginWithAndroidDeviceID(
            new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnAndroidID(), CreateAccount = true },
            OnLoginSuccess,
            OnLoginFailure);
    }

    public void ClickToLogin()
    {
        PlayFabClientAPI.LoginWithEmailAddress(
            new LoginWithEmailAddressRequest { Email = email.text, Password = password.text },
            OnLoginSuccess,
            OnLoginFailure);
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
        playerDataSaver.SetEmail(email.text);
        playerDataSaver.SetPassword(password.text);
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

    public void GetDisplayName(string playerId)
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest() { PlayFabId = playerId },
          result =>
          {
              playerDataSaver.SetUsername(result.AccountInfo.Username);
          },
          error => Debug.LogError(error.GenerateErrorReport()));
    }

    private IEnumerator LoggingProcessSucceeded()
    {
        yield return new WaitForSeconds(3);

        if (messageController.messages[0].activeSelf)
        {
            messageController.messages[0].SetActive(false);
            SceneManager.LoadScene(1);
        }
    }

    private IEnumerator LoggingProcessFailed()
    {
        yield return new WaitForSeconds(1);
    }
}