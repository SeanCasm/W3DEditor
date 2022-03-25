using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Player;

namespace WEditor
{
    public class PlayerGlobalReference : MonoBehaviour
    {
        public static PlayerGlobalReference instance;
        [SerializeField] GameObject player;
        private void Start()
        {
            instance = this;
        }
        public Vector3 position { get => player.transform.position; set => player.transform.position = value; }

    }
}