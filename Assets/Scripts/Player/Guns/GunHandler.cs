using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static WInput;

namespace WEditor.Game.Guns
{
    public class GunHandler : MonoBehaviour
    {
        [SerializeField] Gun pistol, knife;
        Gun currentGun;
        List<Gun> guns = new List<Gun>();
        int gunIndex = 0;
        private void Start()
        {
            guns.Add(pistol);
            guns.Add(knife);
        }

        public void SwapGun()
        {
            currentGun = guns[gunIndex++];
            while (!currentGun.hasAmmo)
            {
                gunIndex++;
                currentGun = guns[gunIndex];
                Mathf.Clamp(gunIndex, 0, guns.Count);
            }
        }
    }
}
