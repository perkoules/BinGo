using Michsky.UI.ModernUIPack;
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

    

    public void RegisterTemporaryAccount(Button btn)
    {
        //if user is NOT guest, disable registration button cause he already did it
        if (playerDataSaver.GetIsGuest() == 0) 
        {
            btn.interactable = false;
        }
        else
        {
            btn.interactable = true;
        }
    }


    public void Music(bool isOn)
    {
        musicController.IsMusicOn(isOn);
    }

    public void SFX(bool isOn)
    {
        collectRubbish.isSfxOn = isOn;
    }

    public void Vibration(bool isOn)
    {
        collectRubbish.isVibrationOn = isOn;
    }

}