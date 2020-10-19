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
            { "54.570546 , -1.233163 ", waste   },         // Athena Building - General Waste
            { "54.570518 , -1.233336 ", waste   },         // Athena Building - General Waste
            { "54.570477 , -1.233729 ", recycle },         // Campus Southfield - Recycle Bin
            { "54.570545 , -1.234701 ", recycle },         // Campus Southfield - Recycle Bin
            { "54.570593 , -1.235453 ", recycle },         // Campus Student Life - Recycle Bin
            { "54.570816 , -1.235594 ", recycle },         // Campus Business School - Recycle Bin
            { "54.571404 , -1.235956 ", waste   },         // Claredon Building -  General Waste
            { "54.571990 , -1.235441 ", waste   },         // Middlesbrough Tower - General Waste
            { "54.572263 , -1.235384 ", waste   },         // Middlesbrough Tower - General Waste
            { "54.572538 , -1.233023 ", waste   },         // PrintWorks - General Waste
            { "54.572527 , -1.232632 ", waste   },         // Printworks Corner - General Waste
            { "54.57226  , -1.23245  ", recycle }          // Outside House - Recycle Bin
        };
        binNames = new string[]
        {
            "Athena Building - General Waste",
            "Athena Building - General Waste",
            "Campus Southfield - Recycle Bin",
            "Campus Southfield - Recycle Bin",
            "Campus Student Life - Recycle Bin",
            "Campus Business School - Recycle Bin",
            "Claredon Building -  General Waste",
            "Middlesbrough Tower - General Waste",
            "Middlesbrough Tower - General Waste",
            "PrintWorks - General Waste",
            "Printworks Corner - General Waste",
            "Outside House - Recycle Bin"
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