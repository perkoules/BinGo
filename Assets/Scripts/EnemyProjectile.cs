using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private bool hit = false;
    private void Start()
    {
        Destroy(gameObject, 5f);
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
            hit = true;
        }
        else if (other.tag == "ShieldTag")
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            hit = true;
        }
    }
    private void OnDestroy()
    {
        BattleController.Instance.EnemyAttackHitResult(true, hit);
    }
}
