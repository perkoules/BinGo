//-----------------------------------------------------------------------
// <copyright file="ForwardGeocodeUserInput.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Mapbox.Examples
{
    using Mapbox.Geocoding;
    using Mapbox.Unity;
    using Mapbox.Utils;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(InputField))]
    public class ForwardGeocodeUserInput : MonoBehaviour
    {
        private InputField _inputField;

        private ForwardGeocodeResource _resource;

        private Vector2d _coordinate;

        public Vector2d Coordinate
        {
            get
            {
                return _coordinate;
            }
        }

        private bool _hasResponse;

        public bool HasResponse
        {
            get
            {
                return _hasResponse;
            }
        }

        public ForwardGeocodeResponse Response { get; private set; }

        public event Action<ForwardGeocodeResponse> OnGeocoderResponse = delegate { };

        private void Awake()
        {
            _inputField = GetComponent<InputField>();
            _inputField.onEndEdit.AddListener(HandleUserInput);
            _resource = new ForwardGeocodeResource("");
        }

        private void HandleUserInput(string searchString)
        {
            _hasResponse = false;
            if (!string.IsNullOrEmpty(searchString))
            {
                _resource.Query = searchString;
                MapboxAccess.Instance.Geocoder.Geocode(_resource, HandleGeocoderResponse);
            }
        }

        private void HandleGeocoderResponse(ForwardGeocodeResponse res)
        {
            _hasResponse = true;
            if (null == res)
            {
                _inputField.text = "no geocode response";
            }
            else if (null != res.Features && res.Features.Count > 0)
            {
                var center = res.Features[0].Center;
                _coordinate = res.Features[0].Center;
            }
            Response = res;
            OnGeocoderResponse(res);
        }
    }
}