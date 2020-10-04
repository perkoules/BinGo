using Mapbox.Unity.Map;
using UnityEngine;

public class SetCameraHeight : MonoBehaviour
{
    [SerializeField]
    private AbstractMap _map;

    [SerializeField]
    private Camera _referenceCamera;

    [SerializeField]
    private float _cameraOffset = 100f;

    private void Start()
    {
        if (_map == null)
        {
            _map = FindObjectOfType<AbstractMap>();
        }
        if (_referenceCamera == null)
        {
            _referenceCamera = FindObjectOfType<Camera>();
        }
    }

    private void Update()
    {
        var position = _referenceCamera.transform.position;
        position.y = _map.QueryElevationInMetersAt(_map.CenterLatitudeLongitude) + _cameraOffset;
        _referenceCamera.transform.position = position;
    }
}