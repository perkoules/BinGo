using UnityEngine;

public class ZoomableMapController : MonoBehaviour
{
    public GameObject zoomMap;
    private Camera zoomableMapCamera;
    private GameObject clone;
    public float speed = 5f;
    private float width;
    private float height;
    private Vector3 position;

    private void OnEnable()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;
        clone = Instantiate(zoomMap, new Vector3(5000, 0, 0), Quaternion.identity);
        zoomableMapCamera = clone.GetComponentInChildren<Camera>();
        zoomableMapCamera.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        Destroy(clone.gameObject);
        zoomableMapCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.GetTouch(0);
            if (myTouch.phase == TouchPhase.Moved)
            {
                Vector2 pos = myTouch.position;
                pos.x = (pos.x - width) / width;
                pos.y = (pos.y - height) / height;
                position = new Vector3(pos.x, 0.0f, pos.y);

                zoomableMapCamera.transform.position += position * speed;
            }
        }
    }
}