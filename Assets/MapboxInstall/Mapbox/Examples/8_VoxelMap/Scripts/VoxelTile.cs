namespace Mapbox.Examples.Voxels
{
    using Mapbox.Geocoding;
    using Mapbox.Map;
    using Mapbox.Platform;
    using Mapbox.Unity;
    using Mapbox.Unity.Utilities;
    using Mapbox.Utils;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    internal class VoxelTile : MonoBehaviour, Mapbox.Utils.IObserver<RasterTile>, Mapbox.Utils.IObserver<RawPngRasterTile>
    {
        [SerializeField]
        private ForwardGeocodeUserInput _geocodeInput;

        [SerializeField]
        private int _zoom = 17;

        [SerializeField]
        private float _elevationMultiplier = 1f;

        [SerializeField]
        private int _voxelDepthPadding = 1;

        [SerializeField]
        private int _tileWidthInVoxels;

        [SerializeField]
        private VoxelFetcher _voxelFetcher;

        [SerializeField]
        private GameObject _camera;

        [SerializeField]
        private int _voxelBatchCount = 100;

        [SerializeField]
        private string _styleUrl;

        private Map<RasterTile> _raster;
        private Map<RawPngRasterTile> _elevation;

        private Texture2D _rasterTexture;
        private Texture2D _elevationTexture;

        private IFileSource _fileSource;

        private List<VoxelData> _voxels = new List<VoxelData>();

        private List<GameObject> _instantiatedVoxels = new List<GameObject>();

        private float _tileScale;

        private void Awake()
        {
            _geocodeInput.OnGeocoderResponse += GeocodeInput_OnGeocoderResponse;
        }

        private void OnDestroy()
        {
            if (_geocodeInput)
            {
                _geocodeInput.OnGeocoderResponse -= GeocodeInput_OnGeocoderResponse;
            }
        }

        private void Start()
        {
            _fileSource = MapboxAccess.Instance;

            _raster = new Map<RasterTile>(_fileSource);
            _elevation = new Map<RawPngRasterTile>(_fileSource);

            if (!string.IsNullOrEmpty(_styleUrl))
            {
                _raster.TilesetId = _styleUrl;
            }
            _elevation.TilesetId = "mapbox.terrain-rgb";

            _elevation.Subscribe(this);
            _raster.Subscribe(this);

            // Torres Del Paine
            FetchWorldData(new Vector2d(-50.98306, -72.96639));
        }

        private void GeocodeInput_OnGeocoderResponse(ForwardGeocodeResponse response)
        {
            Cleanup();
            FetchWorldData(_geocodeInput.Coordinate);
        }

        private void Cleanup()
        {
            StopAllCoroutines();
            _rasterTexture = null;
            _elevationTexture = null;
            _voxels.Clear();
            foreach (var voxel in _instantiatedVoxels)
            {
                voxel.Destroy();
            }
        }

        private void FetchWorldData(Vector2d coordinates)
        {
            _tileScale = (_tileWidthInVoxels / 256f) / Conversions.GetTileScaleInMeters((float)coordinates.x, _zoom);
            var bounds = new Vector2dBounds();
            bounds.Center = coordinates;
            _raster.SetVector2dBoundsZoom(bounds, _zoom);
            _elevation.SetVector2dBoundsZoom(bounds, _zoom);
            _raster.Update();
            _elevation.Update();
        }

        public void OnNext(RasterTile tile)
        {
            if (
                !tile.HasError
                && (tile.CurrentState == Tile.State.Loaded || tile.CurrentState == Tile.State.Updated)
            )
            {
                _rasterTexture = new Texture2D(2, 2);
                _rasterTexture.LoadImage(tile.Data);
                TextureScale.Point(_rasterTexture, _tileWidthInVoxels, _tileWidthInVoxels);

                if (ShouldBuildWorld())
                {
                    BuildVoxelWorld();
                }
            }
        }

        public void OnNext(RawPngRasterTile tile)
        {
            if (
                !tile.HasError
                && (tile.CurrentState == Tile.State.Loaded || tile.CurrentState == Tile.State.Updated)
            )
            {
                _elevationTexture = new Texture2D(2, 2);
                _elevationTexture.LoadImage(tile.Data);
                TextureScale.Point(_elevationTexture, _tileWidthInVoxels, _tileWidthInVoxels);

                if (ShouldBuildWorld())
                {
                    BuildVoxelWorld();
                }
            }
        }

        private bool ShouldBuildWorld()
        {
            return _rasterTexture != null && _elevationTexture != null;
        }

        private void BuildVoxelWorld()
        {
            var baseHeight = (int)Conversions.GetRelativeHeightFromColor(
                (_elevationTexture.GetPixel(_elevationTexture.width / 2, _elevationTexture.height / 2))
                , _elevationMultiplier * _tileScale
            );

            for (int x = 0; x < _rasterTexture.width; x++)
            {
                for (int z = 0; z < _rasterTexture.height; z++)
                {
                    var height = (int)Conversions.GetRelativeHeightFromColor(
                        _elevationTexture.GetPixel(x, z)
                        , _elevationMultiplier * _tileScale
                    ) - baseHeight;

                    var startHeight = height - _voxelDepthPadding - 1;
                    var color = _rasterTexture.GetPixel(x, z);

                    for (int y = startHeight; y < height; y++)
                    {
                        _voxels.Add(new VoxelData() { Position = new Vector3(x, y, z), Prefab = _voxelFetcher.GetVoxelFromColor(color) });
                    }
                }
            }

            if (_camera != null)
            {
                _camera.transform.position = new Vector3(_tileWidthInVoxels * .5f, 2f, _tileWidthInVoxels * .5f);
            }

            if (this != null)
            {
                StartCoroutine(BuildRoutine());
            }
        }

        private IEnumerator BuildRoutine()
        {
            var distanceOrderedVoxels = _voxels.OrderBy(x => (_camera.transform.position - x.Position).magnitude).ToList();

            for (int i = 0; i < distanceOrderedVoxels.Count; i += _voxelBatchCount)
            {
                for (int j = 0; j < _voxelBatchCount; j++)
                {
                    var index = i + j;
                    if (index < distanceOrderedVoxels.Count)
                    {
                        var voxel = distanceOrderedVoxels[index];
                        _instantiatedVoxels.Add(Instantiate(voxel.Prefab, voxel.Position, Quaternion.identity, transform) as GameObject);
                    }
                }
                yield return null;
            }
        }
    }
}