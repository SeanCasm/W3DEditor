using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.Game.Player
{
    public class PlayerGlobalReference : MonoBehaviour
    {
        public static PlayerGlobalReference instance;
        [SerializeField] PlayerController playerController;
        // public IPlayerView viewReference { get; set; }
        private void Start()
        {
            instance = this;
            // viewReference = GetComponent<IPlayerView>();
        }
        public Vector3 playerPosition{get=>playerController.transform.position;set=>playerController.transform.position = value;}
    }
}
