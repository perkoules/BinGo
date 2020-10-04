namespace Mapbox.Examples
{
    using Mapbox.Unity.Location;
    using Mapbox.Unity.Map;
    using UnityEngine;
    using UnityEngine.UI;

    public class RelocateMapByGPS : MonoBehaviour
    {
        [SerializeField]
        private AbstractMap _map;

        [SerializeField]
        private Button _button;

        [SerializeField]
        private Transform _mapTransform;

        private void Start()
        {
            _button.onClick.AddListener(UpdateMapLocation);
        }

        private void UpdateMapLocation()
        {
            var location = LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation;
            _map.UpdateMap(location.LatitudeLongitude, _map.AbsoluteZoom);
            var playerPos = Camera.main.transform.position;
            _mapTransform.position = new Vector3(playerPos.x, _mapTransform.position.y, playerPos.z);
        }
    }
}