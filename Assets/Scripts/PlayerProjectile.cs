using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private bool hit = false;
    private void Start()
    {
        transform.position = Camera.main.transform.forward;
        Destroy(gameObject, 5f);
    }
    private void Update()
    {
        transform.localPosition += transform.forward * 10f * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MonsterHuntTag")
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
