using Mapbox.Geocoding;
using PlayFab;
using PlayFab.AdminModels;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class LeaderboardManager : MonoBehaviour
{
    public ForwardGeocodeResource fgr;
    private PlayerInfo playerInfo;
    public GameObject leaderboardPanel, listingPrefab;
    private string playerID, playerName;
    public Color32 evenColor, oddColor;

    public bool worldCountries = false;
    public bool worldPlayer = false;
    public bool worldTeam = false;

    public List<string> allPlayers;
    private void Awake()
    {
        allPlayers = new List<string>();
    }

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

    public void WorldLeaderboardForRubbish()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest { StartPosition = 0, StatisticName = "RubbishCollected", MaxResultsCount = 10 },
            result =>
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
                    GetPlayersCountry(player.PlayFabId, leaderboardListing);
                    leaderboardListing.rubbishText.text = player.StatValue.ToString();
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
        worldPlayer = true;
    }

    public void GetPlayersCountry(string playerId, LeaderboardListing ll)
    {
        PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest() { PlayFabId = playerId },
        result =>
        {
            if (result.Data.ContainsKey("Country"))
            {
                ll.countryText.text = result.Data["Country"].Value;
            }
        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }

    public void PlayersCountryLeaderboard()
    {
        PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest() { PlayFabId = playerID },
        result =>
        {
            if (result.Data.ContainsKey("Country"))
            {
                PlayersCountryLeaderboardResults(result.Data["Country"].Value + " isCountry");
            }
        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }

    public void PlayersCountryLeaderboardResults(string playerCountry)
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
                leaderboardListing.countryText.text = playerCountry.Replace(" isCountry", "");
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
                leaderboardListing.countryText.text = playerCountry.Replace(" isCountry", "");
                leaderboardListing.rubbishText.text = player.StatValue.ToString();
            }
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }

    public void PlayersProgressInWorldAndCities(string whatToLookFor)
    {
        var getPlayerStatisticNames = new GetPlayerStatisticsRequest { };
        PlayFabClientAPI.GetPlayerStatistics(getPlayerStatisticNames, result =>
        {
            foreach (var stat in result.Statistics)
            {
                if (stat.StatisticName.Contains(" isPlace"))
                {
                    string place = stat.StatisticName.Replace(" isPlace", "");
                    StartCoroutine(PlayersProgressInWorldAndCitiesResults(stat.StatisticName, place, whatToLookFor));
                }
            }
        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }

    private IEnumerator PlayersProgressInWorldAndCitiesResults(string statName, string place, string whatToLookFor)
    {
        yield return new WaitForSeconds(0.1f);
        int i = 0;
        var requestLeaderboard = new GetLeaderboardRequest { StatisticName = statName };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, result =>
        {
            foreach (PlayerLeaderboardEntry player in result.Leaderboard)
            {
                if (player.PlayFabId == playerID)
                {
                    string getCountry = GetCountryFromPlace(place);
                    if (!getCountry.Contains("United Kingdom") && whatToLookFor.Contains("world"))
                    {
                        i++;
                        GameObject obj = Instantiate(listingPrefab, leaderboardPanel.transform);
                        LeaderboardListing leaderboardListing = obj.GetComponent<LeaderboardListing>();
                        leaderboardListing.positionText.text = (player.Position + 1).ToString();
                        leaderboardListing.playerNameText.text = place;
                        leaderboardListing.countryText.text = getCountry;
                        leaderboardListing.rubbishText.text = player.StatValue.ToString();
                    }
                    else if (getCountry.Contains("United Kingdom") && whatToLookFor.Contains("cities"))
                    {
                        GameObject obj = Instantiate(listingPrefab, leaderboardPanel.transform);
                        LeaderboardListing leaderboardListing = obj.GetComponent<LeaderboardListing>();
                        leaderboardListing.positionText.text = (player.Position + 1).ToString();
                        leaderboardListing.playerNameText.text = place;
                        leaderboardListing.countryText.text = getCountry;
                        leaderboardListing.rubbishText.text = player.StatValue.ToString();
                    }
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

    public IEnumerator GetWorldLeaderboardByCountry()
    {
        Dictionary<string, string> idCountry = new Dictionary<string, string>();

        PlayFabAdminAPI.GetPlayersInSegment(
            new GetPlayersInSegmentRequest { SegmentId = "CAD8FCF4CF87AD8E" },
            result =>
            {
                foreach (var item in result.PlayerProfiles)
                {
                    idCountry.Add(item.PlayerId, "");
                    allPlayers.Add(item.PlayerId);
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));

        yield return new WaitForSeconds(3);
        foreach (var id in idCountry.Keys)
        {
            PlayFabClientAPI.GetUserData(
                new PlayFab.ClientModels.GetUserDataRequest { PlayFabId = id },
                result =>
                {
                    if (idCountry.ContainsKey(id))
                    {
                        idCountry[id] = result.Data["Country"].Value;
                    }
                },
                error => Debug.LogError(error.GenerateErrorReport()));
        }
        yield return new WaitForSeconds(2);
        Dictionary<string, int> countryRubbish = new Dictionary<string, int>();
        Dictionary<string, int> countryPlayers = new Dictionary<string, int>();
        foreach (var id in idCountry.Keys)
        {
            PlayFabClientAPI.GetLeaderboardAroundPlayer(
                    new GetLeaderboardAroundPlayerRequest { PlayFabId = id, StatisticName = idCountry[id] + " isCountry" },
                    result =>
                    {
                        int index = result.Leaderboard.FindIndex(pl => pl.PlayFabId == id);
                        if (!countryRubbish.ContainsKey(idCountry[id]))
                        {
                            countryRubbish.Add(idCountry[id], result.Leaderboard[index].StatValue);
                            countryPlayers.Add(idCountry[id], 1);
                        }
                        else
                        {
                            countryRubbish[idCountry[id]] += result.Leaderboard[index].StatValue;
                            countryPlayers[idCountry[id]]++;
                        }
                    },
                    error => Debug.LogError(error.GenerateErrorReport()));
        }
        yield return new WaitForSeconds(1);
        var orderCountryRubbish = countryRubbish.OrderByDescending(key => key.Value);
        for (int i = 0; i < orderCountryRubbish.Count(); i++)
        {
            GameObject obj = Instantiate(listingPrefab, leaderboardPanel.transform);
            LeaderboardListing leaderboardListing = obj.GetComponent<LeaderboardListing>();
            if (i % 2 == 0)
            {
                obj.GetComponent<Image>().color = leaderboardListing.evenColor;
            }
            else if (i % 2 != 0)
            {
                obj.GetComponent<Image>().color = leaderboardListing.oddColor;
            }
            leaderboardListing.positionText.text = (i + 1).ToString();
            leaderboardListing.playerNameText.text = orderCountryRubbish.ElementAt(i).Key;


            int val = 0;
            foreach (var k in countryPlayers)
            {
                if(k.Key == orderCountryRubbish.ElementAt(i).Key)
                {
                    val = k.Value;
                }
            }

            leaderboardListing.countryText.text = val.ToString();
            leaderboardListing.rubbishText.text = orderCountryRubbish.ElementAt(i).Value.ToString();
        }
        worldCountries = true;
    }

    public IEnumerator GetWorldLeaderboardByTeam()
    {
        Dictionary<string, string> idTeamname = new Dictionary<string, string>();

        PlayFabAdminAPI.GetPlayersInSegment(
            new GetPlayersInSegmentRequest { SegmentId = "CAD8FCF4CF87AD8E" },
            result =>
            {
                foreach (var item in result.PlayerProfiles)
                {
                    idTeamname.Add(item.PlayerId, "");
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
        yield return new WaitForSeconds(3);
        foreach (var id in idTeamname.Keys)
        {
            PlayFabClientAPI.GetUserData(
                new PlayFab.ClientModels.GetUserDataRequest { PlayFabId = id },
                result =>
                {
                        idTeamname[id] = result.Data["TeamName"].Value;
                    if (idTeamname.ContainsKey(id))
                    {
                    }
                },
                error => Debug.LogError(error.GenerateErrorReport()));
            //Debug.Log(id + " belongs to " + idTeamname[id] + " team");
        }
        worldTeam = true;
    }
}
