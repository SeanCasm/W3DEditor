using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Collectibles;
namespace WEditor.Game
{
    public class DifficultyHandler : MonoBehaviour
    {
        public static DifficultyHandler instance;
        private void Start()
        {
            instance = this;
        }
        public void HandleDifficulty(DifficultyTier difficultTier)
        {
            switch (difficultTier)
            {
                case DifficultyTier.Easy:
                    Enemy.Health.healthMultiplier = 1;
                    Ammo.ammoDevalue = 0;
                    break;
                case DifficultyTier.Medium:
                    Enemy.Health.healthMultiplier = 1.2f;
                    Ammo.ammoDevalue = 10;
                    break;
                case DifficultyTier.Hard:
                    Enemy.Health.healthMultiplier = 1.5f;
                    Ammo.ammoDevalue = 25;
                    break;
            }
        }
    }
}
