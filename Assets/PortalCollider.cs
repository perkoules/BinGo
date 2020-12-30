using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCollider : MonoBehaviour
{
    private RePositionCamera repos;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            if(repos == null)
            {
                repos = FindObjectOfType<RePositionCamera>();
            }
            repos.ReCenter();
        }
    }
}
