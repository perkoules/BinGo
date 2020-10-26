using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerDataSaver))]
public class LoginManager : MonoBehaviour
{
    public MessageController messageController;
    public TMP_InputField email, password;
    public Button loginBtn;

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
        StartCoroutine(AttemptAutoLogin());
    }
    IEnumerator AttemptAutoLogin()
    {
        yield return new WaitForSeconds(1f);
        if (!string.IsNullOrEmpty(playerDataSaver.GetUsername()) && !string.IsNullOrEmpty(playerDataSaver.GetPassword()))
        {
            loginBtn.onClick.Invoke();
        }
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
            playerDataSaver.SetGuestPlayerRegistered("YES");
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

    public void CancelLogIn()
    { 
        StopAllCoroutines();
        if (messageController.messages[0].activeSelf)
        {
            messageController.messages[0].SetActive(false);
        }
    }

    private IEnumerator LoggingProcessSucceeded()
    {
        yield return new WaitForSeconds(2f);  
        if (messageController.messages[0].activeSelf)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(1);
            if (!operation.isDone)
            {
                yield return new WaitUntil(() => operation.isDone);
            }
            messageController.messages[0].SetActive(false);
        }
        
    }

    private IEnumerator LoggingProcessFailed()
    {
        yield return new WaitForSeconds(1);
    }
}