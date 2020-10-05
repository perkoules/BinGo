using Mapbox.Geocoding;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using System.Collections;
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
    public List<string> friendList, friendIdList;
    public PlayfabManager playfabManager;
    public TextMeshProUGUI teamnameDisplay;
    public PlayerInfo playerInfo;

    private void Start()
    {
        StartCoroutine(InitializeLocation());
    }

    private IEnumerator InitializeLocation()
    {
        yield return new WaitForSeconds(3);
        GetLocationDataOfRubbish();
        playfabManager.GetPlayerStats();
    }

    public void GetLocationDataOfRubbish()
    {
        Vector2d latlon = locationProvider.CurrentLocation.LatitudeLongitude;
        rubLoc = new GetRubbishLocation(latlon)
        {
            Types = new string[] { "country", "region", "district", "place" } //What features to focus on
        };

        string locationUrl = rubLoc.GetUrl();
        var jsonLocationData = new WebClient().DownloadString(locationUrl);

        MyResult myResult = JsonUtility.FromJson<MyResult>(jsonLocationData);
        /*features.text { type}
        * [0] = Bishop Auckland { place}
        * [1] = Durham { district}
        * [2] = England { region}
        * [3] = United Kingdom { country}*/
        int p = myResult.features.FindIndex(f => f.id.Contains("place"));
        int d = myResult.features.FindIndex(f => f.id.Contains("district"));
        int r = myResult.features.FindIndex(f => f.id.Contains("region"));
        int c = myResult.features.FindIndex(f => f.id.Contains("country"));
        if (p >= 0)
        {
            playerInfo.RubbishPlace = myResult.features[p].text;
        }
        if (d >= 0)
        {
            playerInfo.RubbishDistrict = myResult.features[d].text;
        }
        if (r >= 0)
        {
            playerInfo.RubbishRegion = myResult.features[r].text;
        }
        if (c >= 0)
        {
            playerInfo.RubbishCountry = myResult.features[c].text;
        }
    }
}