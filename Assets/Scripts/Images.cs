using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Images : MonoBehaviour
{
    public Container flagSelection, avatarSelection, levelBadgeSelection;
    public Image tm1, tm2, tm3;
    public List<Image> playerAvatar;
    public List<Image> flag;
    public List<Image> badge;

    private void Awake()
    {
        PlayfabManager.OnImageAdjusted += AdjustImages;
    }

    private void AdjustImages(string player, string country, int level)
    {
        var av = avatarSelection.imageContainer.Find(img => img.sprite.name == player);
        playerAvatar.ForEach(img => img.sprite = av.sprite);
        var fl = flagSelection.imageContainer.Find(img => img.sprite.name == country);
        flag.ForEach(img => img.sprite = fl.sprite);
        var bdg = levelBadgeSelection.imageContainer.Find(img => img.name.Remove(0, 6) == level.ToString());
        badge.ForEach(img => img.sprite = bdg.sprite);
    }
}
