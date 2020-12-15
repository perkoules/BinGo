using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private bool hit;
    private void Start()
    {
        hit = false;
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.localPosition += transform.forward * 5f * Time.deltaTime;
    }
    public delegate void ShieldDestroyed();
    public static event ShieldDestroyed OnShieldDestroyed;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("MainCamera"))
        {
            Destroy(gameObject);
            hit = true;
        }
        else if (other.gameObject.CompareTag("ShieldTag"))
        {
            //OnShieldDestroyed();
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
    private void OnDestroy()
    {
        BattleController.Instance.EnemyAttackHitResult(true, hit);
    }
}
