using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WInput;
using UnityEngine.InputSystem;
using System;

namespace WEditor.Game.Player
{
    public class GunHandler : MonoBehaviour, IGunActions
    {
        [SerializeField] List<Gun> playerGuns = new();
        public Gun currentGun { get => playerGuns[gunIndex]; }
        private List<int> initialGunsIndexes = new() { 0 };
        int gunIndex = 0;
        private void OnEnable()
        {
            InitGuns();
        }
        private void OnDisable()
        {
            playerGuns.ForEach(g => g.Reset());
            gunIndex = 0;
            GunInput.instance.Disable();
        }
        private void InitGuns()
        {
            if (DataHandler.levelGunIndex == 0) //all
            {
                initialGunsIndexes = new() { 0, 1, 2, 3 };
            }
            else
                initialGunsIndexes = new() { 0, DataHandler.levelGunIndex };
            foreach (int i in initialGunsIndexes)
            {
                playerGuns[i].RefullAmmo();
                playerGuns[i].onEmptyAmmo = SwapToGunWithAmmo;
            }
            GunInput.instance.EnableAndSetCallbacks(this);

            gunIndex = 0;
            //knife
            playerGuns[gunIndex].Enable();
        }
        public void AddTo(int amount) => currentGun.Add(amount);
        public void AddGun(int index)
        {
            currentGun.Disable();
            gunIndex = index;
            currentGun.SwapToThisFromGround();
            currentGun.onEmptyAmmo = SwapToGunWithAmmo;
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

        public void SwapToGunWithAmmo()
        {
            if (currentGun.IsShooting)
                return;
            currentGun.Disable();
            while (true)
            {
                gunIndex++;
                if (gunIndex >= playerGuns.Count) gunIndex = 0;
                if (currentGun.HasAmmo || currentGun is Knife)
                {
                    break;
                }
            }
            if (!currentGun.gameObject.activeSelf)
                currentGun.Enable();
        }
    }
}