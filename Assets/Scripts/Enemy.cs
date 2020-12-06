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
    private Vector2 distanceRange;
    private NavMeshAgent agent;
    private GameObject player;
    private Animator anim;

    private bool playerDetected = false;
    private const string ANIM_WALKING = "Walking";
    private const string ANIM_DETECTED = "PlayerDetected";
    private const string ANIM_COOLDOWN = "RunningCooldown";
    private const string ANIM_DEAD = "IsDead";
    private const string ANIM_GOTHIT = "GotHit";

    private MeshCollider parkCollider;
    public GameObject prefabAmmo, prefabDeath;
    public Transform ammoStart;
    [SerializeField] private int health; 

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        MonsterDestroyer.OnMonsterClicked += MonsterDestroyer_OnMonsterClicked;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        parkCollider = transform.GetComponentInParent<NavMeshSourceTag>().gameObject.GetComponent<MeshCollider>();
        StartCoroutine(EnableAgent());
    }

    private void MonsterDestroyer_OnMonsterClicked()
    {
        if(gameObject.tag == "MonsterHuntTag")
        {

        }
    }

    private IEnumerator EnableAgent()
    {
        yield return new WaitForSeconds(0.1f);
        agent.enabled = true;
        Idle();
    }
    private void Idle()
    {
        SetDestination();
        StartCoroutine(Walking());
    }

    public IEnumerator Walking()
    {
        //Debug.DrawLine(transform.position, agent.destination, Color.red);  // Destination        
        anim.SetBool(ANIM_WALKING, true);
        if ((transform.position - agent.destination).sqrMagnitude > 1)
        {
            yield return new WaitUntil(() => (transform.position - agent.destination).sqrMagnitude < 1);
        }
        if (!playerDetected)
        {
            anim.SetBool(ANIM_WALKING, false);
            yield return new WaitForSeconds(3);
            Idle();
        }
        else
        {
            Idle();
        }
    }

    private void Update()
    {
        if (anim.GetBool(ANIM_DEAD))
        {
            agent.speed = 0;
            agent.isStopped = true;
        }
        else if (Vector3.Distance(transform.position, player.transform.position) < 10 && !playerDetected)
        {
            playerDetected = true;
            StartCoroutine(Running());            
        }
    }

    public IEnumerator Running()
    {
        anim.SetTrigger(ANIM_DETECTED);
        agent.speed = 15;
        yield return new WaitForSeconds(Random.Range(5,10));
        playerDetected = false;
        anim.SetTrigger(ANIM_COOLDOWN);
        agent.speed = 5;
        Idle();
    }

    private void SetDestination()
    {       
        var target = (Random.insideUnitCircle * Random.Range(parkCollider.bounds.size.x, parkCollider.bounds.size.z)).ToVector3xz() + transform.position;
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
            anim.SetTrigger(ANIM_DEAD);
        }
    }

    public void DestroyObject()
    {
        Instantiate(prefabDeath, gameObject.transform);
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerProjectileTag")
        {
            Destroy(other.gameObject);
            anim.SetTrigger(ANIM_GOTHIT);
        }
    }
}