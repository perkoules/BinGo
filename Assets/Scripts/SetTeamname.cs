using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetTeamname : MonoBehaviour
{
    public TMP_InputField nameToSet;
    public TextMeshProUGUI teamnameText;
    public Button setWindowButton;
    public FriendListController listController;
    private Button button;
    public delegate void AdjustNames(string team, bool on);
    public static event AdjustNames OnNamesAdjusted;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SubmitTeamName);
    }

    private void SubmitTeamName()
    {
        teamnameText.text = nameToSet.text;
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Permission = UserDataPermission.Public,
            Data = new Dictionary<string, string>() {
            {"TeamName", nameToSet.text} }
        },
        result => 
        {
            Debug.Log(nameToSet.text + " added");
            OnNamesAdjusted(nameToSet.text, true);
            SetNameToFriends(nameToSet.text);
            setWindowButton.gameObject.SetActive(false);
        },
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
    
    private void SetNameToFriends(string nameText)
    {
        if (!string.IsNullOrEmpty(listController.friendList.ElementAt(0).Key))
        {
            FriendOne(nameText);
        }
        if (!string.IsNullOrEmpty(listController.friendList.ElementAt(1).Key))
        {
            FriendTwo(nameText);
        }
        if (!string.IsNullOrEmpty(listController.friendList.ElementAt(2).Key))
        {
            FriendThree(nameText);
        }
    }
    
    private void FriendThree(string nameText)
    {
        PlayFabAdminAPI.UpdateUserData(new PlayFab.AdminModels.UpdateUserDataRequest()
        {
            PlayFabId = listController.friendList.ElementAt(2).Key,
            Permission = PlayFab.AdminModels.UserDataPermission.Public,
            Data = new Dictionary<string, string>() {
            {"TeamName", nameText} }
        },
        result => Debug.Log(nameToSet.text + " added"),
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    private void FriendTwo(string nameText)
    {
        PlayFabAdminAPI.UpdateUserData(new PlayFab.AdminModels.UpdateUserDataRequest()
        {
            PlayFabId = listController.friendList.ElementAt(1).Key,
            Permission = PlayFab.AdminModels.UserDataPermission.Public,
            Data = new Dictionary<string, string>() {
            {"TeamName", nameText} }
        },
        result => Debug.Log(nameToSet.text + " added"),
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    private void FriendOne(string nameText)
    {
        PlayFabAdminAPI.UpdateUserData(new PlayFab.AdminModels.UpdateUserDataRequest()
        {
            PlayFabId = listController.friendList.ElementAt(0).Key,
            Permission = PlayFab.AdminModels.UserDataPermission.Public,
            Data = new Dictionary<string, string>() {
            {"TeamName", nameText} }
        },
        result => Debug.Log(nameToSet.text + " added"),
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}