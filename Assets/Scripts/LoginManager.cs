using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using System;

[RequireComponent(typeof(PlayerDataSaver))]
public class LoginManager : MonoBehaviour
{
    public ModalWindowManager success, failure;
    public SwitchManager autologinSwitch;
    public TMP_InputField email, password;
    public MusicController musicController;

    private PlayerDataSaver playerDataSaver;
    private string myID = "";
    private bool isGuest = false;

    private void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
    }

    public void CheckIfPlayerExistsInMemory()
    {
        if (!string.IsNullOrEmpty(playerDataSaver.GetUsername()) && !string.IsNullOrEmpty(playerDataSaver.GetPassword()))
        {
            email.text = playerDataSaver.GetEmail();
            password.text = playerDataSaver.GetPassword();
            CheckAutologin();
        }
    }
    public void CheckAutologin()
    {
        if (playerDataSaver.GetShouldAutologin() == 1)
        {
            Button btn = autologinSwitch.GetComponent<Button>();
            btn.onClick.Invoke();
            ClickToLogin();
        }
    }

    public void RememberMe()
    {
        //When pressed is still off (inverted-logic if)
        if (!autologinSwitch.isOn)
        {
            playerDataSaver.SetShouldAutologin(1);
        }
        else
        {
            playerDataSaver.SetShouldAutologin(0);
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
            if (!isGuest)
            {
                playerDataSaver.SetIsGuest(0); 
            }
            else if (isGuest)
            {
                playerDataSaver.SetIsGuest(1);
            }
            if (autologinSwitch.isOn)
            {
                playerDataSaver.SetShouldAutologin(1);
            }
            else if(!autologinSwitch.isOn)
            {
                playerDataSaver.SetShouldAutologin(0);
            }
            LoggingProcessSucceeded();
        }
    }

    private void OnLoginFailure(PlayFabError error)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            MusicController.Instance.PlayFailedSound();
            failure.OpenWindow();
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

    private void LoggingProcessSucceeded()
    {
        MusicController.Instance.PlaySuccessSound();
        success.OpenWindow();
        LevelManager.Instance.LoadSceneAsyncAdditive("MainScreen");
    }
}