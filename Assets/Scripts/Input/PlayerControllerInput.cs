using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WInput;
namespace WEditor
{
    public class PlayerControllerInput
    {
        private WInput wInput;
        public PlayerControllerInput()
        {
            wInput = GameInput.instance.wInput;
        }
        public void EnableAndSetCallbacks(IPlayerActions callbacks)
        {
            wInput.Player.Enable();
            wInput.Player.SetCallbacks(callbacks);
        }
        public void Disable()
        {
            wInput.Player.Disable();
        }
    }
}
