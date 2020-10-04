namespace Mapbox.Examples
{
    using Mapbox.Unity.Location;
    using Mapbox.Unity.Map;
    using UnityEngine;

    public class PositionWithLocationProvider : MonoBehaviour
    {
        [SerializeField]
        private AbstractMap _map;

        /// <summary>
        /// The rate at which the transform's position tries catch up to the provided location.
        /// </summary>
        [SerializeField]
        private float _positionFollowFactor;

        /// <summary>
        /// Use a mock <see cref="T:Mapbox.Unity.Location.TransformLocationProvider"/>,
        /// rather than a <see cref="T:Mapbox.Unity.Location.EditorLocationProvider"/>.
        /// </summary>
        [SerializeField]
        private bool _useTransformLocationProvider;

        private bool _isInitialized;

        /// <summary>
        /// The location provider.
        /// This is public so you change which concrete <see cref="T:Mapbox.Unity.Location.ILocationProvider"/> to use at runtime.
        /// </summary>
        private ILocationProvider _locationProvider;

        public ILocationProvider LocationProvider
        {
            private get
            {
                if (_locationProvider == null)
                {
                    _locationProvider = _useTransformLocationProvider ?
                        LocationProviderFactory.Instance.TransformLocationProvider : LocationProviderFactory.Instance.DefaultLocationProvider;
                }

                return _locationProvider;
            }
            set
            {
                if (_locationProvider != null)
                {
                    _locationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
                }
                _locationProvider = value;
                _locationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
            }
        }

        private Vector3 _targetPosition;

        private void Start()
        {
            LocationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
            _map.OnInitialized += () => _isInitialized = true;
        }

        private void OnDestroy()
        {
            if (LocationProvider != null)
            {
                LocationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
            }
        }

        private void LocationProvider_OnLocationUpdated(Location location)
        {
            if (_isInitialized && location.IsLocationUpdated)
            {
                _targetPosition = _map.GeoToWorldPosition(location.LatitudeLongitude);
            }
        }

        private void Update()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, Time.deltaTime * _positionFollowFactor);
        }
    }
}