using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Player;
using WEditor.Game.Player.Guns;
namespace WEditor
{
    public class PlayerGlobalReference : MonoBehaviour
    {
        public static PlayerGlobalReference instance;
        [SerializeField] GameObject player;
        public GunHandler gunHandler { get; private set; }
        public Health playerHealth { get; private set; }
        private void Awake()
        {
            instance = this;
            gunHandler = player.GetComponentInChildren<GunHandler>();
            playerHealth = player.GetComponent<Health>();
        }
        public Vector3 position { get => player.transform.position; set => player.transform.position = value; }

    }
}