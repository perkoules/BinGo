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
            playfabManager.wasteCollected++;
            playfabManager.rubbishInPlace++;
            playfabManager.rubbishInDistrict++;
            playfabManager.rubbishInRegion++;
            playfabManager.rubbishInCountry++;
            playfabManager.coinsAvailable++;
            playfabManager.ProgressLevelCheck();
        }
        else if (option == "r")
        {
            playfabManager.recycleCollected++;
            playfabManager.rubbishInPlace++;
            playfabManager.rubbishInDistrict++;
            playfabManager.rubbishInRegion++;
            playfabManager.rubbishInCountry++;
            playfabManager.coinsAvailable += 2;
            playfabManager.ProgressLevelCheck();
        }
        achievementsController.rubbishToUnlockCounter = playfabManager.wasteCollected;
        achievementsController.recycleToUnlockCounter = playfabManager.recycleCollected;
        playfabManager.UpdatePlayerStats();
        playfabManager.LevelBadgeDisplay();
        StartCoroutine(achievementsController.CheckAchievementUnlockability());
    }
}
