using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
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

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SubmitTeamName);
    }

    private void SubmitTeamName()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Permission = UserDataPermission.Public,
            Data = new Dictionary<string, string>() {
            {"TeamName", nameToSet.text} }
        },
        result => Debug.Log(nameToSet.text + " added"),
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
        teamnameText.text = nameToSet.text;
        SetNameToFriends(nameToSet.text);
        setWindowButton.gameObject.SetActive(false);
    }

    private void SetNameToFriends(string nameText)
    {
        FriendOne(nameText);
        FriendTwo(nameText);
        FriendThree(nameText);
    }

    private void FriendThree(string nameText)
    {
        PlayFabAdminAPI.UpdateUserData(new PlayFab.AdminModels.UpdateUserDataRequest()
        {
            PlayFabId = listController.friendList.friend3Id,
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
            PlayFabId = listController.friendList.friend2Id,
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
            PlayFabId = listController.friendList.friend1Id,
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