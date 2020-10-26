using Mapbox.Unity.Location;
using Mapbox.Utils;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TreePlanting : MonoBehaviour
{
    public DeviceLocationProvider locationProvider;
    private PlayerDataSaver playerDataSaver;
    private MonsterDestroyer monsterDestroyer;
    private int monsterKilled;

    private void Awake()
    {
        monsterDestroyer = GetComponent<MonsterDestroyer>();
        playerDataSaver = GetComponent<PlayerDataSaver>();
        monsterKilled = playerDataSaver.GetMonstersKilled();
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(playerDataSaver.GetTreeLocation()))
        {
            SpawnTreeOnMap(playerDataSaver.GetTreeLocation());
        }
    }

    private void Update()
    {
        if (monsterKilled >= 50)
        {
            if (string.IsNullOrEmpty(playerDataSaver.GetTreeLocation()))
            {
                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    PlantTree();
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    PlantTree();
                }
            }
            else
            {
                //Water it!!!
            }
        }
    }

    private void PlantTree()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            Vector2d latlon = locationProvider.CurrentLocation.LatitudeLongitude;
            PlantedTreeLocationToCloud(latlon);
            monsterDestroyer.monstersKilled = monsterKilled - 50;
            monsterDestroyer.SetMonstersStats();
        }
    }

    private void PlantedTreeLocationToCloud(Vector2d latlon)
    {
        string treeLoc = latlon.ToString();
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>() { { "Tree Location", treeLoc } }
            },
            result => Debug.Log("Successfully planted a tree at " + treeLoc + " location"),
            error => Debug.Log(error.GenerateErrorReport()));

        SpawnTreeOnMap(treeLoc);
    }

    private void SpawnTreeOnMap(string loc)
    {
        string[] locArray = loc.Split(',');
        double x = Convert.ToDouble(locArray[0]);
        double y = Convert.ToDouble(locArray[1]);
        Vector2d location = new Vector2d(x, y);
        FindObjectOfType<SpawnTreeOnMap>().Tree(location);
    }
}