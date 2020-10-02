//-----------------------------------------------------------------------
// <copyright file="ForwardGeocoderExample.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Mapbox.Examples.Playground
{
    using Mapbox.Geocoding;
    using Mapbox.Json;
    using Mapbox.Utils.JsonConverters;
    using UnityEngine;
    using UnityEngine.UI;

    public class ForwardGeocoderExample : MonoBehaviour
    {
        [SerializeField]
        private ForwardGeocodeUserInput _searchLocation;

        [SerializeField]
        private Text _resultsText;

        private void Awake()
        {
            _searchLocation.OnGeocoderResponse += SearchLocation_OnGeocoderResponse;
        }

        private void OnDestroy()
        {
            if (_searchLocation != null)
            {
                _searchLocation.OnGeocoderResponse -= SearchLocation_OnGeocoderResponse;
            }
        }

        private void SearchLocation_OnGeocoderResponse(ForwardGeocodeResponse response)
        {
            _resultsText.text = JsonConvert.SerializeObject(_searchLocation.Response, Formatting.Indented, JsonConverters.Converters);
        }
    }
}