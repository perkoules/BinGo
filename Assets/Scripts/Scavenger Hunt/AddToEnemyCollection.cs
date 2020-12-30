using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToEnemyCollection : MonoBehaviour
{
    void Start()
    {
        ScavengerHunt.Instance.AddEnemy(this.gameObject);
    }
}
