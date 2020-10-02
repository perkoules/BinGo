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
            achievementsController.rubbishToUnlockCounter++;
            playfabManager.rubbishCollected++;
            playfabManager.rubbishInPlace++;
            playfabManager.rubbishInDistrict++;
            playfabManager.rubbishInRegion++;
            playfabManager.rubbishInCountry++;
            playfabManager.coinsAvailable++;
            playfabManager.ProgressLevelCheck();
        }
        else if (option == "r")
        {
            achievementsController.recycleToUnlockCounter++;
            playfabManager.rubbishCollected++;
            playfabManager.rubbishInPlace++;
            playfabManager.rubbishInDistrict++;
            playfabManager.rubbishInRegion++;
            playfabManager.rubbishInCountry++;
            playfabManager.coinsAvailable += 2;
            playfabManager.ProgressLevelCheck();
        }
        playfabManager.UpdatePlayerStats();
        playfabManager.LevelBadgeDisplay();
        achievementsController.CheckAchievementUnlockability();
    }
}
