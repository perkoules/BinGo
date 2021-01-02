using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class PPDeleter : MonoBehaviour
{
    public PlayerDataSaver playerDataSaver;
    public void ResetScav()
    {
        playerDataSaver.SetScavHunt(0);
    }

    public void SetTasks(TMP_InputField str)
    {
        playerDataSaver.SetHuntProgress(str.text);
    }

    public void ResetTree()
    {
        playerDataSaver.SetTreeLocation("-"); 
        string treeLoc = "-";
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>() { { "Tree Location", treeLoc } },
                Permission = UserDataPermission.Public
            },
            result => Debug.Log("Successfully planted a tree at " + treeLoc + " location"),
            error => Debug.Log(error.GenerateErrorReport())); ;
    }
}
