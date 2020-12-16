using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class ScavengerHunt : MonoBehaviour
{
    private PlayerDataSaver playerDataSaver;
    public List<GameObject> enemiesAdded, cluesAdded;
    public static ScavengerHunt Instance { get; private set; }
    public Dictionary<string, bool> taskCompletion;

    public GameObject prefabTutorialMessageBox;
    public GameObject prefabObjectives, mainPanel;

    private string taskCompleted = "";

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
    public void AddEnemy(GameObject go, string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            if (!cluesAdded.Exists(g => g.name == go.name))
            {
                cluesAdded.Add(go);
            }
            else
            {
                Destroy(go);
            } 
        }
    }

    public delegate void TaskCompletedDirection(GameObject obj, string tasks, bool done);
    public static event TaskCompletedDirection OnTaskCompleted;

    public void CompleteHuntTask(GameObject go, bool completed)
    {
        string enemyName = go.name.Replace(" Variant", "");
        string taskSaver = playerDataSaver.GetHuntProgress();
        char[] taskSaverArray = taskSaver.ToCharArray();
        taskCompletion[enemyName] = completed;
        for (int i = 0; i < taskCompletion.Count; i++)
        {
            if(taskCompletion.ElementAt(i).Value == true)
            {
                taskSaverArray[i] = '1';
            }
            else
            {
                taskSaverArray[i] = '0';
            }
        }
        playerDataSaver.SetHuntProgress(new string(taskSaverArray));
        taskSaver = playerDataSaver.GetHuntProgress();
        if (taskCompletion.Values.All(val => val == true))
        {
            OnTaskCompleted(go, taskSaver, true);
        }
        else
        {
            OnTaskCompleted(go, taskSaver, false);
        }
        foreach (var item in taskCompletion)
        {
            Debug.Log(item.Key + " - " + item.Value + " =>>>>>>> " + taskSaver);
        }
    }

    public void StartHunting()
    {
        playerDataSaver.SetScavHunt(1);
        playerDataSaver.SetShieldUsed(0);
        playerDataSaver.SetProjectileUsed(0);
        playerDataSaver.SetHuntProgress("00000");
        SpawnOnMap.Instance.SpawnEnemies(playerDataSaver.GetHuntProgress());
        taskCompletion = new Dictionary<string, bool>()
        {
            { "MonsterPlant", false },
            { "Skeleton", false },
            { "Orc", false },
            { "Golem", false },
            { "EvilMage", false }
        };
        StartTutorial();
        StartCoroutine(ShowObjectives());
    }

    public void ContinueHunting()
    {
        taskCompletion = new Dictionary<string, bool>()
        {
            { "MonsterPlant", false },
            { "Skeleton", false },
            { "Orc", false },
            { "Golem", false },
            { "EvilMage", false }
        };
        string taskSaver = playerDataSaver.GetHuntProgress();
        char[] taskSaverArray = taskSaver.ToCharArray();
        for (int i = 0; i < taskSaverArray.Length; i++)
        {
            if(taskSaverArray[i] == '1')
            {
                taskCompletion[taskCompletion.ElementAt(i).Key] = true;
            }
            else
            {
                taskCompletion[taskCompletion.ElementAt(i).Key] = false;
            }
        }
        SpawnOnMap.Instance.SpawnEnemies(taskSaver);
        taskCompleted = new string(taskSaverArray);
        StartCoroutine(ShowObjectives());
    }

    public delegate void ShowDirections(GameObject player, List<Transform> enemies);
    public static event ShowDirections OnShowDirections;

    public IEnumerator ShowObjectives()
    {
        Instantiate(prefabObjectives, mainPanel.transform);
        if (playerDataSaver.GetScavHunt() == 1)
        {
            HuntTasksController.Instance.ResumeHuntingInitialization(taskCompleted);
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        yield return new WaitForSeconds(2);
        List<Transform> enemiesForDirections = new List<Transform>();
        for (int i = 0; i < enemiesAdded.Count; i++)
        {
            enemiesForDirections.Add(enemiesAdded[i].transform);
        }
        OnShowDirections(player, enemiesForDirections);
    }

    public void StartTutorial()
    {
        MonsterDestroyer.Instance.BattlePanelController(true);
        Instantiate(prefabTutorialMessageBox, mainPanel.transform);
    }

    
}