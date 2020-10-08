using Mapbox.CheapRulerCs;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using UnityEngine;

public class MeasureDistance : MonoBehaviour
{
    public DeviceLocationProvider locationProvider;
    public SpawnBinsOnMap spawnBins;
    private double[] from, to;
    public int binToLookFor = 0;
    public double distance;
    private Vector2d playerCurrentLocation, toBinLocation;

    private void Start()
    {
        InvokeRepeating("GetDistanceToBin", 4.0f, 3.0f);
    }

    private void GetDistanceToBin()
    {
        playerCurrentLocation = locationProvider.CurrentLocation.LatitudeLongitude;
        toBinLocation = spawnBins._locations[binToLookFor];
        from = playerCurrentLocation.ToArray();
        to = toBinLocation.ToArray();
        CheapRuler cr = new CheapRuler(from[0], CheapRulerUnits.Meters);
        distance = cr.Distance(from, to);
    }
}