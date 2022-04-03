using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
using static WInput;
namespace WEditor
{
    public class PlayerControllerInput : MonoBehaviour
    {
        public static PlayerControllerInput instance;
        private WInput wInput;
        private void OnEnable()
        {
            instance = this;
            wInput = GameInput.instance.wInput;
        }
        public void EnableAndSetCallbacks(IPlayerActions callbacks)
        {
            Enable();
            wInput.Player.SetCallbacks(callbacks);
        }
        public void Enable()
        {
            wInput.Player.Enable();
        }
        public void Disable()
        {
            wInput.Player.Disable();
        }
    }
}
