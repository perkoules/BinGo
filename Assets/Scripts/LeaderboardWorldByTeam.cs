using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardWorldByTeam : MonoBehaviour
{
    private LeaderboardManager leaderboardManager;
    public GameObject leaderboardHolder, gettingDataMessage;

    private void OnEnable()
    {
        leaderboardManager = GetComponent<LeaderboardManager>();
        if (leaderboardHolder.transform.childCount == 0)
        {
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
        yield return new WaitUntil(() => leaderboardHolder.transform.childCount > 0);
        gettingDataMessage.SetActive(false);
    }
}
