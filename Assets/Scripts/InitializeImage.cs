using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerDataSaver))]
public class InitializeImage : MonoBehaviour
{
    public Container flagSelection, avatarSelection;
    private PlayerDataSaver playerDataSaver;
    private Image myImage;

    private void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        myImage = GetComponent<Image>();
    }

    private void Start()
    {
        if (gameObject.name.Contains("Avatar"))
        {
            StartCoroutine(DisplayMyImage(playerDataSaver.GetAvatar()));
        }
        else
        {
            StartCoroutine(DisplayMyImage(playerDataSaver.GetCountry()));
        }
    }

    private IEnumerator DisplayMyImage(string imageToSearch)
    {
        yield return new WaitForSeconds(1f);
        foreach (var img in flagSelection.imageContainer)
        {
            if (img.sprite.name == imageToSearch)
            {
                myImage.sprite = img.sprite;
            }
        }
        foreach (var img in avatarSelection.imageContainer)
        {
            if (img.sprite.name == imageToSearch)
            {
                myImage.sprite = img.sprite;
            }
        }
        StopCoroutine("DisplayMyImage");
    }
}