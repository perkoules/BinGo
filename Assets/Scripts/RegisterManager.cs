﻿using PlayFab;
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

public class RegisterManager : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public TMP_Dropdown countryDropdown, avatarDropdown;
    public TMP_InputField usernameInputField, passwordInputField, repeatPasswordInputField, emailInputField;
    public Color32 colorDefault;
    public MessageController messageController;
    private PlayerDataSaver playerDataSaver;
    public Image userValidImg, passValidImg, repPassValidImg, emailValidImg;
    public Sprite greenLeaf, redLeaf;
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
        colorDefault = new Color32(90, 216, 98, 255);
        usernameInputField = usernameInputField.GetComponent<TMP_InputField>();
        passwordInputField = passwordInputField.GetComponent<TMP_InputField>();
        repeatPasswordInputField = repeatPasswordInputField.GetComponent<TMP_InputField>();
        emailInputField = emailInputField.GetComponent<TMP_InputField>();
        playerDataSaver = GetComponent<PlayerDataSaver>();
    }

    private void Start()
    {
        usernameInputField.onValueChanged.AddListener(CheckLength);
        passwordInputField.onValueChanged.AddListener(CheckLength);
        repeatPasswordInputField.onValueChanged.AddListener(CheckPasswordSimilarity);
    }

    #region Validity

    private void CheckLength(string str)
    {
        if (usernameInputField.text.Length < 5)
        {
            //usernameInputField.image.color = Color.red;
            userValidImg.gameObject.SetActive(true);
            userValidImg.sprite = redLeaf;
        }
        else
        {
            //usernameInputField.image.color = colorDefault;
            userValidImg.gameObject.SetActive(true);
            userValidImg.sprite = greenLeaf;
        }
    }

    private void CheckPasswordSimilarity(string str)
    {
        if (passwordInputField.text.Length >= 8 && passwordInputField.text.Equals(repeatPasswordInputField.text))
        {
            //passwordInputField.image.color = colorDefault;
            //repeatPasswordInputField.image.color = colorDefault;
            passValidImg.gameObject.SetActive(true);
            repPassValidImg.gameObject.SetActive(true);
            passValidImg.sprite = greenLeaf;
            repPassValidImg.sprite = greenLeaf;
        }
        else
        {
            //passwordInputField.image.color = Color.red;
            //repeatPasswordInputField.image.color = Color.red;
            passValidImg.gameObject.SetActive(true);
            repPassValidImg.gameObject.SetActive(true);
            passValidImg.sprite = redLeaf;
            repPassValidImg.sprite = redLeaf;
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
                Username = username
            },
            OnRegisterSuccess,
            error =>
            {
                Debug.LogError(error.GenerateErrorReport());
                if (currentBuildLevel == 0)
                {
                    messageController.messages[2].SetActive(true);
                }
                else
                {
                    messageController.messages[1].SetActive(true);
                }
            });
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        playerDataSaver.SetUsername(username);
        playerDataSaver.SetEmail(email);
        playerDataSaver.SetPassword(password);
        playerDataSaver.SetCountry(countryDropdown.captionText.text);
        playerDataSaver.SetAvatar(avatarDropdown.captionText.text);
        PlayFabClientAPI.UpdateUserTitleDisplayName(
            new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = playerDataSaver.GetUsername()
            },
            resultSuccess =>
            {
                string capitalFirst = resultSuccess.DisplayName.Replace(resultSuccess.DisplayName.First(), char.ToUpper(resultSuccess.DisplayName.First()));
                playerDataSaver.SetUsername(capitalFirst);
            },
            error =>
            {
                Debug.LogError(error.GenerateErrorReport());
                if (currentBuildLevel == 0)
                {
                    messageController.messages[2].SetActive(true);
                }
                else
                {
                    messageController.messages[0].SetActive(true);
                }
            });
        playerDataSaver.SetIsGuest(0);
        playerDataSaver.SetProgressLevel(1);
        playerDataSaver.SetWasteCollected(0);
        playerDataSaver.SetRecycleCollected(0);
        playerDataSaver.SetCoinsAvailable(0);
        playerInfo = new PlayerInfo
        {
            PlayerUsername = playerDataSaver.GetUsername(),
            PlayerPassword = playerDataSaver.GetPassword(),
            PlayerEmail = playerDataSaver.GetEmail(),
            PlayerRubbish = playerDataSaver.GetWasteCollected(),
            PlayerRecycle = playerDataSaver.GetRecycleCollected(),
            PlayerTeamName = playerDataSaver.GetTeamname(),
            PlayerCoins = playerDataSaver.GetCoinsAvailable(),
            PlayerCurrentLevel = playerDataSaver.GetProgressLevel()
        };
        SetPlayerData();
        myID = result.PlayFabId;
        if (currentBuildLevel == 0)
        {
            messageController.messages[0].SetActive(true);
            StartCoroutine(LoggingProcessSucceeded());
        }
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
                messageController.messages[1].SetActive(true);
                Debug.LogError(error.GenerateErrorReport());
            });
    }

    private void OnRegisterGuestSuccess(AddUsernamePasswordResult result)
    {
        playerDataSaver.SetUsername(username);
        playerDataSaver.SetEmail(email);
        playerDataSaver.SetPassword(password);
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
                    messageController.messages[2].SetActive(true);
                    StartCoroutine(LoggingProcessSucceeded());
                }
            });
        playerDataSaver.SetCountry(countryDropdown.captionText.text);
        playerDataSaver.SetAvatar(avatarDropdown.captionText.text);
        SetPlayerData();
        playerDataSaver.SetIsGuest(0);
        messageController.messages[0].SetActive(true);
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
        yield return new WaitForSeconds(3f);

        if (currentBuildLevel == 0)
        {
            if (messageController.messages[0].activeSelf)
            {
                messageController.messages[0].SetActive(false);
                SceneManager.LoadScene(1);
            }
        }
    }

}