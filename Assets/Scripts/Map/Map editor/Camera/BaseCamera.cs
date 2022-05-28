using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using WEditor.Scenario.Editor;

namespace WEditor.CameraUtils
{

    public class BaseCamera : MonoBehaviour
    {
        [SerializeField] protected float minZoom, maxZoom;
        [SerializeField] protected float speed;
        [SerializeField] float yInitialPosition;
        protected CinemachineVirtualCamera virtualCam;
        protected Vector3 pointViewCenter;
        public static float currentSpeed { get; set; }
        protected void Start()
        {
            currentSpeed = PlayerPrefs.HasKey("camSpeed") ? PlayerPrefs.GetFloat("camSpeed") : speed;
            pointViewCenter = EditorGrid.instance.center;
            virtualCam = GetComponentInChildren<CinemachineVirtualCamera>();
            transform.position = new Vector3(pointViewCenter.x, yInitialPosition, pointViewCenter.y);
        }
    }
}
