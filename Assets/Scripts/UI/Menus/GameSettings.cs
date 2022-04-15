using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor
{
    public class GameSettings : MonoBehaviour
    {
        private void Start() {
            Application.targetFrameRate = 60;
        }
    }
}
