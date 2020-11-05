using Mapbox.Geocoding;
using PlayFab;
using PlayFab.AdminModels;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboards : MonoBehaviour
{
    public ForwardGeocodeResource fgr;
    public GameObject leaderboardHolderTeam, listingPrefabTeam;
    public GameObject leaderboardHolder, listingPrefab;
    public Color32 evenColor, oddColor;
    public List<string> allPlayers;

    public static Leaderboards Instance { get; private set; }


    public Trictionary idTeamnameRubbish;

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
    }

    private void Start()
    {
        PlayFabAdminAPI.GetPlayersInSegment(
            new GetPlayersInSegmentRequest { SegmentId = "CAD8FCF4CF87AD8E" },
            result =>
            {
                foreach (var item in result.PlayerProfiles)
                {
                    allPlayers.Add(item.PlayerId);
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }

    public IEnumerator GetWorldLeaderboardByCountry()
    {
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
            GameObject obj = Instantiate(listingPrefab, leaderboardHolder.transform);
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

        StartCoroutine(GetWorldLeaderboardByTeam());
    }

    
    public IEnumerator GetWorldLeaderboardByTeam()
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
            GameObject obj = Instantiate(listingPrefabTeam, leaderboardHolderTeam.transform);
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
    }
}

public struct TeamNameRubbish
{
    public string Value1;
    public int Value2;
}
public class Trictionary : Dictionary<string, TeamNameRubbish>
{
    public void Add(string key, string teamname, int rubbishCollected)
    {
        TeamNameRubbish t;
        t.Value1 = teamname;
        t.Value2 = rubbishCollected;
        Add(key, t);
    }
}