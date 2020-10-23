using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LocalNavMeshLink : MonoBehaviour
{
    public List<NavMeshLink> meshLink = new List<NavMeshLink>(4);

    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    public Collider[] cols = new Collider[10];
    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshCollider.convex = true;
        meshCollider.isTrigger = true;
        /*for (int i = 0; i < 4; i++)
        {
            this.gameObject.AddComponent(typeof(NavMeshLink));
        }
        foreach (var item in GetComponents<NavMeshLink>())
        {
            meshLink.Add(item);
        }*/
    }

    void Start()
    {
        cols = Physics.OverlapSphere(meshRenderer.bounds.center, 200f);


        /*meshLink[0].startPoint = new Vector3(0f, 0f, 0f);
        meshLink[1].startPoint = new Vector3(0f, 0f, 0f);
        meshLink[2].startPoint = new Vector3(0f, 0f, 0f);
        meshLink[3].startPoint = new Vector3(0f, 0f, 0f);
        meshLink[0].endPoint = new Vector3(0, 0, 200);
        meshLink[1].endPoint = new Vector3(0, 0, -200);
        meshLink[2].endPoint = new Vector3(200, 0, 0);
        meshLink[3].endPoint = new Vector3(-200, 0, 0);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
