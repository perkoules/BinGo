using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Images : MonoBehaviour
{
    public Sprite anonymous;
    public Image tm1, tm2, tm3;
    public List<Sprite> flagContainer, avatarContainer, levelBadgeContainer;
    public List<Image> playerAvatar;
    public List<Image> flag;
    public List<Image> badge;

    
    public Sprite av, fl, bdg;
    private void Start()
    {
        PlayfabManager.OnImageAdjusted += AdjustImages;        
    }

    private void AdjustImages(string player, string country, int level)
    {
        av = avatarContainer.Find(spr => spr.name == player);
        fl = flagContainer.Find(spr => spr.name == country);
        bdg = levelBadgeContainer.Find(spr => spr.name == level.ToString());
        StartCoroutine(Delay(av, fl, bdg));
    }


    private IEnumerator Delay(Sprite player, Sprite country, Sprite level)
    {
        yield return new WaitForSeconds(1);
        playerAvatar.ForEach(img => img.sprite = player);
        flag.ForEach(img => img.sprite = country);
        badge.ForEach(img => img.sprite = level);
    }
}
