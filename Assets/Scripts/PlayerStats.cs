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
    public DeviceLocationProvider dlp;
    public void GetLoc()
    {
        Vector2d latlon = dlp.CurrentLocation.LatitudeLongitude;
        rubLoc = new GetRubbishLocation(latlon)
        {
            Types = new string[] { "country" }
        };

        string str = rubLoc.GetUrl();

        var json = new WebClient().DownloadString(str);
        Debug.Log(json);

        MyResult myResult = JsonUtility.FromJson<MyResult>(json);
        Debug.Log(myResult.features[0].place_name);
    }
}

/*---- URL Result ----*/
[System.Serializable]
public class MyResult
{
    public string type;
    public List<double> query;
    public List<Features> features;
    public string attribution;
}
[System.Serializable]
public class Features
{
    public string id;
    public string type;
    public List<string> place_type;
    public int relevance;
    public List<Properties> properties;
    public string text;
    public string place_name;
    public List<double> bbox;
    public List<double> center;
    public List<Geometry> geometry;
}
[System.Serializable]
public class Properties
{
    public string wikidata;
    public string short_code;
}
[System.Serializable]
public class Geometry
{
    public string type;
    public List<double> coordinates;
}