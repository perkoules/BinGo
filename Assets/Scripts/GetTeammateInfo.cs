using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GetTeammateInfo : MonoBehaviour
{
    public Container flagSelection, avatarSelection;
    public BadgeController badgeController;
    public TeammateInfo teammateInfo;
    public TextMeshProUGUI username;
    public Image country;
    public Image avatar;
    public Image levelBadge;
    public TextMeshProUGUI level;

    public void OnEnable()
    {
        DisplayInfo();
    }

    private void DisplayInfo()
    {
        username.text = teammateInfo.Username;
        country = flagSelection.imageContainer.Find(flag => flag.name == teammateInfo.Country);
        avatar = avatarSelection.imageContainer.Find(flag => flag.name == teammateInfo.Avatar);
        level.text = teammateInfo.Level.ToString();
        levelBadge.sprite = GetLevelBadge(teammateInfo.Level);
    }

    private Sprite GetLevelBadge(int level)
    {
        Sprite spriteToReturn = null;
        foreach (var badge in badgeController.allBadges)
        {
            if(level == Convert.ToInt32(badge.name.Remove(0,10)))
            {
                spriteToReturn = badge.sprite;
            }
        }
        return spriteToReturn;
    }
}
