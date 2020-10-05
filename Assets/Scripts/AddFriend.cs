using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddFriend : MonoBehaviour
{
    public TextMeshProUGUI username, level;
    public Image countryImage, avatarImage, levelBadge;
    public TMP_InputField friendToFind;
    public Container flagSelection, avatarSelection, levelBadgeSelection;
    public GameObject objectToHide;
    private Button button;
    public FriendListController friendListController;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        if (button.interactable)
        {
            StartCoroutine(ShowFriends());
        }
    }

    IEnumerator ShowFriends()
    {
        
        yield return new WaitForSeconds(1);
        if (button.name.Contains("1"))
        {
            friendToFind.text = friendListController.friendList.friend1;
            button.interactable = false;
            SearchForFriend();
        }
        else if (button.name.Contains("2"))
        {
            yield return new WaitForSeconds(1);
            friendToFind.text = friendListController.friendList.friend2;
            button.interactable = false;
            SearchForFriend();
        }
        else if (button.name.Contains("3"))
        {
            yield return new WaitForSeconds(1);
            friendToFind.text = friendListController.friendList.friend3;
            button.interactable = false;
            SearchForFriend();
        }
    }

    public void SearchForFriend()
    {
        PlayFabClientAPI.AddFriend(
             new AddFriendRequest() { FriendUsername = friendToFind.text },
             result =>
             {
                 Debug.Log(friendToFind.text + " added as a friend");
                 button.interactable = false;
             },
             error => Debug.LogError(error.GenerateErrorReport()));
        StartCoroutine(GetFriendsID());        
    }

    private IEnumerator GetFriendsID()
    {
        string friendsID = "";
        yield return new WaitForSeconds(1);
        PlayFabClientAPI.GetFriendsList(
            new GetFriendList() { },
            result =>
            {
                foreach (var item in result.Friends)
                {
                    if (item.Username == friendToFind.text)
                    {
                        friendsID = item.FriendPlayFabId;
                    }
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
        yield return new WaitForSeconds(1);
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = friendsID },
        result =>
        {
            if (result.Data == null) Debug.Log("No Data");
            else
            {
                username.text = friendToFind.text;
                avatarImage.sprite = FindImage(result.Data["Avatar"].Value);
                countryImage.sprite = FindImage(result.Data["Country"].Value);
            }
        },
        error => Debug.Log(error.GenerateErrorReport()));
        yield return new WaitForSeconds(1f);
        GetTeammatesLevel(friendsID);
        yield return new WaitForSeconds(1f);
        levelBadge.sprite = FindImage(level.text);
        objectToHide.SetActive(false);
    }

    private void GetTeammatesLevel(string teammateID)
    {
        PlayFabClientAPI.GetFriendLeaderboard(
            new GetFriendLeaderboardRequest() { StatisticName = "ProgressLevel" },
            result =>
            {
                foreach (var item in result.Leaderboard)
                {
                    if (item.PlayFabId == teammateID)
                    {
                        level.text = item.StatValue.ToString();
                    }
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }

    private Sprite FindImage(string imageToSearch)
    {
        foreach (var img in flagSelection.imageContainer)
        {
            if (img.sprite.name == imageToSearch)
            {
                return img.sprite;
            }
        }
        foreach (var img in avatarSelection.imageContainer)
        {
            if (img.sprite.name == imageToSearch)
            {
                return img.sprite;
            }
        }
        foreach (var img in levelBadgeSelection.imageContainer)
        {
            string imgObj = img.name.Remove(0, 10);
            if (imgObj == imageToSearch)
            {
                return img.sprite;
            }
        }
        return null;
    }
}