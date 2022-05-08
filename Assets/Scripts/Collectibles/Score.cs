using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Game.Collectibles
{
    public class Score : CollectibleBase
    {
        private void OnEnable()
        {
            base.OnPlayerTrigger += PlayerEnter;
        }
        private void OnDisable()
        {
            base.OnPlayerTrigger -= PlayerEnter;
        }
        private void PlayerEnter()
        {
            GameplayEvent.instance.ScoreChanged(amount);
        }
    }
}
