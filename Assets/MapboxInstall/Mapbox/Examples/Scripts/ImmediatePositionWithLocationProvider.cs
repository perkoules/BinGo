namespace Mapbox.Examples
{
    using Mapbox.Unity.Location;
    using UnityEngine;

    public class ImmediatePositionWithLocationProvider : MonoBehaviour
    {
        private bool _isInitialized;

        private ILocationProvider _locationProvider;

        private ILocationProvider LocationProvider
        {
            get
            {
                if (_locationProvider == null)
                {
                    _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
                }

                return _locationProvider;
            }
        }

        private Vector3 _targetPosition;

        private void Start()
        {
            LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isInitialized = true;
        }

        private void LateUpdate()
        {
            if (_isInitialized)
            {
                var map = LocationProviderFactory.Instance.mapManager;
                transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
            }
        }
    }
}