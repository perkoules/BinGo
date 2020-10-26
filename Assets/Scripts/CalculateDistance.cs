using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class CalculateDistance : MonoBehaviour
{
    public GameObject[] bins;
    public float[] distances;
    public int minIndex;
    public TextMeshProUGUI myText;

    public static CalculateDistance Instance { get; private set; }

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
        StartCoroutine(FindAllBinsInMap(4.5f));
        InvokeRepeating("GetAllDistances", 5.0f, 3.0f);
    }

    public IEnumerator FindAllBinsInMap(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        bins = GameObject.FindGameObjectsWithTag("BinTag");
        distances = new float[bins.Length];
    }

    private void GetAllDistances()
    {
        if (bins.Length > 0)
        {
            for (int i = 0; i < bins.Length; i++)
            {
                distances[i] = Vector3.Distance(gameObject.transform.position, bins[i].transform.position);
            }
            minIndex = Array.IndexOf(distances, distances.Min());
            myText.text = distances[minIndex].ToString(); // Show dist of the closest on ui [for testing]
        }
        else
        {
            StartCoroutine(FindAllBinsInMap(2f));
        }
    }
}