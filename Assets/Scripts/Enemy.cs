using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Camera player;
    private Animator anim;

    private const string ANIM_DEAD = "IsDead";
    private const string ANIM_GOTHIT = "GotHit";
    private const string ANIM_ATTACKMODE = "AttackMode";
    private const string ANIM_BATTLE = "ReadyToBattle";
    private const string ANIM_ATTACK = "Attack";
    private const string ANIM_REPOSITION = "Reposition";
    private const string ANIM_WON = "Won";
    private Vector3 target;

    public GameObject prefabAmmo, prefabDeath;
    public Transform ammoStart;
    public int health;
    public float agentDistance;
    public bool isReadyToBattle = false;
    public Canvas canvas;

    private void Awake()
    {
        player = Camera.main;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(EnableAgent());
    }

    private IEnumerator EnableAgent()
    {
        yield return new WaitForSeconds(0.1f);
        agent.enabled = true;
    }

    public void PrepareEnemyForBattle()
    {
        transform.LookAt(player.transform, Vector3.up);
        anim.SetBool(ANIM_ATTACKMODE, true);
        canvas.enabled = false;
        Idle();
    }

    private void Idle()
    {
        SetDestination();
        StartCoroutine(WalkingToPosition());
    }
    public IEnumerator WalkingToPosition()
    {
        agent.speed = 15;
        agentDistance = (transform.position - agent.destination).sqrMagnitude;
        if (agentDistance > 1)
        {
            yield return new WaitUntil(() => agentDistance < 1);
        }
        transform.LookAt(player.transform);
        anim.SetTrigger(ANIM_BATTLE);
        isReadyToBattle = true;
    }
    private void Update()
    {
        if (agent.hasPath)
        {
            agentDistance = (transform.position - agent.destination).sqrMagnitude;
        }
    }
    public bool Reposition()
    {
        if (!agent.hasPath)
        {
            anim.SetTrigger(ANIM_REPOSITION);
            Idle();
            return true;
        }
        return false;
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
    }

    public bool TakeDamage()
    {
        anim.SetTrigger(ANIM_GOTHIT);
        health--;
        if (health <= 0)
        {
            agent.speed = 0;
            agent.isStopped = true;
            return true;
        }
        return false;
    }
    
    public void EnemyWon()
    {
        anim.SetTrigger(ANIM_WON);        
        anim.SetBool(ANIM_ATTACKMODE, false);
        canvas.enabled = true;
    }

    public void EnemyLost()
    {
        anim.SetTrigger(ANIM_DEAD);
    }

    public void DestroyObject()
    {
        Instantiate(prefabDeath, gameObject.transform);
        ScavengerHunt.Instance.CompleteHuntTask(gameObject, true);

        gameObject.SetActive(false);
    }    
}