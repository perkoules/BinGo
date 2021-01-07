using Mapbox.Geocoding;
using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rankings : MonoBehaviour
{
    public WindowManager windowManager;
    public NotificationManager info;
    public LeaderboardSO leaderboard;
    public GameObject leaderboardPanel;
    public RectTransform messagePanel;

    public void RetrieveLeaderboard()
    {
        leaderboard.GetRankings(leaderboard.whichLeaderboard, leaderboardPanel, messagePanel);
    }

    public void ResyncLeaderboard()
    {
        StartCoroutine(LeaderboardSync());
    }

    private IEnumerator LeaderboardSync()
    {
        leaderboard.ClearLeaderboard(leaderboardPanel);
        yield return new WaitForSeconds(1);
        leaderboard.GetRankings(leaderboard.whichLeaderboard, leaderboardPanel, messagePanel);
    }
    /// <summary>
    /// Triggered by button
    /// </summary>
    /// <param name="transform"></param>
    public void ShowLeaderboardInfo()
    {
        info.OpenNotification();
    }
}
