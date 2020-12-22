using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerDataSaver))]
public class SettingsController : MonoBehaviour
{
    public CollectRubbish collectRubbish;
    public MusicController musicController;
    private PlayerDataSaver playerDataSaver;

    private void OnEnable()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
    }

    

    public void RegisterTemporaryAccount()
    {
        /*int index = toggles.FindIndex(t => t.name.Contains("Register") == true);
        if (playerDataSaver.GetIsGuest() == 0) //if user is NOT guest, disable registration button cause he already did it
        {
            toggles[index].image.color = enabledColor;
            toggles[index].interactable = false;
        }
        else
        {
            toggles[index].image.color = Color.red;
        }*/
    }


    public void Music(bool isOn)
    {
        Debug.Log("Music = " + isOn);
        musicController.IsMusicOn(isOn);
    }

    public void SFX(bool isOn)
    {
        Debug.Log("SFX = " + isOn);
        collectRubbish.isSfxOn = isOn;
    }

    public void Vibration(bool isOn)
    {
        Debug.Log("Vibration = " + isOn);
        collectRubbish.isVibrationOn = isOn;
    }

}