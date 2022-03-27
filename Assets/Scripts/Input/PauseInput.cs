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
        private void OnEnable()
        {
            instance = this;
            wInput = GameInput.instance.wInput;
        }
        public void EnableAndSetCallbacks(IPauseActions callbacks)
        {
            wInput.Pause.Enable();
            wInput.Pause.SetCallbacks(callbacks);
        }
        public void Enable()
        {
            wInput.Pause.Enable();
        }
        public void Disable()
        {
            wInput.Pause.Disable();
        }
    }
}
