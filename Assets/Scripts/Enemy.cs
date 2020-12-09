using UnityEngine;
using UnityEngine.AI;
using Mapbox.Unity.Utilities;
using System.Collections;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Camera player;
    private Animator anim;

    private bool attack = false;
    private const string ANIM_DEAD = "IsDead";
    private const string ANIM_GOTHIT = "GotHit";
    private const string ANIM_ATTACKMODE = "AttackMode";
    private const string ANIM_BATTLE = "ReadyToBattle";
    private const string ANIM_ATTACK = "Attack";
    private const string ANIM_REPOSITION = "Reposition";
    private const string ANIM_WON = "Won";

    public GameObject prefabAmmo, prefabDeath;
    public Transform ammoStart;
    [SerializeField] private int health;
    public float distAgent;
    public Vector3 target;
    public Canvas canvas;

    public delegate void AttackCooldown();
    public static event AttackCooldown OnAttackCooldown;

    private void Awake()
    {
        player = Camera.main;
        MonsterDestroyer.OnMonsterClicked += MonsterDestroyer_OnMonsterClicked;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(EnableAgent());
    }

    private void MonsterDestroyer_OnMonsterClicked(string rayTag)
    {
        if(gameObject.CompareTag(rayTag))
        {
            MonsterDestroyer.Instance.BattlePanelController(true);
            transform.LookAt(player.transform, Vector3.up);
            anim.SetBool(ANIM_ATTACKMODE, true);
            canvas.enabled = false;
            Idle();
        }
    }

    private IEnumerator EnableAgent()
    {
        yield return new WaitForSeconds(0.1f);
        agent.enabled = true;
    }
    private void Idle()
    {
        SetDestination();
        StartCoroutine(WalkingToPosition());
    }

    public IEnumerator WalkingToPosition()
    {
        agent.speed = 15;
        distAgent = (transform.position - agent.destination).sqrMagnitude;
        if ((transform.position - agent.destination).sqrMagnitude > 1)
        {
            yield return new WaitUntil(() => (transform.position - agent.destination).sqrMagnitude < 1);
        }
        transform.LookAt(player.transform);
        anim.SetTrigger(ANIM_BATTLE);
    }

    private void Update()
    {
        //Debug.DrawLine(transform.position, agent.destination, Color.green);
        //Debug.DrawLine(player.transform.position, agent.destination, Color.red);
        if (health > 0 && anim.GetBool(ANIM_ATTACKMODE))
        {
            if (attack)
            {
                attack = false;
                anim.SetTrigger(ANIM_ATTACK);
            }
        }
    }

    private void SetDestination()
    {
        target = Camera.main.transform.position + Camera.main.transform.forward * 50;
        if (agent.isOnNavMesh)
        {
            agent.destination = target;
        }
    }

    public void AttackAmmo()
    {
        GameObject go = Instantiate(prefabAmmo, ammoStart.position, Quaternion.identity, ammoStart);
        Destroy(go, 10f);
    }

    public void CheckHealth()
    {
        health--;
        if (health <= 0)
        {
            agent.speed = 0;
            agent.isStopped = true;
            anim.SetTrigger(ANIM_DEAD);
        }
        else
        {
            StartCoroutine(FightBack());
        }
    }

    public IEnumerator FightBack()
    {
        yield return new WaitForSeconds(3);
        attack = true;
        yield return new WaitForSeconds(3);
        FindObjectOfType<AmmoShieldController>().isPlayerTurn = true;
    }

    public void EnemyWon()
    {
        anim.SetTrigger(ANIM_WON);
    }
    //Enemy Lost
    public void DestroyObject()
    {
        Instantiate(prefabDeath, gameObject.transform);
        ScavengerHunt.Instance.CompleteHuntTask(gameObject, true);
        //Sent Ammo used to the cloud
        MonsterDestroyer.Instance.BattlePanelController(false);

        gameObject.SetActive(false); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerProjectileTag")
        {
            Destroy(other.gameObject);
            anim.SetTrigger(ANIM_GOTHIT);
        }
    }

    private void OnDestroy()
    {
        MonsterDestroyer.OnMonsterClicked -= MonsterDestroyer_OnMonsterClicked;
    }

    //Player Defeated
    public void BattleEnded()
    {
        anim.SetBool(ANIM_ATTACKMODE, false);
        canvas.enabled = true;
        MonsterDestroyer.OnMonsterClicked -= MonsterDestroyer_OnMonsterClicked;
        //Sent Ammo used to the cloud

        MonsterDestroyer.Instance.BattlePanelController(false);
    }
}