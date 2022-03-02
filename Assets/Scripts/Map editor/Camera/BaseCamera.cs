using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using WEditor.Scenario;
namespace WEditor.CameraUtils
{

    public class BaseCamera : MonoBehaviour
    {
        protected CinemachineVirtualCamera virtualCam;
        protected Vector3 previewPosition;
        protected WInput wInput;
        protected Vector3 pointViewCenter;
        protected void Start()
        {
            pointViewCenter = EditorGrid.instance.center;
            virtualCam = GetComponent<CinemachineVirtualCamera>();
            virtualCam.transform.position = new Vector3(pointViewCenter.x, pointViewCenter.y, -10);
        }
        protected void OnEnable() {
            wInput = new WInput();
        }
    }
}
