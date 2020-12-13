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

    public delegate void TaskCompleted(string obj, bool done);
    public static event TaskCompleted OnTaskCompleted;

    public void CompleteHuntTask(GameObject go, bool completed)
    {
        string task = go.name.Replace(" Variant", "");
        taskCompletion[task] = completed;
        if (taskCompletion.Values.All(val => val == true))
        {
            OnTaskCompleted(task, true);
        }
        else
        {
            OnTaskCompleted(task, false);
        }
    }

    public void StartHunting()
    {
        playerDataSaver.SetShieldUsed(0);
        playerDataSaver.SetProjectileUsed(0);
        InitializieEnemies();
        StartTutorial();
        ShowObjectives();
    }

    public void InitializieEnemies()
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
    }

    public delegate void ShowDirections(GameObject player, List<Transform> enemies);
    public static event ShowDirections OnShowDirections;

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            List<Transform> enemiesForDirections = new List<Transform>();
            for (int i = 0; i < enemiesAdded.Count; i++)
            {
                enemiesForDirections.Add(enemiesAdded[i].transform);
            }
            OnShowDirections(player, enemiesForDirections);
        }
    }

    public void ShowObjectives()
    {
        Instantiate(prefabObjectives, mainPanel.transform);
        /*GameObject player = GameObject.FindGameObjectWithTag("Player");
        List<Transform> enemiesForDirections = new List<Transform>();
        for (int i = 0; i < enemiesAdded.Count; i++)
        {
            enemiesForDirections.Add(enemiesAdded[i].transform);
        }
        OnShowDirections(player, enemiesForDirections);*/
    }

    public void StartTutorial()
    {
        MonsterDestroyer.Instance.BattlePanelController(true);
        Instantiate(prefabTutorialMessageBox, mainPanel.transform);
    }

    
}