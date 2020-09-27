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
    public PlayfabManager playfabManager;
    public TextMeshProUGUI teamnameDisplay;
    public PlayerInfo playerInfo;

    private void Start()
    {
        StartCoroutine(InitializeLocation());
    }

    IEnumerator InitializeLocation()
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

        //Mock Location url
        //lat 54.640891
        //lon -1.6793837
        /*double lat, lon;
         * string mockLocation = "https://api.mapbox.com/geocoding/v5/mapbox.places/" +
         * lat +"," +lon +
         * ".json?types=country%2Cregion%2Cdistrict%2Cplace&access_token=pk.eyJ1Ijoic" +
         * "GVya291bGVzIiwiYSI6ImNrZTJxcnY3dDBid24ycm1zZHpobmM3bXQifQ.OJPGmxrrojaoLzN_LpjesA";*/
        string locationUrl = rubLoc.GetUrl();
        var jsonLocationData = new WebClient().DownloadString(locationUrl);

        MyResult myResult = JsonUtility.FromJson<MyResult>(jsonLocationData);
        /*features.text { type}
        * [0] = Bishop Auckland { place}
        * [1] = Durham { district}
        * [2] = England { region}
        * [3] = United Kingdom { country}*/
        playerInfo = new PlayerInfo
        {
            rubbishPlace = myResult.features[0].text,
            rubbishDistrict = myResult.features[1].text,
            rubbishRegion = myResult.features[2].text,
            rubbishCountry = myResult.features[3].text
        };
    }
}