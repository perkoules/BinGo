using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityARInterface;
using Mapbox.Unity.MeshGeneration.Interfaces;

public class PlaceOnPlane : ARBase
{
    [SerializeField]
    private GameObject m_ObjectToPlace;

    void Update ()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var camera = GetCamera();

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            int layerMask = 1 << LayerMask.NameToLayer("ARGameObject"); // Planes are in layer ARGameObject

            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, float.MaxValue, layerMask))
                Instantiate(m_ObjectToPlace, rayHit.point, Quaternion.identity);
            //m_ObjectToPlace.transform.position = rayHit.point;
        }
#endif
#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            Touch myTouch = Input.GetTouch(0);
            
            var camera = GetCamera();

            Ray ray = camera.ScreenPointToRay(myTouch.position);

            int layerMask = 1 << LayerMask.NameToLayer("ARGameObject"); // Planes are in layer ARGameObject

            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, float.MaxValue, layerMask))
                Instantiate(m_ObjectToPlace, rayHit.point, Quaternion.identity);
            //m_ObjectToPlace.transform.position = rayHit.point;
        }
#endif
    }
}
