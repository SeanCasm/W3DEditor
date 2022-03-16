using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WInput;

namespace WEditor
{
    public class GunInput
    {
        private WInput wInput;
        public GunInput()
        {
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
