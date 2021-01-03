using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Condition
{
    None,
    Game,
    Testing
}
public class SpawnOnMap : MonoBehaviour
{
    public AbstractMap map;
    private float spawnScale = 1f;
    public GameObject treePrefab, particlePrefab;
    public GameObject[] enemiesToSpawn;
    public Dictionary<string, GameObject> enemiesAndLocations;
    public Vector2d[] locations;
    public List<GameObject> spawnedEnemies;
    public static SpawnOnMap Instance { get; private set; }

    public Condition con;

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
        if (con == Condition.Game)
        {
            enemiesAndLocations = new Dictionary<string, GameObject>()
            {
                { "54.570903, -1.235883",   enemiesToSpawn[0] },
                { "54.574738, -1.231814",   enemiesToSpawn[1] },
                { "54.577917, -1.218668",   enemiesToSpawn[2] },
                { "54.583422, -1.230368",   enemiesToSpawn[3] },
                { "54.564913, -1.235464",   enemiesToSpawn[4] }
            };
        }
        else if (con == Condition.Testing)
        {
            enemiesAndLocations = new Dictionary<string, GameObject>()
            {
                { "54.571749, -1.232586",   enemiesToSpawn[0] },
                { "54.571546, -1.232641",   enemiesToSpawn[1] },
                { "54.571235, -1.232696",   enemiesToSpawn[2] },
                { "54.571409, -1.232756",   enemiesToSpawn[3] },
                { "54.571654, -1.232690",   enemiesToSpawn[4] }
            };
        }
    }
    public void Tree(Vector2d latlon)
    {
        var instance = Instantiate(treePrefab);
        instance.transform.localPosition = map.GeoToWorldPosition(latlon, true);
        instance.transform.localScale = new Vector3(spawnScale, spawnScale, spawnScale);
        instance.gameObject.name = "My Planted Tree";
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