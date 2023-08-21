using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Game.Collectibles
{
    public class Score : CollectibleBase
    {

        protected override bool OnPlayerEnter()
        {
            GameplayEvent.instance.ScoreChanged(amount);
            GameplayEvent.instance.TeasuresChanged();
            return true;
        }
    }
}
