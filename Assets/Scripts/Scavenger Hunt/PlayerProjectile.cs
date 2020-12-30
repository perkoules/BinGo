using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private bool hit;
    private void Start()
    {
        hit = false;
        //transform.position = Camera.main.transform.forward;
        Destroy(gameObject, 5f);
    }
    private void Update()
    {
        transform.localPosition += transform.forward * 25f * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MonsterHuntTag"))
        {
            hit = true;
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        BattleController.Instance.PlayerAttackHitResult(true, hit);
    }
}
