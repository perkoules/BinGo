using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScavengerHunt : MonoBehaviour
{
    private PlayerDataSaver playerDataSaver;
    public List<GameObject> enemiesAdded;
    public static ScavengerHunt Instance { get; private set; }
    public Dictionary<string, bool> taskCompletion;

    public GameObject prefabTutorialMessageBox;
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
        playerDataSaver = GetComponent<PlayerDataSaver>();
        if(playerDataSaver.GetScavHunt() == 1)
        {
            StartCoroutine(InitializeScavengerHunt());
        }
    }

    IEnumerator InitializeScavengerHunt()
    {
        yield return new WaitUntil(() => enemiesAdded.Count > 4);
        ShowObjectives();
        InitializieEnemies();
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
        InitializieEnemies();
        StartTutorial();
        ShowObjectives();
    }

    public void InitializieEnemies()
    {
        Debug.Log("Start Hunting");
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
    }

    public void ShowObjectives()
    {
        Instantiate(prefabObjectives, mainPanel.transform);
        //Paint road/directions red(or each?) on map
    }

    public void StartTutorial()
    {
        MonsterDestroyer.Instance.BattlePanelController(true);
        Instantiate(prefabTutorialMessageBox, mainPanel.transform);
    }
    
    public void AmmoShieldholder()
    {

    }
}