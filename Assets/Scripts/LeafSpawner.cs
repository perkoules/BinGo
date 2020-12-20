using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafSpawner : MonoBehaviour
{
    public GameObject leavesHolder, leafPrefab;
    private PolygonCollider2D col;
    private void Awake()
    {
        col = leavesHolder.GetComponent<PolygonCollider2D>();
    }

    void Start()
    {
        InvokeRepeating("Spawner", 0.1f, 3f); 
    }

    private void Spawner()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject go = Instantiate(leafPrefab, WhereToSpawn(col.bounds), Quaternion.identity, leavesHolder.transform);
        }
    }

    public Vector3 WhereToSpawn(Bounds bounds)
    {
        return new Vector3(
        Random.Range(bounds.min.x, bounds.max.x),
        Random.Range(bounds.min.y, bounds.max.y),
        Random.Range(bounds.min.z, bounds.max.z)
    );
    }

}
