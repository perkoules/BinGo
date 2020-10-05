﻿using Mapbox.Unity.Map;
using UnityEngine;

public class SnapMapToTargetTransform : MonoBehaviour
{
    [SerializeField]
    private AbstractMap _map;

    [SerializeField]
    private Transform _target;

    private void Awake()
    {
        if (_map == null)
        {
            _map = FindObjectOfType<AbstractMap>();
        }
    }

    private void Start()
    {
        _map.OnUpdated += SnapMapToTarget;
    }

    private void SnapMapToTarget()
    {
        var h = _map.QueryElevationInUnityUnitsAt(_map.CenterLatitudeLongitude);
        _map.Root.transform.position = new Vector3(
             _map.Root.transform.position.x,
              _target.transform.position.y - h,
             _map.Root.transform.position.z);
    }
}