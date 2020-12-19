using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject prefabAttack;

    public void InitiateAttack()
    {
        Instantiate(prefabAttack, Camera.main.transform.position, Camera.main.transform.rotation);
    }
}