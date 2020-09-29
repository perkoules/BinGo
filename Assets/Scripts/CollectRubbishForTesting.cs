using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectRubbishForTesting : MonoBehaviour
{
    public AchievementsController achievementsController;
    public PlayerStats playerStats;
    public PlayfabManager playfabManager;
    
    public void SetRubbishCollection(string option)
    {
        playerStats.GetLocationDataOfRubbish();

        if (option == "c")
        {
            playfabManager.rubbishCollected++;
            playfabManager.rubbishInPlace++;
            playfabManager.rubbishInDistrict++;
            playfabManager.rubbishInRegion++;
            playfabManager.rubbishInCountry++;
            playfabManager.coinsAvailable++;
        }
        else if (option == "r")
        {
            playfabManager.rubbishCollected++;
            playfabManager.rubbishInPlace++;
            playfabManager.rubbishInDistrict++;
            playfabManager.rubbishInRegion++;
            playfabManager.rubbishInCountry++;
            playfabManager.coinsAvailable += 2;
        }
        playfabManager.UpdatePlayerStats();
        achievementsController.CheckAchievementUnlockability();
    }
}
