namespace Mapbox.Examples
{
    using UnityARInterface;
    using UnityEngine;

    public class UpdateMapPosByARPlaneY : MonoBehaviour
    {
        [SerializeField]
        private Transform _mapRoot;

        private void Start()
        {
            ARInterface.planeAdded += UpdateMapPosOnY;
            ARInterface.planeUpdated += UpdateMapPosOnY;
        }

        private void UpdateMapPosOnY(BoundedPlane plane)
        {
            var pos = _mapRoot.position;
            _mapRoot.position = new Vector3(pos.x, plane.center.y, pos.z);
        }
    }
}