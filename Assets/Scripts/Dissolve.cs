using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private SkinnedMeshRenderer meshRenderer;
    public float speed = .5f;

    private void Start()
    {
        meshRenderer = this.GetComponent<SkinnedMeshRenderer>();
    }

    private float t = 0.0f;
    public bool dying = false;
    public void Update()
    {
        if (dying)
        {
            Material[] mats = meshRenderer.materials;

            mats[0].SetFloat("_Cutoff", Mathf.Sin(t * speed));
            if(mats[0].GetFloat("_Cutoff") >= 0.95f)
            {
                dying = false;
            }
            t += Time.deltaTime;
            // Unity does not allow meshRenderer.materials[0]...
            meshRenderer.materials = mats;
        }
    }
}
