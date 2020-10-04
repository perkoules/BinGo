using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddFriend : MonoBehaviour
{
    public TeammateInfo teammateInfo;
    public List<GetTeammateInfo> getTeammate;
    public TMP_InputField friendToFind;
    public GameObject friendFinderWindow;
    private Button button;
    public List<Button> addTeammateButtons;
    public List<Button> teammates;
    public int whereToAdd;
    private int level = -1;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SearchAndAddFriend);
    }

    private void SearchAndAddFriend()
    {
        PlayFabClientAPI.AddFriend(
            new AddFriendRequest() { FriendUsername = friendToFind.text },
            result => Debug.Log(friendToFind.text + " added as a friend"),
            error => Debug.LogError(error.GenerateErrorReport()));
        StartCoroutine(GetFriendsID());
    }

    private IEnumerator GetFriendsID()
    {
        string friendsID = "";
        yield return new WaitForSeconds(1);
        PlayFabClientAPI.GetFriendsList(
                    new GetFriendsListRequest() { },
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
                teammateInfo.Username = friendToFind.text;
                teammateInfo.Avatar = result.Data["Avatar"].Value;
                teammateInfo.Country = result.Data["Country"].Value;
                Debug.Log(friendToFind.text);
                Debug.Log(result.Data["Avatar"].Value);
                Debug.Log(result.Data["Country"].Value);
            }
        },
        error => Debug.Log(error.GenerateErrorReport()));
        yield return new WaitForSeconds(1);
        GetTeammatesLevel(friendsID);
        yield return new WaitForSeconds(1);
        teammateInfo.Level = level;
        Debug.Log(level);
        if (whereToAdd == 1)
        {
            Destroy(addTeammateButtons[0].gameObject);
            friendFinderWindow.SetActive(false);
            teammates[0].gameObject.SetActive(true);
        }
        else if (whereToAdd == 2)
        {
            Destroy(addTeammateButtons[1].gameObject);
            friendFinderWindow.SetActive(false);
            teammates[1].gameObject.SetActive(true);
        }
        else if (whereToAdd == 3)
        {
            Destroy(addTeammateButtons[2].gameObject);
            friendFinderWindow.SetActive(false);
            teammates[2].gameObject.SetActive(true);
        }
    }

    public void SetWhereToAdd(int where)
    {
        whereToAdd = where;
    }

    private void GetTeammatesLevel(string teammateID)
    {
        PlayFabClientAPI.GetFriendLeaderboard(
            new GetFriendLeaderboardRequest() {StatisticName = "ProgressLevel" },
            result =>
            {
                foreach (var item in result.Leaderboard)
                {
                    if (item.PlayFabId == teammateID)
                    {
                        Debug.Log(" The Stat Value is: " + item.StatValue);
                        level = item.StatValue;
                    }
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }
}