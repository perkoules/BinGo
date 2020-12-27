using UnityEngine;

[CreateAssetMenu(fileName = "Ranking")]
public class LeaderboardSO : ScriptableObject
{
    public GameObject listingPrefab;
    private GameObject leaderboardPanel;

    public bool autoDelete;

    public LeaderboardToGet whichLeaderboard;

    public void GetRankings(LeaderboardToGet which, GameObject panel)
    {
        leaderboardPanel = panel;
        switch (which)
        {
            case LeaderboardToGet.None:
                Debug.Log("Doing Nothing");
                break;

            case LeaderboardToGet.WorldByPlayers:
                GetProgressInWorldByPlayer();
                break;

            case LeaderboardToGet.WorldByTeams:
                GetProgressInWorldByTeam();
                break;

            case LeaderboardToGet.WorldByCountries:
                GetProgressInWorldByCountry();
                break;

            case LeaderboardToGet.PersonalInCities:
                GetProgressInCities();
                break;

            case LeaderboardToGet.PersonalInCountry:
                GetProgressInCountry();
                break;

            case LeaderboardToGet.PersonalInWorld:
                GetProgressInWorld();
                break;

            default:
                break;
        }
    }

    public void GetProgressInCities()
    {
        if (leaderboardPanel.transform.childCount == 0)
        {
            LeaderboardRetriever.Instance.PlayersProgressInWorldAndCities("cities", listingPrefab, leaderboardPanel);
        }
        else
        {
            foreach (Transform child in leaderboardPanel.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void GetProgressInWorld()
    {
        if (leaderboardPanel.transform.childCount == 0)
        {
            LeaderboardRetriever.Instance.PlayersProgressInWorldAndCities("world", listingPrefab, leaderboardPanel);
        }
        else
        {
            foreach (Transform child in leaderboardPanel.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void GetProgressInCountry()
    {
        if (leaderboardPanel.transform.childCount == 0)
        {
            LeaderboardRetriever.Instance.PlayersCountryLeaderboard(listingPrefab, leaderboardPanel);
        }
        else
        {
            foreach (Transform child in leaderboardPanel.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void GetProgressInWorldByPlayer()
    {
        if (leaderboardPanel.transform.childCount == 0)
        {
            LeaderboardRetriever.Instance.WorldLeaderboardForRubbish(listingPrefab, leaderboardPanel);
        }
        else
        {
            foreach (Transform child in leaderboardPanel.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void GetProgressInWorldByTeam()
    {
        if (leaderboardPanel.transform.childCount == 0)
        {
            LeaderboardRetriever.Instance.GetWorldLeaderboardByTeam(listingPrefab, leaderboardPanel);
        }
        else
        {
            foreach (Transform child in leaderboardPanel.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void GetProgressInWorldByCountry()
    {
        if (leaderboardPanel.transform.childCount == 0)
        {
            LeaderboardRetriever.Instance.GetWorldLeaderboardByCountry(listingPrefab, leaderboardPanel);
        }
        else
        {
            foreach (Transform child in leaderboardPanel.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void ClearLeaderboard(GameObject panel)
    {
        foreach (Transform child in panel.transform)
        {
            Destroy(child.gameObject);
        }
    }
}