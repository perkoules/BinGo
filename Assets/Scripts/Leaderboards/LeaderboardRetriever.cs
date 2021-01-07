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

public class LeaderboardRetriever : MonoBehaviour
{
    public List<string> allPlayers;
    private string playerID, playerName;
    public Trictionary idTeamnameRubbish;
    public static LeaderboardRetriever Instance { get; private set; }

    public delegate void DataRetrieved();

    public static event DataRetrieved OnDataRetrieved;

    private void OnEnable()
    {
        if (Instance != null & Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GetPlayerID();
        GetAllPlayers();
    }

    public void GetAllPlayers()
    {
        PlayFabAdminAPI.GetPlayersInSegment(
                    new GetPlayersInSegmentRequest { SegmentId = "CAD8FCF4CF87AD8E" },
                    result =>
                    {
                        foreach (var item in result.PlayerProfiles)
                        {
                            if (!allPlayers.Contains(item.PlayerId))
                            {
                                allPlayers.Add(item.PlayerId);
                            }
                        }
                    },
                    error => Debug.LogError(error.GenerateErrorReport()));
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

    public void WorldLeaderboardForRubbish(GameObject listingPrefab, GameObject leaderboardPanel)
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
        OnDataRetrieved();
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

    public void PlayersCountryLeaderboard(GameObject listingPrefab, GameObject leaderboardPanel)
    {
        PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest() { PlayFabId = playerID },
        result =>
        {
            if (result.Data.ContainsKey("Country"))
            {
                StartCoroutine(PlayersCountryLeaderboardResults(result.Data["Country"].Value + " isCountry", listingPrefab, leaderboardPanel));
            }
        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }

    private IEnumerator PlayersCountryLeaderboardResults(string playerCountry, GameObject listingPrefab, GameObject leaderboardPanel)
    {
        yield return new WaitForSeconds(0.2f);
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
                leaderboardListing.rubbishText.text = "----";  // playerInfo.PlayerRubbish.ToString();
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
        },
        error => Debug.LogError(error.GenerateErrorReport()));
        OnDataRetrieved?.Invoke();
    }

    public void PlayersProgressInWorldAndCities(string whatToLookFor, GameObject listingPrefab, GameObject leaderboardPanel)
    {
        var getPlayerStatisticNames = new GetPlayerStatisticsRequest { };
        PlayFabClientAPI.GetPlayerStatistics(getPlayerStatisticNames, result =>
        {
            foreach (var stat in result.Statistics)
            {
                if (stat.StatisticName.Contains(" isPlace"))
                {
                    string place = stat.StatisticName.Replace(" isPlace", "");
                    if (!string.IsNullOrEmpty(place))
                    {
                        StartCoroutine(PlayersProgressInWorldAndCitiesResults(stat.StatisticName, place, whatToLookFor, listingPrefab, leaderboardPanel));
                    }
                }
            }
        },
        error => Debug.LogError(error.GenerateErrorReport()));
        OnDataRetrieved?.Invoke();
    }

    private IEnumerator PlayersProgressInWorldAndCitiesResults(string statName, string place, string whatToLookFor, GameObject listingPrefab, GameObject leaderboardPanel)
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
        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }

    public string GetCountryFromPlace(string toSearch)
    {
        string country = "";
        ForwardGeocodeResource fgr = new ForwardGeocodeResource(toSearch) { };
        string locationUrl = fgr.GetUrl();
        try
        {
            var jsonLocationData = new WebClient().DownloadString(locationUrl);
            MyResult myResult = JsonUtility.FromJson<MyResult>(jsonLocationData);
            string[] tt = myResult.features[0].place_name.Split(',');
            country = tt.Last();
        }
        catch (WebException we)
        {
            HttpWebResponse errorResponse = we.Response as HttpWebResponse;
            if (errorResponse.StatusCode == HttpStatusCode.NotFound)
            {
                OnDataRetrieved?.Invoke();
            }
        }
        return country;
    }

    #region Teams And Countries

    public void GetWorldLeaderboardByCountry(GameObject listingPrefab, GameObject leaderboardPanel)
    {
        StartCoroutine(GetWorldLeaderboardByCountryCoroutine(listingPrefab, leaderboardPanel));
    }

    public IEnumerator GetWorldLeaderboardByCountryCoroutine(GameObject listingPrefab, GameObject leaderboardPanel)
    {
        yield return new WaitForSeconds(2);
        Dictionary<string, string> idCountry = new Dictionary<string, string>();

        foreach (var id in allPlayers)
        {
            PlayFabClientAPI.GetUserData(
                new PlayFab.ClientModels.GetUserDataRequest { PlayFabId = id },
                result =>
                {
                    idCountry.Add(id, result.Data["Country"].Value);
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
                if (k.Key == orderCountryRubbish.ElementAt(i).Key)
                {
                    val = k.Value;
                }
            }

            leaderboardListing.countryText.text = val.ToString();
            leaderboardListing.rubbishText.text = orderCountryRubbish.ElementAt(i).Value.ToString();
        }
        OnDataRetrieved?.Invoke();        
    }

    public void GetWorldLeaderboardByTeam(GameObject listingPrefab, GameObject leaderboardPanel)
    {
        StartCoroutine(GetWorldLeaderboardByTeamCoroutine(listingPrefab, leaderboardPanel));
    }

    public IEnumerator GetWorldLeaderboardByTeamCoroutine(GameObject listingPrefab, GameObject leaderboardPanel)
    {
        idTeamnameRubbish = new Trictionary();
        yield return new WaitForSeconds(6);
        foreach (var id in allPlayers)
        {
            PlayFabClientAPI.GetUserData(
                new PlayFab.ClientModels.GetUserDataRequest { PlayFabId = id },
                result =>
                {
                    idTeamnameRubbish[id] = new TeamNameRubbish
                    {
                        Value1 = result.Data["TeamName"].Value
                    };
                },
                error => Debug.LogError(error.GenerateErrorReport()));
        }
        yield return new WaitForSeconds(4f);
        Dictionary<string, int> idRubbish = new Dictionary<string, int>();
        foreach (var id in idTeamnameRubbish.Keys)
        {
            PlayFabClientAPI.GetLeaderboard(
            new GetLeaderboardRequest { StatisticName = "RubbishCollected" },
            result =>
            {
                foreach (var ldb in result.Leaderboard)
                {
                    if (ldb.PlayFabId == id)
                    {
                        idTeamnameRubbish[id] = new TeamNameRubbish
                        {
                            Value1 = idTeamnameRubbish[id].Value1,
                            Value2 = ldb.StatValue
                        };
                    }
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
        }
        yield return new WaitForSeconds(2f);
        Dictionary<string, int> results = new Dictionary<string, int>();
        foreach (var item in idTeamnameRubbish)
        {
            if (!results.ContainsKey(item.Value.Value1))
            {
                results.Add(item.Value.Value1, item.Value.Value2);
            }
            else
            {
                results[item.Value.Value1] += item.Value.Value2;
            }
        }
        var orderResults = results.OrderByDescending(key => key.Value);

        for (int i = 0; i < orderResults.Count(); i++)
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
            leaderboardListing.playerNameText.text = orderResults.ElementAt(i).Key;
            leaderboardListing.rubbishText.text = orderResults.ElementAt(i).Value.ToString();
        }
        OnDataRetrieved?.Invoke();
    }

    #endregion Teams And Countries
}