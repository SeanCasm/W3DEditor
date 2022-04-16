using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.Game.Scriptables
{
    [CreateAssetMenu(fileName = "Scenario", menuName = "ScriptableObjects/Scenario")]
    public class ScenarioScriptable : ScriptableObject
    {
        [SerializeField] List<Sprite> spritesCollection;
        public Sprite GetSprite(string spriteName)
        {
            int index = int.Parse(spriteName.Split('_')[1]);
            return spritesCollection[index];
        }
    }
}
