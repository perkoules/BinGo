namespace Mapbox.Examples
{
    using Mapbox.Unity.Map;
    using Mapbox.Unity.Utilities;
    using UnityEngine;

    public class SpawnOnGlobeExample : MonoBehaviour
    {
        [SerializeField]
        private AbstractMap _map;

        [SerializeField]
        [Geocode]
        private string[] _locations;

        [SerializeField]
        private float _spawnScale = 100f;

        [SerializeField]
        private GameObject _markerPrefab;

        private void Start()
        {
            foreach (var locationString in _locations)
            {
                var instance = Instantiate(_markerPrefab);
                var location = Conversions.StringToLatLon(locationString);
                var earthRadius = ((IGlobeTerrainLayer)_map.Terrain).EarthRadius;
                instance.transform.position = Conversions.GeoToWorldGlobePosition(location, earthRadius);
                instance.transform.localScale = Vector3.one * _spawnScale;
                instance.transform.SetParent(transform);
            }
        }
    }
}