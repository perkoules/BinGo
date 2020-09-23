using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgeController : MonoBehaviour
{
    public PlayfabManager pfm;
    public List<Image> allBadges;
    public List<bool> toggledBadges;
    
    public int myBadge;

    public static BadgeController bc;
    private void OnEnable()
    {
        bc = this;
    }
    private void Start()
    {
        myBadge = pfm.progressLevel;
        StartCoroutine(UpdateAchievements());
    }
    public void BadgeToData()
    {
        for (int i = 0; i < allBadges.Count; i++)
        {
            if (i < myBadge)
            {
                toggledBadges[i] = true;
                allBadges[i].color = Color.white;
            }
            else
            {
                toggledBadges[i] = false;
                allBadges[i].color = Color.black;
            }
        }
    }

    public string BadgeDataToString()
    {
        string toString = "";
        for (int i = 0; i < toggledBadges.Count; i++)
        {
            if(toggledBadges[i] == true)
            {
                toString += "1";
            }
            else
            {
                toString += "0";
            }
        }
        return toString;
    }
       
    IEnumerator UpdateAchievements()
    {
        BadgeToData();
        yield return new WaitForSeconds(2);
        pfm.SetPlayerData();        
    }
}
