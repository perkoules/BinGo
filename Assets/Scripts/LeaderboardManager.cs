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

    public void WorldLeaderboardForRubbish()
    {
        var requestLeaderboard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "RubbishCollected", MaxResultsCount = 10 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, WorldLeaderboardForRubbishResults, error => Debug.LogError(error.GenerateErrorReport()));
    }

    private void WorldLeaderboardForRubbishResults(GetLeaderboardResult result)
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
    }

    private void GetPlayersCountry(string playerId, LeaderboardListing ll)
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


    //This is THE INCORRECT WAY for security reasons
    //Exposing admin tasks
    public CountryRub countryRub;
    public void GetAllPlayers()
    {
        Dictionary<string, int> playersInCountry = new Dictionary<string, int>();
        Dictionary<string, string> segmentsToSearch = new Dictionary<string, string>()
        {
            {"United Kingdom", "F43CFFDFFF02BC40"},
            {"Portugal", "9AD6F24D907081C5"},
            {"Poland", "CF6BE4C64641073F"},
            {"Greece", "675A868507B5483B"},
            {"Germany", "E773C4A5A9B6FEA1"},
            {"France", "351DADD6FD249EB2"},
            {"Spain", "	9FD52454F4613737"},
            {"Sweden", "78FBE0CB313CE16F"},
        };
        StartCoroutine(GetSegment(playersInCountry, segmentsToSearch));
    }

    private IEnumerator GetSegment(Dictionary<string, int> playersInCountry, Dictionary<string, string> segmentsToSearch)
    {
        Debug.Log("Getting segments");
        foreach (var seg in segmentsToSearch)
        {
            yield return new WaitForSeconds(1);
            PlayFabAdminAPI.GetPlayersInSegment(
                    new GetPlayersInSegmentRequest() { SegmentId = segmentsToSearch[seg.Key] },
                    result =>
                    {
                        playersInCountry.Add(seg.Key, result.ProfilesInSegment); // x country has x players
                    },
                    error => Debug.LogError(error.GenerateErrorReport())
                    );
        }
        yield return new WaitForSeconds(3);
        Debug.Log("Getting Leaderboards");
        foreach (var seg in segmentsToSearch)
        {
            yield return new WaitForSeconds(1);
            PlayFabClientAPI.GetLeaderboard(
                new GetLeaderboardRequest() { StatisticName = seg.Key + " isCountry" },
                result =>
                {
                    foreach (var item in result.Leaderboard)
                    {
                        if (seg.Key.Contains("Kingdom"))
                        {
                            countryRub.unitedKingdom += item.StatValue;
                        }
                        else if (seg.Key.Contains("Portugal"))
                        {
                            countryRub.portugal += item.StatValue;
                        }
                        else if (seg.Key.Contains("Poland"))
                        {
                            countryRub.poland += item.StatValue;
                        }
                        else if (seg.Key.Contains("Greece"))
                        {
                            countryRub.greece += item.StatValue;
                        }
                        else if (seg.Key.Contains("Germany"))
                        {
                            countryRub.germany += item.StatValue;
                        }
                        else if (seg.Key.Contains("France"))
                        {
                            countryRub.france += item.StatValue;
                        }
                        else if (seg.Key.Contains("Spain"))
                        {
                            countryRub.spain += item.StatValue;
                        }
                        else if (seg.Key.Contains("Sweden"))
                        {
                            countryRub.sweden += item.StatValue;
                        }
                    }
                },
                error => Debug.LogError(error.GenerateErrorReport())
                );
        }
        Debug.Log("Presenting");
        yield return new WaitForSeconds(3);
        List<int> allcountries = new List<int>()
        {
            countryRub.unitedKingdom,
            countryRub.portugal,
            countryRub.poland,
            countryRub.greece,
            countryRub.germany,
            countryRub.france,
            countryRub.spain,
            countryRub.sweden
        };

        for (int i = 0; i < segmentsToSearch.Count; i++)
        {
            GameObject obj = Instantiate(listingPrefab, leaderboardPanel.transform);
            LeaderboardListing leaderboardListing = obj.GetComponent<LeaderboardListing>();
            leaderboardListing.positionText.text = (i + 1).ToString();
            leaderboardListing.playerNameText.text = segmentsToSearch.ElementAt(i).Key;
            leaderboardListing.countryText.text = playersInCountry.ElementAt(i).Value.ToString();
            leaderboardListing.rubbishText.text = allcountries[i].ToString();
        }
    }
}

public class CountryRub
{
    public int unitedKingdom;
    public int portugal;
    public int poland;
    public int greece;
    public int germany;
    public int france;
    public int spain;
    public int sweden;
}