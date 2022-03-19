using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WEditor.Game.Guns
{
    public class GunHandler : MonoBehaviour
    {
        [SerializeField] List<Gun> playerGuns;
        private Gun currentGun { get => playerGuns[gunIndex]; }
        private int playerGunsCount { get => playerGuns.Count; }
        int gunIndex = 0;
        private void Start()
        {
            playerGuns[0].Init(true);
            playerGuns.ForEach(gun =>
            {
                gun.onEmptyAmmo = TrySwapGun;
            });
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