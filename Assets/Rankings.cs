using Mapbox.Geocoding;
using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rankings : MonoBehaviour
{
    public WindowManager windowManager;
    public LeaderboardSO leaderboard;
    public GameObject leaderboardPanel;

    public void RetrieveLeaderboard()
    {
        leaderboard.GetRankings(leaderboard.whichLeaderboard, leaderboardPanel);
    }

    public void TriggerAutodeletion()
    {
        if (leaderboard.autoDelete)
        {
            leaderboard.ClearLeaderboard(leaderboardPanel);
        }
    }

}
