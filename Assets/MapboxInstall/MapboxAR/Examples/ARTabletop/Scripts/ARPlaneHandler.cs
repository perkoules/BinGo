using System;
using UnityEngine;

namespace UnityARInterface
{
    public class ARPlaneHandler : MonoBehaviour
    {
        public static Action resetARPlane;
        public static Action<BoundedPlane> returnARPlane;
        private string _planeId;
        private BoundedPlane _cachedARPlane;

        private void Start()
        {
            ARInterface.planeAdded += UpdateARPlane;
            ARInterface.planeUpdated += UpdateARPlane;
        }

        private void UpdateARPlane(BoundedPlane arPlane)
        {
            if (_planeId == null)
            {
                _planeId = arPlane.id;
            }

            if (arPlane.id == _planeId)
            {
                _cachedARPlane = arPlane;
            }

            returnARPlane(_cachedARPlane);
        }
    }
}