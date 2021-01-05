using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; set; }
    public GameObject leavesHolder;
    public PolygonCollider2D col;

    public List<ObjectPoolItem> itemsToPool;

    public List<GameObject> pooledObjects;

    private void OnEnable()
    {
        Instance = this;
    }
    private void Awake()
    {
        pooledObjects = new List<GameObject>();
        foreach (var item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject leaf = Instantiate(item.objectToPool, WhereToSpawn(col.bounds), Quaternion.identity, leavesHolder.transform);
                leaf.SetActive(false);
                pooledObjects.Add(leaf);
            }
        }
    }
    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].CompareTag(tag))
            {
                return pooledObjects[i];
            }
        }
        foreach (var item in itemsToPool)
        {
            if (item.objectToPool.CompareTag(tag))
            {

            }
        }
        return null;
    }
    public void Reposition(GameObject go)
    {
        go.transform.position = WhereToSpawn(col.bounds);
    }
    public Vector3 WhereToSpawn(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z));
    }
}
