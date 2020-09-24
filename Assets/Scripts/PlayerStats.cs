using Mapbox.Examples;
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
    public DeviceLocationProvider locationProvider;
    public LocationResults locationResults;


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
        locationResults = new LocationResults
        {
            place = myResult.features[0].text,
            district = myResult.features[1].text,
            region = myResult.features[2].text,
            country = myResult.features[3].text
        };
    }
   
}
[System.Serializable]
public class LocationResults
{
    public string place;
    public string district;
    public string region;
    public string country;
}