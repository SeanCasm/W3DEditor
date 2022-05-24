using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Player.Guns;
namespace WEditor.Game.Player
{
    public class StatusBehaviour : MonoBehaviour
    {
        public static StatusBehaviour instance;
        [SerializeField] GunHandler gunHandler;
        private void Start()
        {
            instance = this;
        }
        public void Respawn()
        {
            Vector3Int pos = DataHandler.spawnPosition;
            PlayerGlobalReference.instance.position = new Vector3(pos.x, pos.y, pos.z);
            gunHandler.RefullPistolAmmo();
        }
    }
}
