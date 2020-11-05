using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendListController : MonoBehaviour
{
    public List<Button> addFriendButtons;
    public Dictionary<string, string> friendList;
    public List<TextMeshProUGUI> friendsName, friendsLevel;
    public List<string> friendsFlag, friendsAvatar, friendsBadge;

    public bool friendsExist = false;

    public static FriendListController Instance { get; private set; }

    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        Invoke("GetFriends", 2f);
    }
    public void GetFriends()
    {
        friendList = new Dictionary<string, string>();
        StartCoroutine(GetFriendsList());
    }

    public IEnumerator GetFriendsList()
    {
        PlayFabClientAPI.GetFriendsList(
            new GetFriendsListRequest() { },
            result =>
            {
                if(result.Friends.Count == 0)
                {
                    friendsExist = false;
                }
                else
                {
                    friendsExist = true;
                    for (int i = 0; i < 2; i++)
                    {
                        if (!string.IsNullOrEmpty(result.Friends[i].FriendPlayFabId))
                        {
                            friendList.Add(result.Friends[i].FriendPlayFabId, result.Friends[i].Username);
                        }
                    }
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
        yield return new WaitForSeconds(1f);
        if (friendsExist)
        {
            for (int i = 0; i < 2; i++)
            {
                if (!string.IsNullOrEmpty(friendList.ElementAt(i).Key))
                {
                    PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = friendList.ElementAt(i).Key },
                        result =>
                        {
                            if (result.Data == null) Debug.Log("No Data");
                            else
                            {
                                friendsName[i].text = friendList.ElementAt(i).Value;
                                friendsFlag[i] = result.Data["Country"].Value;
                                friendsAvatar[i] = result.Data["Avatar"].Value;

                            }
                        },
                        error => Debug.Log(error.GenerateErrorReport()));
                    yield return new WaitForSeconds(2f);
                }
            }
            GetTeammatesLevel();
        }
    }
    private void GetTeammatesLevel()
    {
        PlayFabClientAPI.GetFriendLeaderboard(
            new GetFriendLeaderboardRequest() { StatisticName = "ProgressLevel" },
            result =>
            {
                for (int i = 0; i < result.Leaderboard.Count; i++)
                {
                    int ind = friendsName.FindIndex(tmp => tmp.text == result.Leaderboard[i].DisplayName.ToLower());
                    if (ind != -1)
                    {
                        if (result.Leaderboard[i].DisplayName.ToLower() == friendsName[ind].text)
                        {
                            friendsLevel[ind].text = result.Leaderboard[i].StatValue.ToString();
                        }
                    }
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }
}