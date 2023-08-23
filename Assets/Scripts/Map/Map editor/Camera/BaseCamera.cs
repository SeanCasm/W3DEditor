using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using WEditor.Scenario.Editor;

namespace WEditor.CameraUtils
{

    public class BaseCamera : MonoBehaviour
    {
        [Tooltip("Min zoom of the camera represents the y axis MAXIMUM ALTITUDE")]
        [SerializeField] protected float minZoom = 28;
        [Tooltip("Max zoom of the camera represents the y axis MINIMUM ALTITUDE")]
        [SerializeField] protected float maxZoom;
        [SerializeField] protected float speed;
        [SerializeField] float yInitialPosition;
        protected Vector3 pointViewCenter;
        public static float currentSpeed { get; set; }
        protected void Start()
        {
            currentSpeed = PlayerPrefs.HasKey("camSpeed") ? PlayerPrefs.GetFloat("camSpeed") : speed;
            pointViewCenter = EditorGrid.instance.center;
            transform.position = new Vector3(pointViewCenter.x, yInitialPosition, pointViewCenter.y);
        }
    }
}
