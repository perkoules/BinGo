using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendListController : MonoBehaviour
{
    public List<Button> buttons;
    public FriendList friendList;

    public void OnEnable()
    {
        if (buttons[0].interactable || buttons[1].interactable || buttons[2].interactable)
        {
            PlayFabClientAPI.GetFriendsList(
                new GetFriendList() { },
                result =>
                {
                    friendList.friend1 = result.Friends[0].Username;
                    friendList.friend1Id = result.Friends[0].FriendPlayFabId;
                    friendList.friend2 = result.Friends[1].Username;
                    friendList.friend2Id = result.Friends[1].FriendPlayFabId;
                    friendList.friend3 = result.Friends[2].Username;
                    friendList.friend3Id = result.Friends[2].FriendPlayFabId;
                },
                error => Debug.LogError(error.GenerateErrorReport()));
        }
    }
}

[System.Serializable]
public class FriendList
{
    public string friend1;
    public string friend1Id;
    public string friend2;
    public string friend2Id;
    public string friend3;
    public string friend3Id;
}