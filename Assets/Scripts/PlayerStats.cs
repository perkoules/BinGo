using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public List<TextMeshProUGUI> rubbishTextDisplay;
    public List<TextMeshProUGUI> levelTextDisplay;
    public List<TextMeshProUGUI> usernameTextDisplay;
    public List<TextMeshProUGUI> coinsTextDisplay;
    public List<Image> flagImageDisplay;
    public List<Image> avatarImageDisplay;
    public List<Image> lvlBadgeDisplay;
    public TextMeshProUGUI teamnameDisplay;
    public FlagSelection flagSelection, avatarSelection;

    /*-------------  ----------------*/
    public BadgeController badgeController;
    public AchievementsController achievementsController;
    public PlayfabManager playfabManager;
}
