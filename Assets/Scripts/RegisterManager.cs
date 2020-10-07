using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterManager : MonoBehaviour
{
    public PlayfabManager playfabManager;
    public TMP_Dropdown countryDropdown, avatarDropdown;
    public TMP_InputField usernameInputField, passwordInputField, repeatPasswordInputField, emailInputField;
    public Color32 colorDefault;
    public MessageController messageController;
    private PlayerDataSaver playerDataSaver;
    private string userEmail = "";
    private string userPassword = "";
    private string username = "";
    private string country = "Australia";
    private string avatar = "Avatar 1";
    private string teamname = "-";
    private string myID = "";

    private void Start()
    {
        colorDefault = new Color32(90, 216, 98, 255);
        usernameInputField = usernameInputField.GetComponent<TMP_InputField>();
        passwordInputField = passwordInputField.GetComponent<TMP_InputField>();
        repeatPasswordInputField = repeatPasswordInputField.GetComponent<TMP_InputField>();
        emailInputField = emailInputField.GetComponent<TMP_InputField>();
        usernameInputField.onValueChanged.AddListener(CheckLength);
        passwordInputField.onValueChanged.AddListener(CheckLength);
        repeatPasswordInputField.onValueChanged.AddListener(CheckPasswordSimilarity);
        
    }
        
    #region Validity

    private void CheckLength(string str)
    {
        if (usernameInputField.text.Length < 5)
        {
            usernameInputField.image.color = Color.red;
        }
        else
        {
            usernameInputField.image.color = colorDefault;
        }
    }

    private void CheckPasswordSimilarity(string str)
    {
        if (passwordInputField.text.Length >= 8 && passwordInputField.text.Equals(repeatPasswordInputField.text))
        {
            passwordInputField.image.color = colorDefault;
            repeatPasswordInputField.image.color = colorDefault;
        }
        else
        {
            passwordInputField.image.color = Color.red;
            repeatPasswordInputField.image.color = Color.red;
        }
    }

    #endregion Validity

    public void ClickToRegister()
    {
        userEmail = usernameInputField.text;
        userPassword = repeatPasswordInputField.text;
        username = usernameInputField.text;
        PlayFabClientAPI.RegisterPlayFabUser(
            new RegisterPlayFabUserRequest
            {
                Email = userEmail,
                Password = userPassword,
                Username = username
            },
            OnRegisterSuccess,
            error =>
            {
                Debug.LogError(error.GenerateErrorReport());
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    messageController.messages[2].SetActive(true);
                    StartCoroutine(LoggingProcessFailed());
                }
            });
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        playerDataSaver.SetEmail(userEmail);
        playerDataSaver.SetPassword(userPassword);
        PlayFabClientAPI.UpdateUserTitleDisplayName(
            new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = playerDataSaver.GetUsername()
            },
            resultSuccess =>
            {
                string capitalFirst = resultSuccess.DisplayName.Replace(resultSuccess.DisplayName.First(), char.ToUpper(resultSuccess.DisplayName.First()));
                playerDataSaver.SetUsername(capitalFirst);
                playerDataSaver.SetCountry(countryDropdown.captionText.text);
                playerDataSaver.SetAvatar(avatarDropdown.captionText.text);
            },
            error =>
            {
                Debug.LogError(error.GenerateErrorReport());
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    messageController.messages[2].SetActive(true);
                    StartCoroutine(LoggingProcessFailed());
                }
            });

        SetPlayerData();
        myID = result.PlayFabId;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            messageController.messages[0].SetActive(true);
            StartCoroutine(LoggingProcessSucceeded());
        }
    }

    /*
    public void ClickToRegisterGuest()
    {
        PlayFabClientAPI.AddUsernamePassword(
            new AddUsernamePasswordRequest
            {
                Email = userEmail,
                Password = userPassword,
                Username = username
            },
            OnRegisterGuestSuccess,
            OnRegisterGuestFailure);
    }

    private void OnRegisterGuestSuccess(AddUsernamePasswordResult result)
    {
        playerDataSaver.SetEmail(userEmail);
        playerDataSaver.SetPassword(userPassword);
        PlayFabClientAPI.UpdateUserTitleDisplayName(
            new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = username
            },
            OnDisplayName,
            error =>
            {
                Debug.LogError(error.GenerateErrorReport());
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    messageController.messages[2].SetActive(true);
                    StartCoroutine(LoggingProcessSucceeded());
                }
            });
        playerDataSaver.SetCountry(countryDropdown.captionText.text);
        playerDataSaver.SetAvatar(avatarDropdown.captionText.text);
        PlayfabManager.Instance.SetPlayerData();
        playerDataSaver.SetGuestPlayerRegistered("YES");
        messageController.messages[0].SetActive(true);
    }

    private void OnRegisterGuestFailure(PlayFabError error)
    {
        messageController.messages[1].SetActive(true);
    }
    */

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
            {"TeamName", teamname} }
        },
        result => Debug.Log("Successfully updated user data"),
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }

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
}