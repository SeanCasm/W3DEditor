using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
namespace WEditor.Game.Player
{
    /// <summary>
    /// Handle the player spawn at dead
    /// </summary>
    public class StatusBehaviour : MonoBehaviour
    {
        [SerializeField] Health playerHealth;
        private void OnEnable()
        {
            GameplayEvent.instance.onReset += Respawn;
        }
        private void OnDisable()
        {
            GameplayEvent.instance.onReset -= Respawn;
        }
        private void Respawn()
        {
            Vector3Int pos = DataHandler.spawnPosition;
            PlayerGlobalReference.instance.Position = new Vector3(pos.x, pos.y, pos.z);
            playerHealth.Add(100);
            if (SceneHandler.instance.isEditorScene)
                WEditor.Scenario.Editor.ScenarioGenerator.instance.ClearLevel();
            else
                WEditor.Scenario.Playable.ScenarioGenerator.instance.ClearLevel();
        }
    }
}
