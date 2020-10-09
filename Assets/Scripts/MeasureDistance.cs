using Mapbox.CheapRulerCs;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using System.Linq;
using UnityEngine;

public class MeasureDistance : MonoBehaviour
{
    public DeviceLocationProvider locationProvider;
    public SpawnBinsOnMap spawnBins;
    private double[] from, to;
    public double[] distances;
    private Vector2d playerCurrentLocation, toBinLocation;

    private void Start()
    {
        distances = new double[11];
        InvokeRepeating("GetDistanceToBin", 4.0f, 3.0f);
    }
    private void GetDistanceToBin()
    {
        playerCurrentLocation = locationProvider.CurrentLocation.LatitudeLongitude;
        from = playerCurrentLocation.ToArray();
        CheapRuler cr = new CheapRuler(from[0], CheapRulerUnits.Meters);
        for (int i = 0; i < spawnBins._locations.Length; i++)
        {
            toBinLocation = spawnBins._locations[i];
            to = toBinLocation.ToArray();
            distances[i] = cr.Distance(from, to);
        }
    }
}