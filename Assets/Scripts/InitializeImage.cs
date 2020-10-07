﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerDataSaver))]
public class InitializeImage : MonoBehaviour
{
    public Container flagSelection, avatarSelection, levelBadgeSelection;
    private PlayerDataSaver playerDataSaver;
    private Image myImage;

    private void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        myImage = GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(DisplayMyImage(gameObject.name));
    }

    private IEnumerator DisplayMyImage(string imageToSearch)
    {
        yield return new WaitForSeconds(0.5f);
        switch (imageToSearch)
        {
            case "PlayerAvatar":
                foreach (var img in avatarSelection.imageContainer)
                {
                    if (img.sprite.name == playerDataSaver.GetAvatar())
                    {
                        myImage.sprite = img.sprite;
                    }
                }
                break;

            case "Flag":
                foreach (var img in flagSelection.imageContainer)
                {
                    if (img.sprite.name == playerDataSaver.GetCountry())
                    {
                        myImage.sprite = img.sprite;
                    }
                }
                break;

            case "Badge":
                foreach (var img in levelBadgeSelection.imageContainer)
                {
                    string imgObj = img.name.Remove(0, 10);
                    if (imgObj == playerDataSaver.GetProgressLevel().ToString())
                    {
                        myImage.sprite = img.sprite;
                    }
                }
                break;

            default:
                break;
        }
        StopCoroutine("DisplayMyImage");
    }
}