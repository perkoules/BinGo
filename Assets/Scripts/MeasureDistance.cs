﻿using Mapbox.CheapRulerCs;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class MeasureDistance : MonoBehaviour
{
    public DeviceLocationProvider locationProvider;
    public SpawnBinsOnMap spawnBins;
    private double[] from, to;
    public double[] distances;
    public int minIndex = -1;
    private Vector2d playerCurrentLocation, toBinLocation;

    public TextMeshProUGUI myText;

    private void Awake()
    {
        if (locationProvider == null)
        {
            locationProvider = FindObjectOfType<DeviceLocationProvider>();
        }
        if (spawnBins == null)
        {
            spawnBins = FindObjectOfType<SpawnBinsOnMap>();
        }
    }

    private void Start()
    {
        distances = new double[12];
        InvokeRepeating("GetDistanceToBin", 5.0f, 3.0f);
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

        myText.text = distances[minIndex].ToString(); // Show dist of the closest on ui [for testing]


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