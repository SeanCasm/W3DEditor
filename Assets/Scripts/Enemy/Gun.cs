using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.Game.Enemy
{
    public class Gun : GunBase<int>
    {
        [Range(0, 100)]
        [SerializeField] int accuracy;
        public void Fire()
        {
            int acc = UnityEngine.Random.Range(0, 100);
            if (acc <= accuracy) return;
            PlayerGlobalReference.instance.playerHealth.Take(damage);
        }
    }
}
