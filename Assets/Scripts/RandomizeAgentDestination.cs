using UnityEngine;
using UnityEngine.AI;
using Mapbox.Unity.Utilities;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class RandomizeAgentDestination : MonoBehaviour
{
    [SerializeField]
    Vector2 _timeRange;

    [SerializeField]
    Vector2 _distanceRange;

    NavMeshAgent _agent;

    float _elapsedTime;
    float _waitTime;
    bool _isFocused;


    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        StartCoroutine(EnableAgent());
        SetDestination();
    }
    IEnumerator EnableAgent()
    {
        yield return new WaitForSeconds(0.1f);
        _agent.enabled = true;
    }
    void Update()
    {
        Debug.DrawLine(transform.position, _agent.destination, Color.red);
        if (_elapsedTime >= _waitTime || (transform.position - _agent.destination).sqrMagnitude < 1)
        {
            _elapsedTime = 0f;
            _waitTime = Random.Range(_timeRange.x, _timeRange.y);
            SetDestination();
        }

        _elapsedTime += Time.deltaTime;
    }

    void SetDestination()
    {
        var target = (Random.insideUnitCircle * Random.Range(_distanceRange.x, _distanceRange.y)).ToVector3xz() + transform.position;
        if (_agent.isOnNavMesh)
        {
            _agent.destination = target;
        }
    }
}