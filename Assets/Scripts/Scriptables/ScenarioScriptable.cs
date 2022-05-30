using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Utils;

namespace WEditor.Game.Scriptables
{
    [CreateAssetMenu(fileName = "Scenario", menuName = "ScriptableObjects/Scenario")]
    public class ScenarioScriptable : ScriptableObject
    {
        [SerializeField] List<Sprite> spritesCollection;
        public List<Sprite> SpritesCollection { get => spritesCollection; }
        public Sprite GetSprite(string spriteName)
        {
            return spritesCollection.Find(x => x.name == spriteName);
        }
    }
}
