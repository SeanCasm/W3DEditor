using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using WEditor.Scenario;

namespace WEditor.EditorCamera
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera virtualCam;
        [SerializeField] float cameraDepth;
        // Start is called before the first frame update
        void Start()
        {
            virtualCam.Follow = EditorGrid.instance.transform;
            var pos = EditorGrid.instance.center;
            virtualCam.transform.position = new Vector3(pos.x, pos.y, cameraDepth);
        }

    }
}

