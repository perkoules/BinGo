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

}
