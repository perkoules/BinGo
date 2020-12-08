using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScavengerHunt : MonoBehaviour
{
    public List<GameObject> enemiesAdded;
    public static ScavengerHunt Instance { get; private set; }
    public Dictionary<string, bool> taskCompletion;

    public GameObject prefabTutorialMessageBox, battlePanel;
    public GameObject prefabObjectives, mainPanel;

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
    }

    public void AddEnemy(GameObject go)
    {
        if (!enemiesAdded.Exists(g => g.name == go.name))
        {
            go.SetActive(false);
            enemiesAdded.Add(go);
        }
        else
        {
            Destroy(go);
        }
    }

    public delegate void TaskCompleted(string obj);
    public static event TaskCompleted OnTaskCompleted;

    public void CompleteHuntTask(GameObject go, bool completed)
    {
        string task = go.name.Replace(" Variant", "");
        taskCompletion[task] = completed;
        OnTaskCompleted(task);
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

    public void StartHunting()
    {
        foreach (var go in enemiesAdded)
        {
            go.SetActive(true);
        }
        taskCompletion = new Dictionary<string, bool>()
        {
            { "MonsterPlant", false },
            { "Skeleton", false },
            { "Orc", false },
            { "Golem", false },
            { "EvilMage", false }
        };
        StartTutorial();
        ShowObjectives();
    }

    private void ShowObjectives()
    {
        Instantiate(prefabObjectives, mainPanel.transform);
        //Paint road/directions red(or each?) on map
    }

    public void StartTutorial()
    {
        MonsterDestroyer.Instance.BattlePanelController(true);
        Instantiate(prefabTutorialMessageBox, battlePanel.transform);
    }
}