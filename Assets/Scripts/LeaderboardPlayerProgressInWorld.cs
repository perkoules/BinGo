using System.Collections;
using UnityEngine;

public class LeaderboardPlayerProgressInWorld : MonoBehaviour
{
    private LeaderboardManager leaderboardManager;
    public GameObject leaderboardHolder, gettingDataMessage;

    private void OnEnable()
    {
        if (leaderboardHolder.transform.childCount == 0)
        {
            leaderboardManager = GetComponent<LeaderboardManager>();
            leaderboardManager.PlayersProgressInWorldAndCities("world");
            StartCoroutine(GettingDataMessage());
        }
        else
        {
            foreach (Transform child in leaderboardHolder.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        foreach (Transform child in leaderboardHolder.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void ClearLeaderboard()
    {
        foreach (Transform child in leaderboardHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator GettingDataMessage()
    {
        gettingDataMessage.SetActive(true);
        yield return new WaitForSeconds(3);
        gettingDataMessage.SetActive(false);
    }
}