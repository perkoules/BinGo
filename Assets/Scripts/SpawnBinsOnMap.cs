using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnBinsOnMap : MonoBehaviour
{
    [SerializeField]
    private AbstractMap _map;

    [SerializeField]
    [Geocode]
    private string[] _locationStrings;

    public Vector2d[] _locations;

    [SerializeField]
    private float _spawnScale = 100f;

    private List<GameObject> _spawnedObjects;

    public GameObject wasteBinPrefab, recycleBinPrefab;
    private GameObject _markerPrefab;
    public Dictionary<string, string> binLocations;
    public string[] binNames;
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
        _locationStrings = new string[binLocations.Count];
        for (int i = 0; i < binLocations.Count; i++)
        {
            if (binLocations.ElementAt(i).Value == waste)
            {
                _markerPrefab = wasteBinPrefab;
            }
            else
            {
                _markerPrefab = recycleBinPrefab;
            }
            _locationStrings[i] = binLocations.ElementAt(i).Key;
        }
    }

    private void Start()
    {
        _locations = new Vector2d[_locationStrings.Length];
        _spawnedObjects = new List<GameObject>();
        for (int i = 0; i < _locationStrings.Length; i++)
        {
            var locationString = _locationStrings[i];
            _locations[i] = Conversions.StringToLatLon(locationString);
            var instance = Instantiate(_markerPrefab);
            instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
            instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            instance.gameObject.name = i + " : " + binNames[i];
            _spawnedObjects.Add(instance);
        }
    }

    private void Update()
    {
        int count = _spawnedObjects.Count;
        for (int i = 0; i < count; i++)
        {
            var spawnedObject = _spawnedObjects[i];
            var location = _locations[i];
            spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
            spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
        }
    }
}