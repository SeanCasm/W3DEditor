using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Scenario;

namespace WEditor.Game.Player
{
    /// <summary>
    /// Handle the player spawn at dead
    /// </summary>
    public class StatusBehaviour : MonoBehaviour
    {
        public static StatusBehaviour instance;
        [SerializeField] GunHandler gunHandler;
        private void Start() => instance = this;
        public void Respawn()
        {
            Vector3Int pos = DataHandler.spawnPosition;
            PlayerGlobalReference.instance.position = new Vector3(pos.x, pos.y, pos.z);
            gunHandler.RefullDefaultAmmo();
            if (SceneHandler.instance.isEditorScene)
                WEditor.Scenario.Editor.ScenarioGenerator.instance.ResetLevel();
            else
                WEditor.Scenario.Playable.ScenarioGenerator.instance.ResetLevel();
        }
    }
}
