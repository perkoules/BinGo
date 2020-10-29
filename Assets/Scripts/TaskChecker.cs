using System;
using System.Collections;
using UnityEngine;

public class TaskChecker : MonoBehaviour
{
    public GameObject[] gotasks;
    public static TaskChecker Instance { get; private set; }

    private int waste, recycle, bins, monsters;

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

    public void FindActiveTasks()
    {
        gotasks = GameObject.FindGameObjectsWithTag("TaskTag");
        if(gotasks.Length <= 0)
        {
            ResetCounters();
        }
    }

    public void CheckTaskDone()
    {
        Debug.Log("Checker");
        foreach (var go in gotasks)
        {
            if (go.name.Contains("waste"))
            {
                int amount = Convert.ToInt32((go.name.Replace("Collect", "")).Replace("waste", ""));
                waste++;
                bins++;
                if (waste >= amount)
                {
                    Destroy(go);
                }
            }
            else if (go.name.Contains("recycle"))
            {
                int amount = Convert.ToInt32((go.name.Replace("Collect", "")).Replace("recycle", ""));
                recycle++;
                bins++;
                if (waste >= amount)
                {
                    Destroy(go);
                }
            }
            else if (go.name.Contains("bin"))
            {
                int amount = Convert.ToInt32((go.name.Replace("Visit", "")).Replace("bin", ""));
                if (bins >= amount)
                {
                    Destroy(go);
                }
            }
            else if (go.name.Contains("monsters"))
            {
                int amount = Convert.ToInt32((go.name.Replace("Kill", "")).Replace("monsters", ""));
                if (monsters >= amount)
                {
                    Destroy(go);
                }
            }
        }
    }

    public void ResetCounters()
    {
        waste = 0;
        recycle = 0;
        bins = 0;
        monsters = 0;        
    }
}