using Mapbox.Unity.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CalculateDistance : MonoBehaviour
{
    public static CalculateDistance Instance { get; set; }

    public List<GameObject> bins;
    public float[] distances;
    [SerializeField]private int minIndex;


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
        SpawnOnMap.Instance.map.OnTileFinished += Map_OnTileFinished;
        SpawnOnMap.Instance.map.OnTilesDisposing += Map_OnTilesDisposing;
        InvokeRepeating("GetAllDistances", 5.0f, 3.0f);
    }

    private void Map_OnTilesDisposing(List<Mapbox.Map.UnwrappedTileId> obj)
    {
        StartCoroutine(FindAgain());
    }

    IEnumerator FindAgain()
    {
        yield return new WaitForSeconds(1f);
        bins = GameObject.FindGameObjectsWithTag("BinTag").ToList();
        distances = new float[bins.Count];
    }

    private void Map_OnTileFinished(Mapbox.Unity.MeshGeneration.Data.UnityTile obj)
    {
        bins = GameObject.FindGameObjectsWithTag("BinTag").ToList();
        distances = new float[bins.Count];
    }

    private void GetAllDistances()
    {
        if (bins.Count > 0)
        {
            for (int i = 0; i < bins.Count; i++)
            {
                distances[i] = Vector3.Distance(gameObject.transform.position, bins[i].transform.position);
            }

            minIndex = Array.IndexOf(distances, distances.Min());
        }
    }
    public string GetClosestBinData()
    {
        return bins[minIndex].name;
    }
    public float MinDistance()
    {
        return distances[minIndex];
    }

}