using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static WInput;
using UnityEngine.InputSystem;
using WEditor.Events;

namespace WEditor.Game.Player.Guns
{
    public class GunHandler : MonoBehaviour, IGunActions
    {
        [SerializeField] List<Gun> playerGuns;
        public Gun currentGun { get => playerGuns[gunIndex]; }
        private int playerGunsCount { get => playerGuns.Count; }
        int gunIndex = 0;
        private void OnEnable()
        {
            GunInput.instance.EnableAndSetCallbacks(this);
            playerGuns[0].Init(true);
            playerGuns.ForEach(gun =>
            {
                gun.onEmptyAmmo = TrySwapGun;
            });
        }
        public void AddTo(int ammoID, int amount)
        {
            playerGuns[ammoID].Add(amount);
        }
        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                currentGun.FirePerformed();
            }
            else
            if (context.canceled)
            {
                currentGun.FireCanceled();
            }
        }
        public void TrySwapGun()
        {
            if (currentGun.isShooting && currentGun.onGunStoppedFire == null)
            {
                currentGun.onGunStoppedFire = SwapToGunWithAmmo;
            }
            else
            {
                SwapToGunWithAmmo();
            }
        }

        private void SwapToGunWithAmmo()
        {
            do
            {
                currentGun.Init(false);
                gunIndex++;

                if (gunIndex == playerGunsCount)
                {
                    gunIndex = 0;
                }

                currentGun.Init(true);

            } while (!currentGun.hasAmmo);
        }
    }
}