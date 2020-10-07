using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerDataSaver))]
public class AchievementsController : MonoBehaviour
{
    public int rubbishToUnlockCounter = 0;
    public int recycleToUnlockCounter = 0;
    public int cityToUnlockCounter = 0;
    public int statesToUnlockCounter = 0;
    public int countryToUnlockCounter = 0;
    public int mountainToUnlockCounter = 0;
    public int seaToUnlockCounter = 0;
    public int continentsToUnlockCounter = 0;
    public List<Image> allBadges;
    private string tasks;
    private PlayerDataSaver playerDataSaver;

    private void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        rubbishToUnlockCounter = playerDataSaver.GetWasteCollected();
        recycleToUnlockCounter = playerDataSaver.GetRecycleCollected();
        tasks = playerDataSaver.GetTasks();
        CheckForLocations();
        GetAchievementsFromData();
    }

    public void GetAchievementsFromData()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            if (int.Parse(tasks[i].ToString()) > 0)
            {
                allBadges[i].color = Color.white;
            }
            else
            {
                allBadges[i].color = Color.black;
            }
        }
    }

    public void UpdateAchievements(string newAchievements)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() { { "Achievements", newAchievements } }
        },
        result => Debug.Log("Successfully updated Achievements with " + newAchievements),
        error => Debug.Log(error.GenerateErrorReport()));
    }

    public IEnumerator CheckAchievementUnlockability()
    {
        CheckForLocations();
        yield return new WaitForSeconds(2);
        TaskChanger();
        UpdateAchievements(TaskUpdater());
    }

    private string TaskUpdater()
    {
        string unlockedAchievements = "";
        for (int i = 0; i < allBadges.Count; i++)
        {
            if (allBadges[i].color == Color.white)
            {
                unlockedAchievements += "1";
            }
            else
            {
                unlockedAchievements += 0;
            }
        }
        return unlockedAchievements;
    }

    private void TaskChanger()
    {
        Dictionary<string, int> searchTypes = new Dictionary<string, int>()
        {
            {"recycled", recycleToUnlockCounter },
            {"city", cityToUnlockCounter },
            {"rubbish", rubbishToUnlockCounter },
            {"country", countryToUnlockCounter },
            {"state", statesToUnlockCounter }
        };
        foreach (var search in searchTypes)
        {
            for (int i = 0; i < allBadges.Count; i++)
            {
                if (allBadges[i].sprite.name.Contains(search.Key))
                {
                    if (search.Value >= 1 && allBadges[i].sprite.name.EndsWith("1"))
                    {
                        allBadges[i].color = Color.white;
                    }
                    if (search.Value >= 5 && allBadges[i].sprite.name.EndsWith("5"))
                    {
                        allBadges[i].color = Color.white;
                    }
                    if (search.Value >= 10 && allBadges[i].sprite.name.EndsWith("10"))
                    {
                        allBadges[i].color = Color.white;
                    }
                    if (search.Value >= 50 && allBadges[i].sprite.name.EndsWith("50"))
                    {
                        allBadges[i].color = Color.white;
                    }
                    if (search.Value >= 100 && allBadges[i].sprite.name.EndsWith("100"))
                    {
                        allBadges[i].color = Color.white;
                    }
                    if (search.Value >= 500 && allBadges[i].sprite.name.EndsWith("500"))
                    {
                        allBadges[i].color = Color.white;
                    }
                    if (search.Value >= 1000 && allBadges[i].sprite.name.EndsWith("1000"))
                    {
                        allBadges[i].color = Color.white;
                    }
                    if (search.Value >= 5000 && allBadges[i].sprite.name.EndsWith("5000"))
                    {
                        allBadges[i].color = Color.white;
                    }
                }
            }
        }
    }

    public void CheckForLocations()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest() { },
            result =>
            {
                int city = 0;
                int country = 0;
                int state = 0;
                foreach (var st in result.Statistics)
                {
                    if (st.StatisticName.Contains(" isPlace"))
                    {
                        city++;
                    }
                    else if (st.StatisticName.Contains(" isCountry"))
                    {
                        country++;
                    }
                    else if (st.StatisticName.Contains(" isDistrict"))
                    {
                        state++;
                    }
                }
                cityToUnlockCounter = city;
                countryToUnlockCounter = country;
                statesToUnlockCounter = state;
            },
            error => Debug.LogError(error.GenerateErrorReport())
            );
    }
}