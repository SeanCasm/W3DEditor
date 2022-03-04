using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using WEditor.Scenario;
using static WInput;
using UnityEngine.InputSystem;

namespace WEditor.CameraUtils
{

    public class BaseCamera : MonoBehaviour
    {
        [SerializeField] protected float minZoom, maxZoom;
        [SerializeField] protected float speed;
        [SerializeField] float zInitialPosition;
        protected CinemachineVirtualCamera virtualCam;
        protected WInput wInput;
        protected Vector3 pointViewCenter;
        protected void Start()
        {
            wInput = new WInput();
            pointViewCenter = EditorGrid.instance.center;
            virtualCam = GetComponentInChildren<CinemachineVirtualCamera>();
            transform.position = new Vector3(pointViewCenter.x, pointViewCenter.y, zInitialPosition);
        }
        protected void CamMove(InputAction.CallbackContext context)
        {
            Vector2 move = context.ReadValue<Vector2>();
            if (context.started)
            {
                StartCoroutine("MoveCamera", move);
            }
            else if (context.canceled)
            {
                StopCoroutine("MoveCamera");
            }
        }

    }
}
