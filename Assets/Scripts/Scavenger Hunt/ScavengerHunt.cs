using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Mapbox.Unity.Map;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class ScavengerHunt : MonoBehaviour
{
    private PlayerDataSaver playerDataSaver;
    public List<GameObject> enemiesAdded, cluesAdded;
    public static ScavengerHunt Instance { get; private set; }
    public Dictionary<string, bool> taskCompletion;

    public GameObject prefabTutorialMessageBox;
    public GameObject prefabObjectives, mainPanel;

    private string taskCompleted = "";
    public Button camBtn;

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
    private void Start()
    {
        if (playerDataSaver.GetScavHunt() == 1)
        {
            CheckHuntDate();
        }
    }

    private void CheckHuntDate()
    {
        string dateStarted = "";
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() { },
        result =>
        {
            if (result.Data == null) Debug.Log("No Data");
            else
            {
                dateStarted = result.Data["HuntStartedOn"].Value;
                DateTime dateSt = DateTime.Parse(dateStarted);
                DateTime today = DateTime.Today;
                int daysPassed = (today - dateSt).Days;
                if (daysPassed >= 7)
                {
                    playerDataSaver.SetScavHunt(2);
                }
                else
                {
                    SpawnOnMap.Instance.map.OnInitialized += ContinueHunting;
                }
            }
        },
        error => Debug.Log(error.GenerateErrorReport()));
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
        RememberHuntDate();
        camBtn.onClick.Invoke();
        playerDataSaver.SetScavHunt(1);
        playerDataSaver.SetShieldUsed(0);
        playerDataSaver.SetProjectileUsed(0);
        SpawnOnMap.Instance.map.OnUpdated += Spawn;
        //SpawnOnMap.Instance.map.OnTilesStarting += Map_OnTilesStarting;
        taskCompletion = new Dictionary<string, bool>()
        {
            { "MonsterPlant", false },
            { "Skeleton", false },
            { "Orc", false },
            { "Golem", false },
            { "EvilMage", false }
        };
        StartTutorial();
    }

    private void RememberHuntDate()
    {
        string dateStarted = "";
        DateTime today = DateTime.Now;
        dateStarted = today.ToString("d");
        PlayFabClientAPI.UpdateUserData(
           new UpdateUserDataRequest
           {
               Data = new Dictionary<string, string>() { { "HuntStartedOn", dateStarted } },
               Permission = UserDataPermission.Public
           },
           result => Debug.Log("Monster hunt started on " + dateStarted),
           error => Debug.Log(error.GenerateErrorReport())); ;
    }

    private void Map_OnTilesStarting(List<Mapbox.Map.UnwrappedTileId> obj)
    {
        SpawnOnMap.Instance.map.OnTilesStarting -= Map_OnTilesStarting;
        SpawnOnMap.Instance.SpawnEnemies("00000");
        playerDataSaver.SetHuntProgress("00000");
        StartCoroutine(ShowObjectives());
    }

    private void Spawn()
    {
        SpawnOnMap.Instance.map.OnUpdated -= Spawn;
        SpawnOnMap.Instance.SpawnEnemies("00000");
        playerDataSaver.SetHuntProgress("00000");
        StartCoroutine(ShowObjectives());
    }

    public void ContinueHunting()
    {
        SpawnOnMap.Instance.map.OnInitialized -= ContinueHunting;
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
        if (taskSaver != "11111")
        {
            StartCoroutine(ShowObjectives());
        }
        else if (taskSaver == "11111")
        {
            Debug.Log("NO SHOW");
        }
        SpawnOnMap.Instance.map.UpdateMap();
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