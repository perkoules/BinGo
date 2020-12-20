using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

[RequireComponent(typeof(PlayerDataSaver))]
public class LoginManager : MonoBehaviour
{
    public NotificationManager success, failure;

    public TMP_InputField email, password;
    public Button loginBtn;

    private PlayerDataSaver playerDataSaver;
    private string myID = "";
    private bool isGuest = false;
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
    }


    private void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        if (playerDataSaver.GetShouldAutologin() == 1)
        {
            StartCoroutine(AttemptAutoLogin());
        }
    }

    public void ShouldAutologin(bool isOn)
    {
        if (isOn)
        {
            playerDataSaver.SetShouldAutologin(1);
        }
        else
        {
            playerDataSaver.SetShouldAutologin(0);
        }
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
        isGuest = true;
        playerDataSaver.SetIsGuest(1);
        PlayFabClientAPI.LoginWithAndroidDeviceID(
            new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnAndroidID(), CreateAccount = true },
            OnLoginSuccess,
            OnLoginFailure);
    }

    public void ClickToLogin()
    {
        isGuest = false;
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
            success.OpenNotification();
            if (!isGuest)
            {
                playerDataSaver.SetIsGuest(0); 
            }
            else if (isGuest)
            {
                playerDataSaver.SetIsGuest(1);
            }
            StartCoroutine(LoggingProcessSucceeded());
        }
    }

    private void OnLoginFailure(PlayFabError error)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            failure.OpenNotification();
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
        yield return new WaitForSeconds(2f);
        success.OpenNotification();
        if (success.isActiveAndEnabled)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(1);
            while (!operation.isDone)
            {
                yield return null;
            }
        }
    }

    private IEnumerator LoggingProcessFailed()
    {
        yield return new WaitForSeconds(1);
    }
}