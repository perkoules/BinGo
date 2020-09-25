using Mapbox.Geocoding;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public AchievementsController achievementsController;
    public BadgeController badgeController;
    public DeviceLocationProvider locationProvider;
    public FlagSelection flagSelection, avatarSelection;
    public GetRubbishLocation rubLoc;
    public List<Image> avatarImageDisplay;
    public List<Image> flagImageDisplay;
    public List<Image> lvlBadgeDisplay;
    public List<TextMeshProUGUI> coinsTextDisplay;
    public List<TextMeshProUGUI> levelTextDisplay;
    public List<TextMeshProUGUI> rubbishTextDisplay;
    public List<TextMeshProUGUI> usernameTextDisplay;
    public List<TextMeshProUGUI> voucherTextDisplay;
    public PlayfabManager playfabManager;
    public TextMeshProUGUI teamnameDisplay;
    public PlayerInfo playerInfo;

    public void GetLocationData()
    {
        Vector2d latlon = locationProvider.CurrentLocation.LatitudeLongitude;
        rubLoc = new GetRubbishLocation(latlon)
        {
            Types = new string[] { "country", "region", "district", "place" }
        };

        string str = rubLoc.GetUrl();
        var json = new WebClient().DownloadString(str);

        MyResult myResult = JsonUtility.FromJson<MyResult>(json);
        /* features.text {type}
         * [0] = Bishop Auckland {place}
         * [1] = Durham {district}
         * [2] = England {region}
         * [3] = United Kingdom {country}*/
        /*locationResults = new LocationResults
        {
            place = myResult.features[0].text,
            district = myResult.features[1].text,
            region = myResult.features[2].text,
            country = myResult.features[3].text
        };*/
        playerInfo = new PlayerInfo
        {
            rubbishPlace = myResult.features[0].text,
            rubbishDistrict = myResult.features[1].text,
            rubbishRegion = myResult.features[2].text,
            rubbishCountry = myResult.features[3].text
        };
    }
}