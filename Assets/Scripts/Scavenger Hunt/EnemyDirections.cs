namespace Mapbox.Unity.MeshGeneration.Factories
{
    using Data;
    using Mapbox.Directions;
    using Mapbox.Unity.Map;
    using Mapbox.Unity.Utilities;
    using Mapbox.Utils;
    using Modifiers;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class EnemyDirections : MonoBehaviour
    {
        [SerializeField]
        private AbstractMap _map;

        [SerializeField]
        private MeshModifier[] MeshModifiers;

        [SerializeField]
        private Material _material;

        [SerializeField]
        private Transform[] _waypoints;

        private List<Vector3> _cachedWaypoints;

        [SerializeField]
        [Range(1, 10)]
        private float UpdateFrequency = 2;

        private Directions _directions;
        private int _counter;

        private GameObject _directionsGO;
        private bool _recalculateNext;

        protected virtual void Awake()
        {
            if (_map == null)
            {
                _map = FindObjectOfType<AbstractMap>();
            }
            _directions = MapboxAccess.Instance.Directions;
            ScavengerHunt.OnShowDirections += ScavengerHunt_OnShowDirections;
            ScavengerHunt.OnTaskCompleted += ScavengerHunt_OnTaskCompleted;
        }

        private void ScavengerHunt_OnTaskCompleted(GameObject obj, string tasks, bool done)
        {
            if (!done)
            {
                _waypoints = _waypoints.Where(en => en.gameObject != obj).ToArray();
                obj.SetActive(false);
            }
            else
            {
                obj.SetActive(false);
                ScavengerHunt.OnShowDirections -= ScavengerHunt_OnShowDirections;
                ScavengerHunt.OnTaskCompleted -= ScavengerHunt_OnTaskCompleted;
            }
        }

        private void ScavengerHunt_OnShowDirections(GameObject player, List<Transform> enemies)
        {
            _map.OnInitialized += Query;
            _map.OnUpdated += Query;
            _waypoints = new Transform[enemies.Count + 1];
            _waypoints[0] = player.transform;
            enemies.CopyTo(_waypoints, 1);
            _cachedWaypoints = new List<Vector3>(_waypoints.Length);
            foreach (var item in _waypoints)
            {
                _cachedWaypoints.Add(item.position);
            }
            _recalculateNext = false;

            foreach (var modifier in MeshModifiers)
            {
                modifier.Initialize();
            }

            StartCoroutine(QueryTimer());
        }

        protected virtual void OnDestroy()
        {
            _map.OnInitialized -= Query;
            _map.OnUpdated -= Query;
        }

        private void Query()
        {
            var count = _waypoints.Length;
            var wp = new Vector2d[count];
            for (int i = 0; i < count; i++)
            {
                wp[i] = _waypoints[i].GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
            }
            var _directionResource = new DirectionResource(wp, RoutingProfile.Walking);
            _directionResource.Steps = true;
            _directions.Query(_directionResource, HandleDirectionsResponse);
        }

        public IEnumerator QueryTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(UpdateFrequency);
                for (int i = 0; i < _waypoints.Length; i++)
                {
                    if (_waypoints[i].position != _cachedWaypoints[i])
                    {
                        _recalculateNext = true;
                        _cachedWaypoints[i] = _waypoints[i].position;
                    }
                }

                if (_recalculateNext)
                {
                    Query();
                    _recalculateNext = false;
                }
            }
        }

        private void HandleDirectionsResponse(DirectionsResponse response)
        {
            if (response == null || null == response.Routes || response.Routes.Count < 1)
            {
                return;
            }

            var meshData = new MeshData();
            var dat = new List<Vector3>();
            foreach (var point in response.Routes[0].Geometry)
            {
                dat.Add(Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
            }

            var feat = new VectorFeatureUnity();
            feat.Points.Add(dat);

            foreach (MeshModifier mod in MeshModifiers.Where(x => x.Active))
            {
                mod.Run(feat, meshData, _map.WorldRelativeScale);
            }

            CreateGameObject(meshData);
        }

        private GameObject CreateGameObject(MeshData data)
        {
            if (_directionsGO != null)
            {
                _directionsGO.Destroy();
            }
            _directionsGO = new GameObject("direction waypoint " + " entity");
            var mesh = _directionsGO.AddComponent<MeshFilter>().mesh;
            mesh.subMeshCount = data.Triangles.Count;

            mesh.SetVertices(data.Vertices);
            _counter = data.Triangles.Count;
            for (int i = 0; i < _counter; i++)
            {
                var triangle = data.Triangles[i];
                mesh.SetTriangles(triangle, i);
            }

            _counter = data.UV.Count;
            for (int i = 0; i < _counter; i++)
            {
                var uv = data.UV[i];
                mesh.SetUVs(i, uv);
            }

            mesh.RecalculateNormals();
            _directionsGO.AddComponent<MeshRenderer>().material = _material;
            _directionsGO.layer = LayerMask.NameToLayer("Player");
            return _directionsGO;
        }
    }
}