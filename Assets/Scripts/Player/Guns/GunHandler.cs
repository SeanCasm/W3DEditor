using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WInput;
using UnityEngine.InputSystem;
namespace WEditor.Game.Player
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
            initialAvailableGuns = DataHandler.levelGuns;
            GunInput.instance.EnableAndSetCallbacks(this);
            foreach (int i in initialAvailableGuns)
            {
                playerGuns[i].RefullAmmo();
                playerGuns[i].onEmptyAmmo = SwapToGunWithAmmo;
            }
            gunIndex = initialAvailableGuns[0];
            playerGuns[gunIndex].Init(true);
        }
        private void OnDisable()
        {
            foreach (Gun g in playerGuns)
            {
                g.Init(false);
                g.RefullAmmo();
            }
            GunInput.instance.Disable();
        }
        public void SetDefault()
        {
            gunIndex = 0;
            currentGun.Init(true);
            for (int i = 1; i < playerGunsCount; i++)
            {
                playerGuns[i].RefullAmmo();
                playerGuns[i].Init(false);
            }
        }

        public void AddTo(int amount) => currentGun.Add(amount);
        public void AddGun(int index)
        {
            playerGuns[index].RefullAmmo();
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
            int actualIndex = gunIndex;
            while (true)
            {
                gunIndex++;
                if (gunIndex >= playerGunsCount) gunIndex = 0;
                if (currentGun.hasAmmo || currentGun is Knife)
                {
                    break;
                }
            }
            if (!currentGun.gameObject.activeSelf)
            {
                playerGuns[actualIndex].Init(false);
                currentGun.Init(true);
            }
        }
    }
}