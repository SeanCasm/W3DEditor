using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Guns;

namespace WEditor.Game.Enemy.Guns
{
    public class Gun : GunBase<int>
    {
        [Range(0, 100)]
        [SerializeField] int failChance;
        public void Fire()
        {
            int finalChance = UnityEngine.Random.Range(0, 100);
            if (finalChance <= failChance) return;
            PlayerGlobalReference.instance.playerHealth.Take(damage);
        }
    }
}
