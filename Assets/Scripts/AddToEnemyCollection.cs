﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToEnemyCollection : MonoBehaviour
{
    void Start()
    {
        ScavengerHunt.Instance.AddEnemy(this.gameObject);
    }

    public void LookAtPlayer()
    {
        transform.LookAt(Camera.main.transform, Vector3.up);
    }
}
