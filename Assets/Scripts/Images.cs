using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Images : MonoBehaviour
{
    public Sprite anonymous;
    public Image tm1, tm2, tm3;
    public List<Sprite> flagContainer, avatarContainer, levelBadgeContainer;
    public List<Image> playerAvatar;
    public List<Image> flag;
    public List<Image> badge;

    private void Start()
    {
        PlayfabManager.OnImageAdjusted += AdjustImages;
    }

    private void AdjustImages(string player, string country, int level)
    {
        StartCoroutine(Delay(player, country, level));
    }

    private IEnumerator Delay(string player, string country, int level)
    {
        var av = avatarContainer.Find(spr => spr.name == player);
        var fl = flagContainer.Find(spr => spr.name == country);
        var bdg = levelBadgeContainer.Find(spr => spr.name == level.ToString());
        yield return new WaitForSeconds(2);
        playerAvatar.ForEach(img => img.sprite = av);
        flag.ForEach(img => img.sprite = fl);
        badge.ForEach(img => img.sprite = bdg);
    }
}
