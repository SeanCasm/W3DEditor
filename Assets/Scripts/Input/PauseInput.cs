using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WInput;

namespace WEditor
{
    public class PauseInput : MonoBehaviour
    {
        public static PauseInput instance;
        private WInput wInput;
        private void Start()
        {
            instance = this;
            wInput = GameInput.instance.wInput;
        }
        public void EnableAndSetCallbacks(IPauseActions callbacks)
        {
            wInput.Pause.Enable();
            wInput.Pause.SetCallbacks(callbacks);
        }
        public void Disable()
        {
            wInput.Pause.Disable();
        }
    }
}
