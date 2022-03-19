using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace WEditor.Game.Guns
{
    public class AutoFire : Gun
    {
        [SerializeField] float animationSpeedPerShoot;
        private new void Start()
        {
            base.Start();
            animator.speed = animationSpeedPerShoot;
        }
    }
}
