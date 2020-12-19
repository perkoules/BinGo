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

    public GameObject[] enemiesToSpawn;
    public Dictionary<string, GameObject> enemiesAndLocations;
    public Vector2d[] locations;
    public List<GameObject> spawnedEnemies;
    public static SpawnOnMap Instance { get; private set; }
    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        /*enemiesAndLocations = new Dictionary<string, GameObject>()
        {
            { "54.570785, -1.235899",   enemiesToSpawn[0] },
            { "54.574730, -1.231823",   enemiesToSpawn[1] },
            { "54.577917, -1.218668",   enemiesToSpawn[2] },
            { "54.583581, -1.229148",   enemiesToSpawn[3] },
            { "54.564898, -1.235498",   enemiesToSpawn[4] }
        };*/

        //For Testing
        enemiesAndLocations = new Dictionary<string, GameObject>()
        {
            { "54.571749, -1.232586",   enemiesToSpawn[0] },
            { "54.571546, -1.232641",   enemiesToSpawn[1] },
            { "54.571235, -1.232696",   enemiesToSpawn[2] },
            { "54.571409, -1.232756",   enemiesToSpawn[3] },
            { "54.571654, -1.232690",   enemiesToSpawn[4] }
        };
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
    public void SpawnEnemies(string en)
    {
        char[] enArray = en.ToCharArray();
        locations = new Vector2d[enemiesToSpawn.Length];
        spawnedEnemies = new List<GameObject>();
        for (int i = 0; i < enemiesToSpawn.Length; i++)
        {
            if (enArray[i] == '0')
            {
                var locationString = enemiesAndLocations.ElementAt(i).Key;
                locations[i] = Conversions.StringToLatLon(locationString);
                var instance = Instantiate(enemiesToSpawn[i]);
                instance.transform.localPosition = map.GeoToWorldPosition(locations[i], true);
                instance.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
                spawnedEnemies.Add(instance);
            }
        }
    }
}