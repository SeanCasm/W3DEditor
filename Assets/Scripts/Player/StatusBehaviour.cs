using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Scenario;

namespace WEditor.Game.Player
{
    public class StatusBehaviour : MonoBehaviour
    {
        public static StatusBehaviour instance;
        [SerializeField] GunHandler gunHandler;
        [SerializeField] GameMode gameMode;
        private void Start()
        {
            instance = this;
        }
        public void Respawn()
        {
            Vector3Int pos = DataHandler.spawnPosition;
            PlayerGlobalReference.instance.position = new Vector3(pos.x, pos.y, pos.z);
            gunHandler.RefullDefaultAmmo();
            if (gameMode == GameMode.Editor)
            {
                WEditor.Scenario.Editor.ScenarioGenerator.instance.ResetLevel();
            }
            else
            {
                WEditor.Scenario.Playable.ScenarioGenerator.instance.ResetLevel();
            }
        }
    }
}
