using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsController : MonoBehaviour
{
    public PlayfabManager pfm;
    public List<Image> allBadges;
    public List<bool> toggledBadges;
    
    public int myBadge;

    public static AchievementsController bc;
    private void OnEnable()
    {
        bc = this;
    }
    private void Start()
    {

    }
    
}
