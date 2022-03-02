using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.CameraUtils
{
    public class EditorCamera : BaseCamera
    {
        new void Start()
        {
            base.Start();
            
            previewPosition = virtualCam.transform.position;
        }
    }
}
