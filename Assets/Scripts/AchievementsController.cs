using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsController : MonoBehaviour
{
    public PlayfabManager pfm;
    public List<Image> allBadges;
    public TaskController taskController;
    private string tasks;

    public static AchievementsController bc;

    private void OnEnable()
    {
        bc = this;
        tasks = pfm.GetTasks();
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
        result => Debug.Log("Successfully updated user data"),
        error => Debug.Log(error.GenerateErrorReport()));
    }

    public void CheckAchievementUnlockability()
    {
        //Debug.Log("Checking");
    }
}