using Mapbox.CheapRulerCs;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using System;
using System.Linq;
using UnityEngine;

public class MeasureDistance : MonoBehaviour
{
    public DeviceLocationProvider locationProvider;
    public SpawnBinsOnMap spawnBins;
    private double[] from, to;
    public double[] distances;
    public int minIndex = -1;
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
        for (int i = 0; i < spawnBins.locations.Length; i++)
        {
            spawnBins.spawnedObjects[i].GetComponentInChildren<MeshRenderer>().material.color = Color.black;
            toBinLocation = spawnBins.locations[i];
            to = toBinLocation.ToArray();
            distances[i] = cr.Distance(from, to);
        }
        minIndex = Array.IndexOf(distances, distances.Min());

        spawnBins.spawnedObjects[minIndex].GetComponentInChildren<MeshRenderer>().material.color = Color.green;

        //minIndex = GetIndexOfArray(distances, distances.Min());
    }

    public int GetIndexOfArray(double[] distArray, double whatToSearch)
    {
        if (distArray == null)
        {
            throw new ArgumentNullException("Cannot be null");
        }

        for (int i = 0; i < distArray.Length; i++)
        {
            if (whatToSearch == distArray[i])
            {
                return i;
            }
        }
        return -1;
    }
}