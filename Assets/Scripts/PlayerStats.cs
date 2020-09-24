using Mapbox.Examples;
using Mapbox.Geocoding;
using Mapbox.Unity.Location;
using Mapbox.Utils;
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
    public List<TextMeshProUGUI> voucherTextDisplay;
    public List<Image> flagImageDisplay;
    public List<Image> avatarImageDisplay;
    public List<Image> lvlBadgeDisplay;
    public TextMeshProUGUI teamnameDisplay;
    public FlagSelection flagSelection, avatarSelection;

    /*-------------  ----------------*/
    public BadgeController badgeController;
    public AchievementsController achievementsController;
    public PlayfabManager playfabManager;

    public GetRubbishLocation rubLoc;
    public DeviceLocationProvider dlp;
    public TextMeshProUGUI myText;
    public void GetLoc()
    {
        Vector2d latlon = dlp.CurrentLocation.LatitudeLongitude;
        rubLoc = new GetRubbishLocation(latlon);
        rubLoc.Types = new string[] { "country" };

        string str = rubLoc.GetUrl();
        Debug.Log(str);
        /*MyClass myObject = new MyClass();
        myObject.level = 1;
        myObject.timeElapsed = 47.5f;
        myObject.playerName = "Dr Charles Francis";*/
    }
}
/*[Serializable]
public class MyClass
{
    public int level;
    public float timeElapsed;
    public string playerName;
}*/