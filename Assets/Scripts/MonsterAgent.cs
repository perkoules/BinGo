using UnityEngine;
using UnityEngine.AI;
using Mapbox.Unity.Utilities;
using System.Collections;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterAgent : MonoBehaviour
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

    public MeshCollider parkCollider;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        parkCollider = transform.GetComponentInParent<NavMeshSourceTag>().gameObject.GetComponent<MeshCollider>();
        StartCoroutine(EnableAgent());
    }
    private IEnumerator EnableAgent()
    {
        yield return new WaitForSeconds(0.1f);
        agent.enabled = true;
        SetDestination();
        StartCoroutine(Walking());
    }
    private void Idle()
    {
        SetDestination();
        StartCoroutine(Walking());
    }

    private IEnumerator Walking()
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

    IEnumerator Running()
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
}