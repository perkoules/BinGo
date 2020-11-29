using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionTest : MonoBehaviour
{
    public List<GameObject> enemiesAdded;
    public static CollectionTest Instance { get; private set; }
    private void OnEnable()
    {
        if(Instance != null && Instance != this)
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
        enemiesAdded = new List<GameObject>();
    }

    public void AddEnemy(GameObject go)
    {
        if(!enemiesAdded.Exists(g => g.name == go.name))
        {
            enemiesAdded.Add(go);
        }
    }
}
