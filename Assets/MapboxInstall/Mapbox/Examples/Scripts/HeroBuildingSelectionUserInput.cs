//-----------------------------------------------------------------------
// <copyright file="HeroBuildingSelectionUserInput.cs" company="Mapbox">
//     Copyright (c) 2018 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Mapbox.Examples
{
    using Mapbox.Geocoding;
    using Mapbox.Unity;
    using Mapbox.Unity.Utilities;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public class HeroBuildingSelectionUserInput : MonoBehaviour
    {
        [Geocode]
        public string location;

        [SerializeField]
        private Vector3 _cameraPosition;

        [SerializeField]
        private Vector3 _cameraRotation;

        private Camera _camera;

        private Button _button;

        private ForwardGeocodeResource _resource;

        private bool _hasResponse;

        public bool HasResponse
        {
            get
            {
                return _hasResponse;
            }
        }

        public ForwardGeocodeResponse Response { get; private set; }

        public event Action<ForwardGeocodeResponse, bool> OnGeocoderResponse = delegate { };

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleUserInput);
            _resource = new ForwardGeocodeResource("");
            _camera = Camera.main;
        }

        private void TransformCamera()
        {
            _camera.transform.position = _cameraPosition;
            _camera.transform.localEulerAngles = _cameraRotation;
        }

        private void HandleUserInput()
        {
            _hasResponse = false;
            if (!string.IsNullOrEmpty(location))
            {
                _resource.Query = location;
                MapboxAccess.Instance.Geocoder.Geocode(_resource, HandleGeocoderResponse);
            }
        }

        private void HandleGeocoderResponse(ForwardGeocodeResponse res)
        {
            _hasResponse = true;
            Response = res;
            TransformCamera();
            OnGeocoderResponse(res, false);
        }

        public void BakeCameraTransform()
        {
            _cameraPosition = _camera.transform.position;
            _cameraRotation = _camera.transform.localEulerAngles;
        }
    }
}