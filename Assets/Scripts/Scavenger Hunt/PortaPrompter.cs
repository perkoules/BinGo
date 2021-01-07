using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PortaPrompter : MonoBehaviour
{
    public TextMeshProUGUI myText;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera") || other.gameObject.CompareTag("Player"))
        {
            myText.gameObject.SetActive(true);
            if (Camera.main.enabled && GetComponent<SphereCollider>().enabled)
            {
                GetComponent<SphereCollider>().enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera") || other.gameObject.CompareTag("Player"))
        {
            myText.gameObject.SetActive(false);
        }
    }
}
