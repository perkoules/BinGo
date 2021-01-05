using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafDestruction : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
        ObjectPooler.Instance.Reposition(collision.gameObject);
    }
}
