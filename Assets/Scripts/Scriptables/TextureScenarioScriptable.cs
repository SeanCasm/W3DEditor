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
            int index = int.Parse(textureName.Split('_')[1]);
            return allTextures[index];
        }
    }
}
