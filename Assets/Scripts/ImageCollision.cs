using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCollision : MonoBehaviour
{
    public Material matG, matR;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("in"))
        {
            this.gameObject.GetComponent<MeshRenderer>().material = matG;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("in"))
        {
            this.gameObject.GetComponent<MeshRenderer>().material = matR;
        }
    }
}
