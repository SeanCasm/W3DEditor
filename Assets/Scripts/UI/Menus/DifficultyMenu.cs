using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.UI
{
    public class DifficultyMenu : MonoBehaviour
    {
        [SerializeField] Sprite[] tiers;
        public static DifficultyTier difficultyTier = DifficultyTier.Normal;
        public void Button_ChooseDifficulty(int tier)
        {
            switch (tier)
            {
                case 0:
                    difficultyTier = DifficultyTier.Easy;
                    break;
                case 1:
                    difficultyTier = DifficultyTier.Normal;
                    break;
                case 2:
                    difficultyTier = DifficultyTier.Hard;
                    break;
                case 3:
                    difficultyTier = DifficultyTier.VeryHard;
                    break;
            }
        }
    }
    public enum DifficultyTier
    {
        Easy, Normal, Hard, VeryHard
    }
}
