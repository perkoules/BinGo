using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Michsky.UI.ModernUIPack;
using System;

[RequireComponent(typeof(PlayerDataSaver))]
public class RegisterManager : MonoBehaviour
{
    public ModalWindowManager success, userExist;
    public CustomDropdown countryDropdown, avatarDropdown;
    public TMP_InputField usernameInputField, passwordInputField, repeatPasswordInputField, emailInputField;
    public Image tickP, tickRepP;
    private PlayerDataSaver playerDataSaver;
    private string email = "";
    private string password = "";
    private string username = "";
    public string country = "Australia";
    public string avatar = "Avatar 1";
    public string teamname = "-";
    private string myID = "";
    private int currentBuildLevel = -1;

    private void Awake()
    {
        currentBuildLevel = SceneManager.GetActiveScene().buildIndex;
        usernameInputField = usernameInputField.GetComponent<TMP_InputField>();
        passwordInputField = passwordInputField.GetComponent<TMP_InputField>();
        repeatPasswordInputField = repeatPasswordInputField.GetComponent<TMP_InputField>();
        emailInputField = emailInputField.GetComponent<TMP_InputField>();
        playerDataSaver = GetComponent<PlayerDataSaver>();
    }

    private void Start()
    {
        repeatPasswordInputField.onValueChanged.AddListener(CheckPasswordSimilarity);
    }

    #region Validity

    public void CheckLength(Image img)
    {
        if (usernameInputField.text.Length >= 5)
        {
            img.color = Color.green;
        }
        else
        {
            img.color = Color.red;
        }
    }

    public void CheckPasswordSimilarity(string str)
    {
        if (passwordInputField.text.Length >= 8 && passwordInputField.text.Equals(repeatPasswordInputField.text))
        {
            tickP.color = Color.green;
            tickRepP.color = Color.green;
        }
        else
        {
            tickP.color = Color.red;
            tickRepP.color = Color.red;
        }
    }

    public void CheckEmailValidity(Image img)
    {
        string e = emailInputField.text;
        if (e.EndsWith("@gmail.com") || e.EndsWith("@outlook.com") || e.EndsWith("@yahoo.com"))
        {
            img.color = Color.green;
        }
        else
        {
            img.color = Color.red;
        }
    }

    #endregion Validity

    public void ClickToRegister()
    {
        email = emailInputField.text;
        password = repeatPasswordInputField.text;
        username = usernameInputField.text;
        PlayFabClientAPI.RegisterPlayFabUser(
            new RegisterPlayFabUserRequest
            {
                Email = email,
                Password = password,
                Username = username,
                DisplayName = username
            },
            OnRegisterSuccess,
            error =>
            {
                userExist.OpenWindow();
                Debug.LogError(error.GenerateErrorReport());
            });
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        playerDataSaver.SetUsername(username);
        playerDataSaver.SetEmail(email);
        playerDataSaver.SetPassword(password);
        playerDataSaver.SetCountry(countryDropdown.selectedText.text);
        playerDataSaver.SetAvatar(avatarDropdown.selectedText.text);
        playerDataSaver.SetIsGuest(0);
        playerDataSaver.SetProgressLevel(1);
        playerDataSaver.SetWasteCollected(0);
        playerDataSaver.SetRecycleCollected(0);
        playerDataSaver.SetCoinsAvailable(0);        
        SetInitialPlayerStats();
        SetPlayerData();
        myID = result.PlayFabId;
        if (currentBuildLevel == 0)
        {
            StartCoroutine(LoggingProcessSucceeded());
        }
    }

    private void SetInitialPlayerStats()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStats",
            FunctionParameter = new
            {
                cloudProgressLevel = 1,
                cloudWasteCollected = 0,
                cloudRubbishCollected = 0,
                cloudRecycleCollected = 0,
                cloudStatisticNamePlace = " isPlace",
                cloudStatisticNameDistrict =" isDistrict",
                cloudStatisticNameRegion = " isRegion",
                cloudStatisticNameCountry = " isCountry",
                cloudRubbishCollectedInPlace = 0,
                cloudRubbishCollectedInDistrict = 0,
                cloudRubbishCollectedInRegion = 0,
                cloudRubbishCollectedInCountry = 0,
                cloudCoinsAvailable = 0
            },
            GeneratePlayStreamEvent = true,
        },
        result => Debug.Log(result.FunctionResult),
        error => Debug.Log(error.GenerateErrorReport()));
    }

    public void ClickToRegisterGuest()
    {
        playerDataSaver.SetTeamname(teamname);
        playerDataSaver.SetIsGuest(0);
        email = emailInputField.text;
        password = repeatPasswordInputField.text;
        username = usernameInputField.text;
        PlayFabClientAPI.AddUsernamePassword(
            new AddUsernamePasswordRequest
            {
                Email = email,
                Password = password,
                Username = username
            },
            OnRegisterGuestSuccess,
            error =>
            {
                userExist.OpenWindow();
                Debug.LogError(error.GenerateErrorReport());
            });
    }

    private void OnRegisterGuestSuccess(AddUsernamePasswordResult result)
    {
        playerDataSaver.SetUsername(username);
        playerDataSaver.SetEmail(email);
        playerDataSaver.SetPassword(password);
        playerDataSaver.SetIsGuest(0);
        playerDataSaver.SetProgressLevel(1);
        playerDataSaver.SetWasteCollected(0);
        playerDataSaver.SetRecycleCollected(0);
        playerDataSaver.SetCoinsAvailable(0);
        PlayFabClientAPI.UpdateUserTitleDisplayName(
            new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = username
            },
            resultSuccess =>
            {
                Debug.Log(username + " is the Display Name");
            },
            error =>
            {
                Debug.LogError(error.GenerateErrorReport());
                if (currentBuildLevel == 0)
                {
                    success.OpenWindow();
                    StartCoroutine(LoggingProcessSucceeded());
                }
            });
        playerDataSaver.SetCountry(countryDropdown.selectedText.text);
        playerDataSaver.SetAvatar(avatarDropdown.selectedText.text);
        SetPlayerData();
        playerDataSaver.SetIsGuest(0);
        success.OpenWindow();
    }

    public void SetPlayerData()
    {
        country = playerDataSaver.GetCountry();
        avatar = playerDataSaver.GetAvatar();
        teamname = playerDataSaver.GetTeamname();
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Permission = UserDataPermission.Public,
            Data = new Dictionary<string, string>() {
            {"Country", country},
            {"Avatar", avatar},
            {"Achievements", "0"},
            {"Tree Location", "-"},
            {"TeamName", "-"} }
        },
        result => Debug.Log("Successfully updated user data"),
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
    private IEnumerator LoggingProcessSucceeded()
    {
        success.OpenWindow();
        yield return new WaitForSeconds(3f);
        if (currentBuildLevel == 0)
        {
            SceneManager.LoadScene(1);
        }
    }
}