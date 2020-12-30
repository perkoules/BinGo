using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToClueCollection : MonoBehaviour
{
    void Start()
    {
        ScavengerHunt.Instance.AddEnemy(this.gameObject, "clueBat");
    }
}
