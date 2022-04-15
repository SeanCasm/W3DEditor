using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Game.Scriptables
{
    [CreateAssetMenu(fileName = "Collectible", menuName = "ScriptableObjects/Enemy")]
    public class EnemyScriptable : ScriptableObject
    {
        public Sprite enemySprite;
        public string enemyName;
    }
}
