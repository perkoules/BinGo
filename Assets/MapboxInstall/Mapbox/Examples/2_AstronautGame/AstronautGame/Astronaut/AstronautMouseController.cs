using Mapbox.Unity.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mapbox.Examples
{
    public class AstronautMouseController : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField]
        private GameObject character;

        [SerializeField]
        private float characterSpeed;

        [SerializeField]
        private Animator characterAnimator;

        [Header("References")]
        [SerializeField]
        private AstronautDirections directions;

        [SerializeField]
        private Transform startPoint;

        [SerializeField]
        private Transform endPoint;

        [SerializeField]
        private AbstractMap map;

        [SerializeField]
        private GameObject rayPlane;

        [SerializeField]
        private Transform _movementEndPoint;

        [SerializeField]
        private LayerMask layerMask;

        private Ray ray;
        private RaycastHit hit;
        private LayerMask raycastPlane;
        private float clicktime;
        private bool moving;
        private bool characterDisabled;

        private void Start()
        {
            characterAnimator = GetComponentInChildren<Animator>();
            if (!Application.isEditor)
            {
                this.enabled = false;
                return;
            }
        }

        private void Update()
        {
            if (characterDisabled)
                return;

            CamControl();

            bool click = false;

            if (Input.GetMouseButtonDown(0))
            {
                clicktime = Time.time;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (Time.time - clicktime < 0.15f)
                {
                    click = true;
                }
            }

            if (click)
            {
                ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    startPoint.position = transform.localPosition;
                    endPoint.position = hit.point;
                    MovementEndpointControl(hit.point, true);

                    directions.Query(GetPositions, startPoint, endPoint, map);
                }
            }
        }

        #region Character : Movement

        private List<Vector3> futurePositions;
        private bool interruption;

        private void GetPositions(List<Vector3> vecs)
        {
            futurePositions = vecs;

            if (futurePositions != null && moving)
            {
                interruption = true;
            }
            if (!moving)
            {
                interruption = false;
                MoveToNextPlace();
            }
        }

        private Vector3 nextPos;

        private void MoveToNextPlace()
        {
            if (futurePositions.Count > 0)
            {
                nextPos = futurePositions[0];
                futurePositions.Remove(nextPos);

                moving = true;
                characterAnimator.SetBool("IsWalking", true);
                StartCoroutine(MoveTo());
            }
            else if (futurePositions.Count <= 0)
            {
                moving = false;
                characterAnimator.SetBool("IsWalking", false);
            }
        }

        private Vector3 prevPos;

        private IEnumerator MoveTo()
        {
            prevPos = transform.localPosition;

            float time = CalculateTime();
            float t = 0;

            StartCoroutine(LookAtNextPos());

            while (t < 1 && !interruption)
            {
                t += Time.deltaTime / time;

                transform.localPosition = Vector3.Lerp(prevPos, nextPos, t);

                yield return null;
            }

            interruption = false;
            MoveToNextPlace();
        }

        private float CalculateTime()
        {
            float timeToMove = 0;

            timeToMove = Vector3.Distance(prevPos, nextPos) / characterSpeed;

            return timeToMove;
        }

        #endregion Character : Movement

        #region Character : Rotation

        private IEnumerator LookAtNextPos()
        {
            Quaternion neededRotation = Quaternion.LookRotation(nextPos - character.transform.position);
            Quaternion thisRotation = character.transform.localRotation;

            float t = 0;
            while (t < 1.0f)
            {
                t += Time.deltaTime / 0.25f;
                var rotationValue = Quaternion.Slerp(thisRotation, neededRotation, t);
                character.transform.rotation = Quaternion.Euler(0, rotationValue.eulerAngles.y, 0);
                yield return null;
            }
        }

        #endregion Character : Rotation

        #region CameraControl

        [Header("CameraSettings")]
        [SerializeField]
        private Camera cam;

        private Vector3 previousPos = Vector3.zero;
        private Vector3 deltaPos = Vector3.zero;

        private void CamControl()
        {
            deltaPos = transform.position - previousPos;
            deltaPos.y = 0;
            cam.transform.position = Vector3.Lerp(cam.transform.position, cam.transform.position + deltaPos, Time.time);
            previousPos = transform.position;
        }

        #endregion CameraControl

        #region Utility

        public void DisableCharacter()
        {
            characterDisabled = true;
            moving = false;
            StopAllCoroutines();
            character.SetActive(false);
        }

        public void EnableCharacter()
        {
            characterDisabled = false;
            character.SetActive(true);
        }

        public void LayerChangeOn()
        {
            Debug.Log("OPEN");
        }

        public void LayerChangeOff()
        {
            Debug.Log("CLOSE");
        }

        private void MovementEndpointControl(Vector3 pos, bool active)
        {
            _movementEndPoint.position = new Vector3(pos.x, 0.2f, pos.z);
            _movementEndPoint.gameObject.SetActive(active);
        }

        #endregion Utility
    }
}