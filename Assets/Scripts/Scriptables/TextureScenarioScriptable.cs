using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace WEditor.Game.Scriptables
{
    [CreateAssetMenu(fileName = "TextureScenario", menuName = "ScriptableObjects/Texture")]
    public class TextureScenarioScriptable : ScriptableObject
    {
        [SerializeField] List<Texture2D> allTextures;

        public Texture2D GetTexture(string textureName)
        {
            string[] nameSplitted = textureName.Split('_');
            int index = int.Parse(nameSplitted.Last());
            return allTextures[index];
        }
    }
}
