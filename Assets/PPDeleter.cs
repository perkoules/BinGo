using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
}
