using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerDataSaver))]
public class BadgeController : MonoBehaviour
{
    public PlayfabManager pfm;
    public List<Image> allBadges;
    private PlayerDataSaver playerDataSaver;
    public int myBadge;

    public static BadgeController bc;

    private void OnEnable()
    {
        bc = this;
    }

    private void Start()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        myBadge = playerDataSaver.GetProgressLevel();
        BadgeToData();
    }

    public void BadgeToData()
    {
        for (int i = 0; i < allBadges.Count; i++)
        {
            if (i < myBadge)
            {
                allBadges[i].color = Color.white;
            }
            else
            {
                allBadges[i].color = Color.black;
            }
        }
    }
}