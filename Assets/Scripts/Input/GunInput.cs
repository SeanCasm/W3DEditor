using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WInput;

namespace WEditor
{
    public class GunInput : MonoBehaviour
    {
        public static GunInput instance;
        private WInput wInput;
        private void Start()
        {
            instance=this;
            wInput = GameInput.instance.wInput;
        }
        public void EnableAndSetCallbacks(IGunActions callbacks)
        {
            wInput.Gun.Enable();
            wInput.Gun.SetCallbacks(callbacks);
        }
        public void Disable()
        {
            wInput.Gun.Disable();
        }
    }
}
