using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnBinsOnMap : MonoBehaviour
{
    [SerializeField]
    private AbstractMap map;

    [SerializeField]
    [Geocode]
    private string[] locationStrings;

    public Vector2d[] locations;

    [SerializeField]
    private float spawnScale = 100f;

    public List<GameObject> spawnedObjects;

    public GameObject wasteBinPrefab, recycleBinPrefab;
    private GameObject markerPrefab;
    public Dictionary<string, string> binLocations;
    private string[] binNames;
    private const string recycle = "recycle";
    private const string waste = "waste";

    private void Awake()
    {
        binLocations = new Dictionary<string, string>()
        {
            { "54.57059 , -1.235456", recycle},
            { "54.57049 , -1.233695", recycle },
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
        binNames = new string[]
        {
            "Campus Southfield - Recycle Bin",
            "Campus Southfield - Recycle Bin",
            "Athena Building - General Waste",
            "PrintWorks - General Waste",
            "Campus Student Life - Recycle Bin",
            "Campus Business - Recycle Bin",
            "Middlesbrough Tower - General Waste",
            "Claredon Building -  General Waste",
            "Middlesbrough Tower - General Waste",
            "Printworks Corner - General Waste",
            "Athena Building - General Waste"
        };
        locationStrings = new string[binLocations.Count];
    }

    private void Start()
    {
        locations = new Vector2d[locationStrings.Length];
        spawnedObjects = new List<GameObject>();
        for (int i = 0; i < locationStrings.Length; i++)
        {
            locationStrings[i] = binLocations.ElementAt(i).Key;
            var locationString = locationStrings[i];
            locations[i] = Conversions.StringToLatLon(locationString);
            if (binLocations.ElementAt(i).Value == waste)
            {
                markerPrefab = wasteBinPrefab;
            }
            else
            {
                markerPrefab = recycleBinPrefab;
            }
            var instance = Instantiate(markerPrefab);
            instance.transform.localPosition = map.GeoToWorldPosition(locations[i], true);
            instance.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
            instance.gameObject.name = i + " : " + binNames[i];
            spawnedObjects.Add(instance);
        }
    }

    private void Update()
    {
        if (map == null)
        {
            map = FindObjectOfType<AbstractMap>();
        }
        int count = spawnedObjects.Count;
        for (int i = 0; i < count; i++)
        {
            var spawnedObject = spawnedObjects[i];
            var location = locations[i];
            spawnedObject.transform.localPosition = map.GeoToWorldPosition(location, true);
            spawnedObject.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
        }
    }
}