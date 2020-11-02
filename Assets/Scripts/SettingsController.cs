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
    public Color32 enabledColor;
    public List<Toggle> toggles;

    private void OnEnable()
    {
        foreach (Toggle tog in FindObjectsOfType<Toggle>())
        {
            toggles.Add(tog);
        }
        playerDataSaver = GetComponent<PlayerDataSaver>();
        Initialization();
    }

    private void Initialization()
    {
        if (playerDataSaver.GetIsGuest() == 0) //if user is NOT guest, disable registration button cause he already did it
        {
            int index = toggles.FindIndex(t => t.name.Contains("Register") == true);
            toggles[index].image.color = enabledColor;
            toggles[index].interactable = false;
        }
        int indexMusic = toggles.FindIndex(t => t.name.Contains("Music") == true);
        toggles[indexMusic].image.color = enabledColor;
        toggles[indexMusic].isOn = true;
        int indexSfx = toggles.FindIndex(t => t.name.Contains("SFX") == true);
        toggles[indexSfx].image.color = enabledColor;
        toggles[indexSfx].isOn = true;
        int indexVibration = toggles.FindIndex(t => t.name.Contains("Vibration") == true);
        toggles[indexVibration].image.color = enabledColor;
        toggles[indexVibration].isOn = true;
    }

    public void DisableToggles()
    {
        throw new NotImplementedException();
    }

    public void FriendsProgress()
    {
        //if done
        int index = toggles.FindIndex(t => t.name.Contains("Friends") == true);
        if (!toggles[index].isOn)
        {
            toggles[index].image.color = Color.red;
        }
        else
        {
            toggles[index].image.color = enabledColor;
        }
    }

    public void Mountain()
    {
        //if done
        int index = toggles.FindIndex(t => t.name.Contains("Mountain") == true);
        if (!toggles[index].isOn)
        {
            toggles[index].image.color = Color.red;
        }
        else
        {
            toggles[index].image.color = enabledColor;
        }
    }

    public void Sea()
    {
        //if done
        int index = toggles.FindIndex(t => t.name.Contains("Sea") == true);
        if (!toggles[index].isOn)
        {
            toggles[index].image.color = Color.red;
        }
        else
        {
            toggles[index].image.color = enabledColor;
        }
    }

    public void RegisterTemporaryAccount()
    {
        int index = toggles.FindIndex(t => t.name.Contains("Register") == true);
        if (playerDataSaver.GetIsGuest() == 0) //if user is NOT guest, disable registration button cause he already did it
        {
            toggles[index].image.color = enabledColor;
            toggles[index].interactable = false;
        }
        else
        {
            toggles[index].image.color = Color.red;
        }
    }

    public void GoogleLink()
    {
        //if done
        int index = toggles.FindIndex(t => t.name.Contains("Google") == true);
        if (!toggles[index].isOn)
        {
            toggles[index].image.color = Color.red;
        }
        else
        {
            toggles[index].image.color = enabledColor;
        }
    }

    public void FacebookLink()
    {
        //if done
        int index = toggles.FindIndex(t => t.name.Contains("Facebook") == true);
        if (!toggles[index].isOn)
        {
            toggles[index].image.color = Color.red;
        }
        else
        {
            toggles[index].image.color = enabledColor;
        }
    }

    public void Music()
    {
        //if done
        int index = toggles.FindIndex(t => t.name.Contains("Music") == true);
        if (!toggles[index].isOn)
        {
            toggles[index].image.color = Color.red;
            musicController.IsMusicOn(false);
        }
        else
        {
            toggles[index].image.color = enabledColor;
            musicController.IsMusicOn(true);
        }
    }

    public void SFX()
    {
        //if done
        int index = toggles.FindIndex(t => t.name.Contains("SFX") == true);
        if (!toggles[index].isOn)
        {
            toggles[index].image.color = Color.red;
            collectRubbish.isSfxOn = false;
        }
        else
        {
            toggles[index].image.color = enabledColor;
            collectRubbish.isSfxOn = true;
        }
    }

    public void Vibration()
    {
        //if done
        int index = toggles.FindIndex(t => t.name.Contains("Vibration") == true);
        if (!toggles[index].isOn)
        {
            toggles[index].image.color = Color.red;
            collectRubbish.isVibrationOn = false;
        }
        else
        {
            toggles[index].image.color = enabledColor;
            collectRubbish.isVibrationOn = true;
        }
    }

    public void Progress()
    {
        //if done
        int index = toggles.FindIndex(t => t.name.Contains("Progress") == true);
        if (!toggles[index].isOn)
        {
            toggles[index].image.color = Color.red;
        }
        else
        {
            toggles[index].image.color = enabledColor;
        }
    }
}