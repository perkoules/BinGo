using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnOnMap : MonoBehaviour
{
    [SerializeField]
    private AbstractMap map;
    [SerializeField]
    private float spawnScale = 1f;
    public GameObject treePrefab, particlePrefab;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            BatEffect();
        }
    }

    public void Tree(Vector2d latlon)
    {
        var instance = Instantiate(treePrefab);
        instance.transform.localPosition = map.GeoToWorldPosition(latlon, true);
        instance.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
        instance.gameObject.name = "My Planted Tree";
    }
    public void BatEffect()
    {
        Vector2d latlon = new Vector2d(54.572142, -1.235557);
        var instance = Instantiate(particlePrefab);
        instance.transform.localPosition = map.GeoToWorldPosition(latlon, true);
        instance.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
        instance.gameObject.name = "Bat effect";
    }
}