namespace Mapbox.Examples
{
    using UnityEngine;

    namespace Scripts.Utilities
    {
        public class DragRotate : MonoBehaviour
        {
            [SerializeField]
            private Transform _objectToRotate;

            [SerializeField]
            private float _multiplier;

            private Vector3 _startTouchPosition;

            private void Update()
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _startTouchPosition = Input.mousePosition;
                }

                if (Input.GetMouseButton(0))
                {
                    var dragDelta = Input.mousePosition - _startTouchPosition;
                    var axis = new Vector3(0f, -dragDelta.x * _multiplier, 0f);
                    _objectToRotate.RotateAround(_objectToRotate.position, axis, _multiplier);
                }
            }
        }
    }
}