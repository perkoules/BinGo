//-----------------------------------------------------------------------
// <copyright file="DirectionsExample.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Mapbox.Examples.Playground
{
    using Mapbox.Directions;
    using Mapbox.Geocoding;
    using Mapbox.Json;
    using Mapbox.Unity;
    using Mapbox.Utils;
    using Mapbox.Utils.JsonConverters;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Fetch directions JSON once start and end locations are provided.
    /// Example: Enter Start Location: San Francisco, Enter Destination: Los Angeles
    /// </summary>
    public class DirectionsExample : MonoBehaviour
    {
        [SerializeField]
        private Text _resultsText;

        [SerializeField]
        private ForwardGeocodeUserInput _startLocationGeocoder;

        [SerializeField]
        private ForwardGeocodeUserInput _endLocationGeocoder;

        private Directions _directions;

        private Vector2d[] _coordinates;

        private DirectionResource _directionResource;

        private void Start()
        {
            _directions = MapboxAccess.Instance.Directions;
            _startLocationGeocoder.OnGeocoderResponse += StartLocationGeocoder_OnGeocoderResponse;
            _endLocationGeocoder.OnGeocoderResponse += EndLocationGeocoder_OnGeocoderResponse;

            _coordinates = new Vector2d[2];

            // Can we make routing profiles an enum?
            _directionResource = new DirectionResource(_coordinates, RoutingProfile.Driving);
            _directionResource.Steps = true;
        }

        private void OnDestroy()
        {
            if (_startLocationGeocoder != null)
            {
                _startLocationGeocoder.OnGeocoderResponse -= StartLocationGeocoder_OnGeocoderResponse;
            }

            if (_startLocationGeocoder != null)
            {
                _startLocationGeocoder.OnGeocoderResponse -= EndLocationGeocoder_OnGeocoderResponse;
            }
        }

        /// <summary>
        /// Start location geocoder responded, update start coordinates.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void StartLocationGeocoder_OnGeocoderResponse(ForwardGeocodeResponse response)
        {
            _coordinates[0] = _startLocationGeocoder.Coordinate;
            if (ShouldRoute())
            {
                Route();
            }
        }

        /// <summary>
        /// End location geocoder responded, update end coordinates.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void EndLocationGeocoder_OnGeocoderResponse(ForwardGeocodeResponse response)
        {
            _coordinates[1] = _endLocationGeocoder.Coordinate;
            if (ShouldRoute())
            {
                Route();
            }
        }

        /// <summary>
        /// Ensure both forward geocoders have a response, which grants access to their respective coordinates.
        /// </summary>
        /// <returns><c>true</c>, if both forward geocoders have a response, <c>false</c> otherwise.</returns>
        private bool ShouldRoute()
        {
            return _startLocationGeocoder.HasResponse && _endLocationGeocoder.HasResponse;
        }

        /// <summary>
        /// Route
        /// </summary>
        private void Route()
        {
            _directionResource.Coordinates = _coordinates;
            _directions.Query(_directionResource, HandleDirectionsResponse);
        }

        /// <summary>
        /// Log directions response to UI.
        /// </summary>
        /// <param name="res">Res.</param>
        private void HandleDirectionsResponse(DirectionsResponse res)
        {
            var data = JsonConvert.SerializeObject(res, Formatting.Indented, JsonConverters.Converters);
            string sub = data.Substring(0, data.Length > 5000 ? 5000 : data.Length) + "\n. . . ";
            _resultsText.text = sub;
        }
    }
}