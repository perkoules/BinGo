using Mapbox.Unity.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraZoom : MonoBehaviour
{
    public delegate void ZoomChanged(float currentZoom);
    public static event ZoomChanged OnZoomChanged;
    public Camera mapCamera;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float minZoom, maxZoom;
    private void Update()
    {
        if(Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            Vector2 t1Dist = t1.position - t1.deltaPosition;
            Vector2 t2Dist = t2.position - t2.deltaPosition;

            float oldTouchDistance = Vector2.Distance(t1Dist, t2Dist);
            float currentTouchDistance = Vector2.Distance(t1.position, t2.position);
            float deltaDistance = oldTouchDistance - currentTouchDistance;

            mapCamera.orthographicSize += deltaDistance * speed;
            if(mapCamera.orthographicSize < minZoom)
            {
                mapCamera.orthographicSize = minZoom;
            }
            else if (mapCamera.orthographicSize > maxZoom)
            {
                mapCamera.orthographicSize = maxZoom;
            }
            OnZoomChanged(mapCamera.orthographicSize);
            FindObjectOfType<AbstractMap>().UpdateMap();
        }
    }
}
