using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;
using WEditor.Utils;

namespace WEditor.Game.Scriptables
{
    [CreateAssetMenu(fileName = "Scenario", menuName = "ScriptableObjects/Scenario")]
    public class ScenarioScriptable : ScriptableObject
    {
        [SerializeField] List<Sprite> spritesCollection;
        [SerializeField] List<Texture2D> textures;
        public List<Sprite> SpritesCollection { get => spritesCollection; }
        public Sprite GetSprite(string spriteName)
        {
            return spritesCollection.Find(x => x.name == spriteName);
        }
        public Texture2D GetTexture(string textureName)
        {
            Texture2D find = textures.Find(x => x.name == textureName);
            return find;
        }
    }
}
