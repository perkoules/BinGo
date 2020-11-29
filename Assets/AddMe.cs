using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CollectionTest.Instance.AddEnemy(this.gameObject);
    }

}
