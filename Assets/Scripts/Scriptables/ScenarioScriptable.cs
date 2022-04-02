using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.Game.Scriptables
{
    [CreateAssetMenu(fileName = "Scenario", menuName = "ScriptableObjects/Scenario")]
    public class ScenarioScriptable : ScriptableObject
    {
        public List<Sprite> spritesCollection;
    }
}
