using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.localPosition += transform.forward * 10f * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    { 
        if(other.tag == "Player" || other.tag == "MainCamera")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "ShieldTag")
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
