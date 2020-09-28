using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject leaderboardPanel, listingPrefab;
    private string playerID;

    private void Start()
    {
        GetPlayerID();
    }

    private void GetPlayerID()
    {
        var idrequest = new GetAccountInfoRequest { };
        PlayFabClientAPI.GetAccountInfo(idrequest, result => playerID = result.AccountInfo.PlayFabId, error => Debug.LogError(error.GenerateErrorReport()));
    }

    public void GetLeaderboardRubbishCollected()
    {
        var requestLeaderboard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "RubbishCollected", MaxResultsCount = 10 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeaderboardRubbishResults, error => Debug.LogError(error.GenerateErrorReport()));
    }
    private void OnGetLeaderboardRubbishResults(GetLeaderboardResult result)
    {
        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            GameObject obj = Instantiate(listingPrefab, leaderboardPanel.transform);
            LeaderboardListing leaderboardListing = obj.GetComponent<LeaderboardListing>();

            if (player.Position % 2 == 0)
            {
                obj.GetComponent<Image>().color = leaderboardListing.evenColor;
            }
            else if (player.Position % 2 != 0)
            {
                obj.GetComponent<Image>().color = leaderboardListing.oddColor;
            }
            leaderboardListing.positionText.text = (player.Position + 1).ToString();
            leaderboardListing.playerNameText.text = player.DisplayName;
            GetCountryForLeaderboard(player.PlayFabId, leaderboardListing);
            leaderboardListing.rubbishText.text = player.StatValue.ToString();
        }
    }
    private void GetCountryForLeaderboard(string playerId, LeaderboardListing ll)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = playerId },
        result =>
        {
            if (result.Data.ContainsKey("Country"))
            {
                ll.countryText.text = result.Data["Country"].Value;
            }
        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }


    public void GetCountryLeaderboard()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { PlayFabId = playerID }, 
        result =>
        {
            if (result.Data.ContainsKey("Country"))
            {
                GetLeaderboardInCountry(result.Data["Country"].Value);
            }
        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }
    public void GetLeaderboardInCountry(string playerCountry)
    {
        var requestLeaderboard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = playerCountry, MaxResultsCount = 10 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, result =>
        {
            foreach (PlayerLeaderboardEntry player in result.Leaderboard)
            {
                GameObject obj = Instantiate(listingPrefab, leaderboardPanel.transform);
                LeaderboardListing leaderboardListing = obj.GetComponent<LeaderboardListing>();

                if (player.Position % 2 == 0)
                {
                    obj.GetComponent<Image>().color = leaderboardListing.evenColor;
                }
                else if (player.Position % 2 != 0)
                {
                    obj.GetComponent<Image>().color = leaderboardListing.oddColor;
                }
                leaderboardListing.positionText.text = (player.Position + 1).ToString();
                leaderboardListing.playerNameText.text = player.DisplayName;
                leaderboardListing.countryText.text = playerCountry;
            leaderboardListing.rubbishText.text = player.StatValue.ToString();
            }
        }, error => Debug.LogError(error.GenerateErrorReport()));

    }
}
