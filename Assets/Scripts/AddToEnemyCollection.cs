using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToEnemyCollection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnemyCollection.Instance.AddEnemy(this.gameObject);
    }

    public void LookAtPlayer()
    {
        transform.LookAt(Camera.main.transform, Vector3.up);
    }
}
