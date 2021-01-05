using System.Collections.Generic;
using UnityEngine;

public class LeafSpawner : MonoBehaviour
{
    private void Start()
    {
        InvokeRepeating("EnableLeaves", 1f, 0.5f);
    }

    private void EnableLeaves()
    {
        GameObject go = ObjectPooler.Instance.GetPooledObject("Orange Leaf");
        if (go != null)
        {
            go.SetActive(true);
        }
    }

    
}