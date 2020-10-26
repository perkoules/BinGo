using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnTreeOnMap : MonoBehaviour
{
    [SerializeField]
    private AbstractMap map;
    [SerializeField]
    private float spawnScale = 1f;
    public GameObject treePrefab;

    public void Tree(Vector2d latlon)
    {
        var instance = Instantiate(treePrefab);
        instance.transform.localPosition = map.GeoToWorldPosition(latlon, true);
        instance.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
        instance.gameObject.name = "My Planted Tree";
    }
}