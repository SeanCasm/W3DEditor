using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WEditor.Game.Guns
{
    public class SingleFire : Gun
    {
        public override void OnFire(InputAction.CallbackContext context)
        {
            base.Fire();
        }
    }
}
