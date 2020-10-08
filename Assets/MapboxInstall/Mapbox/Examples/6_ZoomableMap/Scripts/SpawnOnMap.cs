namespace Mapbox.Examples
{
    using Mapbox.Unity.Map;
    using Mapbox.Unity.Utilities;
    using Mapbox.Utils;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class SpawnOnMap : MonoBehaviour
    {
        [SerializeField]
        private AbstractMap _map;

        [SerializeField]
        [Geocode]
        public string[] _locationStrings;

        public Vector2d[] _locations;

        [SerializeField]
        private float _spawnScale = 100f;

        [SerializeField]
        public GameObject _markerPrefab;

        private List<GameObject> _spawnedObjects;

        private BinLocations binLocations;

        private void Awake()
        {
            binLocations = GetComponent<BinLocations>();
        }



        private void Start()
        {
            _locations = new Vector2d[_locationStrings.Length];
            _spawnedObjects = new List<GameObject>();
            for (int i = 0; i < _locationStrings.Length; i++)
            {
                var locationString = _locationStrings[i];
                _locations[i] = Conversions.StringToLatLon(locationString);
                if(binLocations.binLocations.ElementAt(i).Value == "recycle")
                {
                    _markerPrefab = binLocations.recycleBinPrefab;
                }
                else
                {
                    _markerPrefab = binLocations.wasteBinPrefab;
                }                
                var instance = Instantiate(_markerPrefab);
                instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
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
}