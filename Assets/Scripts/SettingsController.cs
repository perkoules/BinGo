using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Color32 enabledColor;
    public List<Toggle> toggles;

    private void OnEnable()
    {
        foreach (Toggle tog in FindObjectsOfType<Toggle>())
        {
            toggles.Add(tog);
        }
        if (FindObjectOfType<PlayfabManager>().GetGuestPlayerRegistered() == "YES")
        {
            int index = toggles.FindIndex(t => t.name.Contains("Register") == true);
            toggles[index].image.color = enabledColor;
            toggles[index].interactable = false;
        }
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
        //if done
        int index = toggles.FindIndex(t => t.name.Contains("Register") == true);
        if (!toggles[index].isOn)
        {
            toggles[index].image.color = Color.red;
        }
        else
        {
            toggles[index].image.color = enabledColor;
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
        }
        else
        {
            toggles[index].image.color = enabledColor;
        }
    }
    public void SFX()
    {
        //if done
        int index = toggles.FindIndex(t => t.name.Contains("SFX") == true);
        if (!toggles[index].isOn)
        {
            toggles[index].image.color = Color.red;
        }
        else
        {
            toggles[index].image.color = enabledColor;
        }
    }
    public void Vibration()
    {
        //if done
        int index = toggles.FindIndex(t => t.name.Contains("Vibration") == true);
        if (!toggles[index].isOn)
        {
            toggles[index].image.color = Color.red;
        }
        else
        {
            toggles[index].image.color = enabledColor;
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
