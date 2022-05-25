using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static WInput;
using UnityEngine.InputSystem;

namespace WEditor.Game.Player.Guns
{
    public class GunHandler : MonoBehaviour, IGunActions
    {
        [SerializeField] Gun[] playerGuns;
        public Gun currentGun { get => playerGuns[gunIndex]; }
        public int[] initialAvailableGuns { get; set; }
        private int playerGunsCount { get => playerGuns.Length; }
        int gunIndex = 0;
        private void OnEnable()
        {
            GunInput.instance.EnableAndSetCallbacks(this);
            foreach (int i in initialAvailableGuns)
            {
                playerGuns[i].RefullAmmo();
                playerGuns[i].onEmptyAmmo = TrySwapGun;
            }
            gunIndex = initialAvailableGuns[0];
            playerGuns[gunIndex].Init(true);
        }
        private void OnDisable()
        {
            foreach (Gun g in playerGuns)
            {
                g.Init(false);
                g.ResetAmmo();
            }
        }
        public void RefullDefaultAmmo()
        {
            //knife
            playerGuns[0].RefullAmmo();

            // pistol
            gunIndex = 1;
            currentGun.Init(true);
            currentGun.RefullAmmo();
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
            int actualIndex = gunIndex;
            gunIndex++;

            if (gunIndex >= playerGunsCount)
                gunIndex = 0;

            while (gunIndex != actualIndex && !currentGun.hasAmmo)
            {
                if (gunIndex == playerGunsCount)
                    gunIndex = -1;

                gunIndex++;
            }
            if (!currentGun.gameObject.activeSelf)
            {
                playerGuns[actualIndex].Init(false);
                currentGun.Init(true);
            }
        }
    }
}