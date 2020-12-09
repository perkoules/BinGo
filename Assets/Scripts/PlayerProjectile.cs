using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private void Start()
    {
        transform.position = Camera.main.transform.forward;
    }
    private void Update()
    {
        transform.localPosition += transform.forward * 10f * Time.deltaTime;
    }
    private void OnDestroy()
    {
        FindObjectOfType<AmmoShieldController>().isPlayerTurn = true;
    }
}
