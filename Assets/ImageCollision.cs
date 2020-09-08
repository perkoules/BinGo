using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError(other.gameObject.name);
        /*if (other.gameObject.name)
        {

        }*/
    }
}
