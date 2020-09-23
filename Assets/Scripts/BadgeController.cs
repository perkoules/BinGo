using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgeController : MonoBehaviour
{
    public PlayfabManager pfm;
    public List<Image> allBadges;
    
    public int myBadge;

    public static BadgeController bc;
    private void OnEnable()
    {
        bc = this;
    }
    private void Start()
    {
        myBadge = pfm.progressLevel;
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
