//-----------------------------------------------------------------------
// <copyright file="RasterTileExample.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Mapbox.Examples.Playground
{
    using Mapbox.Geocoding;
    using Mapbox.Map;
    using Mapbox.Unity;
    using Mapbox.Unity.Utilities;
    using Mapbox.Utils;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    public class RasterTileExample : MonoBehaviour, Mapbox.Utils.IObserver<RasterTile>
    {
        [SerializeField]
        private ForwardGeocodeUserInput _searchLocation;

        [SerializeField]
        private Slider _zoomSlider;

        [SerializeField]
        private Dropdown _stylesDropdown;

        [SerializeField]
        private RawImage _imageContainer;

        private Map<RasterTile> _map;

        [Geocode]
        [SerializeField]
        private string _latLon;

        // initialize _mapboxStyles
        private string[] _mapboxStyles = new string[]
        {
            "mapbox://styles/mapbox/satellite-v9",
            "mapbox://styles/mapbox/streets-v9",
            "mapbox://styles/mapbox/dark-v9",
            "mapbox://styles/mapbox/light-v9"
        };

        // start location - San Francisco
        private Vector2d _startLoc = new Vector2d();

        private int _mapstyle = 0;

        private void Awake()
        {
            _searchLocation.OnGeocoderResponse += SearchLocation_OnGeocoderResponse;
            _stylesDropdown.ClearOptions();
            _stylesDropdown.AddOptions(_mapboxStyles.ToList());
            _stylesDropdown.onValueChanged.AddListener(ToggleDropdownStyles);
            _zoomSlider.onValueChanged.AddListener(AdjustZoom);

            var parsed = _latLon.Split(',');
            _startLoc.x = double.Parse(parsed[0]);
            _startLoc.y = double.Parse(parsed[1]);
        }

        private void OnDestroy()
        {
            if (_searchLocation != null)
            {
                _searchLocation.OnGeocoderResponse -= SearchLocation_OnGeocoderResponse;
            }
        }

        private void Start()
        {
            _map = new Map<RasterTile>(MapboxAccess.Instance);
            _map.TilesetId = _mapboxStyles[_mapstyle];
            _map.Center = _startLoc;
            _map.Zoom = (int)_zoomSlider.value;
            _map.Subscribe(this);
            _map.Update();
        }

        /// <summary>
        /// New search location has become available, begin a new _map query.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void SearchLocation_OnGeocoderResponse(ForwardGeocodeResponse response)
        {
            _map.Center = _searchLocation.Coordinate;
            _map.Update();
        }

        /// <summary>
        /// Zoom was modified by the slider, begin a new _map query.
        /// </summary>
        /// <param name="value">Value.</param>
        private void AdjustZoom(float value)
        {
            _map.Zoom = (int)_zoomSlider.value;
            _map.Update();
        }

        /// <summary>
        /// Style dropdown updated, begin a new _map query.
        /// </summary>
        /// <param name="value">If set to <c>true</c> value.</param>
        private void ToggleDropdownStyles(int target)
        {
            _mapstyle = target;
            _map.TilesetId = _mapboxStyles[target];
            _map.Update();
        }

        /// <summary>
        /// Update the texture with new data.
        /// </summary>
        /// <param name="tile">Tile.</param>
        public void OnNext(RasterTile tile)
        {
            if (
                tile.HasError
                || (tile.CurrentState != Tile.State.Loaded && tile.CurrentState != Tile.State.Updated)
            )
            {
                return;
            }

            // Can we utility this? Should users have to know source size?
            var texture = new Texture2D(256, 256);
            texture.LoadImage(tile.Data);
            _imageContainer.texture = texture;
        }
    }
}