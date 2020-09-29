using Mapbox.Geocoding;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{

    public ForwardGeocodeResource fgr;
    public PlayerInfo playerInfo;
    public GameObject leaderboardPanel, listingPrefab;
    private string playerID, playerName;

    private void Start()
    {
        GetPlayerID();
    }

    private void GetPlayerID()
    {
        var idrequest = new GetAccountInfoRequest { };
        PlayFabClientAPI.GetAccountInfo(idrequest,
            result =>
            {
                playerID = result.AccountInfo.PlayFabId;
                playerName = result.AccountInfo.Username;
            }
        , error => Debug.LogError(error.GenerateErrorReport()));
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
            if (result.Leaderboard.Count == 0)
            {
                GameObject obj = Instantiate(listingPrefab, leaderboardPanel.transform);
                LeaderboardListing leaderboardListing = obj.GetComponent<LeaderboardListing>();
                leaderboardListing.positionText.text = "1";
                leaderboardListing.playerNameText.text = playerName;
                leaderboardListing.countryText.text = playerCountry;
                leaderboardListing.rubbishText.text = playerInfo.PlayerRubbish.ToString();
            }
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

    public List<string> strs;
    //Place
    //Position in that place
    //Country of place
    //rubbish in that place

    public void GetWorldLeaderboard()
    {
        var getPlayerStatisticNames = new GetPlayerStatisticsRequest {};
        PlayFabClientAPI.GetPlayerStatistics(getPlayerStatisticNames, result =>
        {
            foreach (var stat in result.Statistics)
            {
                if (stat.StatisticName.Contains("Place"))
                {
                    string place = stat.StatisticName.Replace("Place", "");
                    StartCoroutine(GetLeaderboardInWorld(stat.StatisticName, place));
                }
            }
        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }

    private IEnumerator GetLeaderboardInWorld(string statName, string place)
    {
        yield return new WaitForSeconds(0.1f);
        var requestLeaderboard = new GetLeaderboardRequest { StatisticName = statName};
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, result =>
        {
            foreach (PlayerLeaderboardEntry player in result.Leaderboard)
            {
                if (player.PlayFabId == playerID)
                {
                    GameObject obj = Instantiate(listingPrefab, leaderboardPanel.transform);
                    LeaderboardListing leaderboardListing = obj.GetComponent<LeaderboardListing>();
                    leaderboardListing.positionText.text = (player.Position + 1).ToString();
                    leaderboardListing.playerNameText.text = place;
                    leaderboardListing.countryText.text = GetCountryFromPlace(place);
                    leaderboardListing.rubbishText.text = player.StatValue.ToString();
                }
            }
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }

    public string GetCountryFromPlace(string toSearch)
    {
        string country = "";
        fgr = new ForwardGeocodeResource(toSearch) { };
        string locationUrl = fgr.GetUrl();
        var jsonLocationData = new WebClient().DownloadString(locationUrl);
        MyResult myResult = JsonUtility.FromJson<MyResult>(jsonLocationData);
        string[] tt = myResult.features[0].place_name.Split(',');
        country = tt.Last();
        return country;
    }
}
