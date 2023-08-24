using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
using WEditor.Game.Player;
namespace WEditor
{
    public class PlayerGlobalReference : MonoBehaviour
    {
        public static PlayerGlobalReference instance;
        [SerializeField] GameObject player;
        public GunHandler GunHandler { get; private set; }
        public Inventory PlayerInventory { get; private set; }
        public Health PlayerHealth { get; private set; }
        private void Awake()
        {
            instance = this;
            GunHandler = player.GetComponentInChildren<GunHandler>();
            PlayerHealth = player.GetComponent<Health>();
            PlayerInventory = player.GetComponent<Inventory>();
        }
        void OnEnable()
        {
            EditorEvent.instance.onPreviewModeEnter += EnablePlayer;
            EditorEvent.instance.onPreviewModeExit += DisablePlayer;
            GameplayEvent.instance.onReset += EnablePlayer;
        }
        void OnDisable()
        {
            EditorEvent.instance.onPreviewModeEnter -= EnablePlayer;
            EditorEvent.instance.onPreviewModeExit -= DisablePlayer;
            GameplayEvent.instance.onReset -= EnablePlayer;
        }
        private void EnablePlayer() => player.SetActive(true);
        private void DisablePlayer() => player.SetActive(false);
        public Vector3 Position { get => player.transform.position; set => player.transform.position = value; }

    }
}