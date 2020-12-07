using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyCollection : MonoBehaviour
{
    public List<GameObject> enemiesAdded;
    public static EnemyCollection Instance { get; private set; }
    public Dictionary<string, bool> taskCompletion;
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
    private void Awake()
    {
        enemiesAdded = new List<GameObject>();
        taskCompletion = new Dictionary<string, bool>()
        {
            { "1", false },
            { "2", false },
            { "3", false },
            { "4", false },
            { "5", false }
        };
    }

    public void AddEnemy(GameObject go)
    {
        if (!enemiesAdded.Exists(g => g.name == go.name))
        {
            enemiesAdded.Add(go);
        }
        else
        {
            Destroy(go);
        }
    }

    public void CompleteHuntTask(string task, bool completed)
    {
        taskCompletion[task] = completed;
    }

    public int CurrentHuntTask()
    {
        for (int i = 0; i < taskCompletion.Count; i++)
        {
            if (!taskCompletion.ElementAt(i).Value)
            {
                return i;
            }
        }
        return -1;
    }

    /*public GameObject GetClosestBat()
    {
        float[] dist = new float[0];
        for (int i = 0; i < clues.Count; i++)
        {
            dist[i] = Vector3.Distance(clues[i].transform.position, Camera.main.transform.position);
        }
        int index = Array.IndexOf(dist, dist.Min());
        return clues[index];
    }*/
}
