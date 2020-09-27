using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetCountry : MonoBehaviour
{
    private PlayerPrefManager playerPrefs;
    private PlayerInfo playerInfo;
    private TMP_Dropdown dropdown;
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(Set);
    }

    private void Set(int arg0)
    {
        if (dropdown.name.Contains("Country"))
        {
            playerPrefs.SetCountry(dropdown.captionText.text);
            playerInfo.playerCountry.name = playerPrefs.GetCountry();
        }
        else
        {
            playerPrefs.SetAvatar(dropdown.captionText.text);
            playerInfo.playerAvatar.name = playerPrefs.GetAvatar();
        }
    }

}
