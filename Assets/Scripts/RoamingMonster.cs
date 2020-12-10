using Mapbox.Unity.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamingMonster : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject player;
    private Animator anim;

    private bool playerDetected = false;
    private const string ANIM_WALKING = "Walking";
    private const string ANIM_DETECTED = "PlayerDetected";
    private const string ANIM_COOLDOWN = "RunningCooldown";
    private const string ANIM_DEAD = "IsDead";

    private MeshCollider parkCollider;

    private void Awake()
    {
        MonsterDestroyer.OnMonsterClicked += MonsterDestroyer_OnMonsterClicked;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        parkCollider = transform.GetComponentInParent<NavMeshSourceTag>().gameObject.GetComponent<MeshCollider>();
        StartCoroutine(EnableAgent());
    }

    private void MonsterDestroyer_OnMonsterClicked(string rayTag, GameObject go)
    {
        if(gameObject.CompareTag(rayTag))
        {
            agent.speed = 0;
            agent.isStopped = true;
            anim.SetTrigger(ANIM_DEAD);
        }
    }
    //Triggered from Animation
    private void MonsterDestruction()
    {
        gameObject.GetComponentInChildren<Dissolve>().dying = true;
        TaskChecker.Instance.CheckTaskDone();
        MonsterDestroyer.Instance.ChangeMonsterText();
        Destroy(gameObject);
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
        if (Vector3.Distance(transform.position, player.transform.position) < 10 && !playerDetected)
        {
            playerDetected = true;
            StartCoroutine(Running());
        }
    }

    public IEnumerator Running()
    {
        anim.SetTrigger(ANIM_DETECTED);
        agent.speed = 15;
        yield return new WaitForSeconds(Random.Range(5, 10));
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
    private void OnDestroy()
    {
        MonsterDestroyer.OnMonsterClicked -= MonsterDestroyer_OnMonsterClicked;
    }
}
