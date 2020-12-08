using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public GameObject prefabDeath;
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
    private void Start()
    {
        MonsterDestroyer.OnMonsterClicked += MonsterDestroyer_OnMonsterClicked;
    }

    private void MonsterDestroyer_OnMonsterClicked(string rayTag)
    {
        if (gameObject.CompareTag(rayTag))
        {
            Instantiate(prefabDeath, gameObject.transform);
            ScavengerHunt.Instance.StartHunting();
            gameObject.SetActive(false);
            Deactivation(); 
        }
    }

    private void Update()
    {
        if (Camera.main.isActiveAndEnabled)
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
