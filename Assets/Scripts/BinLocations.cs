using Mapbox.Examples;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BinLocations : MonoBehaviour
{
    private SpawnOnMap spawnOn;
    public GameObject wasteBinPrefab, recycleBinPrefab;
    public Dictionary<string, string> binLocations;
    private const string recycle = "recycle";
    private const string waste = "waste";

    private void Awake()
    {
        spawnOn = GetComponent<SpawnOnMap>();
        binLocations = new Dictionary<string, string>()
        {            
            { "54.57059 , -1.235456", recycle},
            { "54.57049 , -1.233695", recycle },
            { "54.57226 , -1.23245 ", recycle },
            { "54.570567 , -1.23321 ", waste },
            { "54.572564 , -1.232988", waste },
            { "54.570534 , -1.234636", recycle },
            { "54.570839 , -1.23558 ", recycle },
            { "54.571996 , -1.235373", waste },
            { "54.571404 , -1.235802", waste },
            { "54.572271 , -1.235357", waste },
            { "54.572527 , -1.232599", waste },
            { "54.570549 , -1.233324", waste }
        };
        spawnOn._locationStrings = new string[binLocations.Count];
        for (int i = 0; i < binLocations.Count; i++)
        {
            if(binLocations.ElementAt(i).Value == waste)
            {
                spawnOn._markerPrefab = wasteBinPrefab;
            }
            else
            {
                spawnOn._markerPrefab = recycleBinPrefab;
            }
            spawnOn._locationStrings[i] = binLocations.ElementAt(i).Key;
        }
    }
}