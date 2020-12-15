using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    private PlayerDataSaver playerDataSaver;
    public GameObject prefabDeath;
    private MonsterDestroyer cam;
    public static Bat Instance { get; set; }

    private void OnEnable()
    {
        if(Instance !=null && Instance != this)
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
        cam = Camera.main.GetComponent<MonsterDestroyer>();
        playerDataSaver = GetComponent<PlayerDataSaver>();
        if (playerDataSaver.GetScavHunt() == 1)
        {
            gameObject.SetActive(false);
            StartCoroutine(ScavengerHunt.Instance.ContinueHunting());
            Deactivation();
        }
    }

    private void Start()
    {
        MonsterDestroyer.OnMonsterClicked += MonsterDestroyer_OnMonsterClicked;
    }

    private void MonsterDestroyer_OnMonsterClicked(string rayTag, GameObject go)
    {
        if (gameObject.CompareTag(rayTag))
        {
            Instantiate(prefabDeath, gameObject.transform);
            ScavengerHunt.Instance.StartHunting();
            gameObject.SetActive(false);
            Deactivation(); 
            Destroy(GameObject.FindGameObjectWithTag("BatEffectTag"));
        }
    }

    private void Update()
    {
        if (cam.canRaycast)
        {
            transform.LookAt(Camera.main.transform, Vector3.up);
        }
    }
    private void Deactivation()
    {
        MonsterDestroyer.OnMonsterClicked -= MonsterDestroyer_OnMonsterClicked;
    }
    private void OnDestroy()
    {
        MonsterDestroyer.OnMonsterClicked -= MonsterDestroyer_OnMonsterClicked;
    }
}
