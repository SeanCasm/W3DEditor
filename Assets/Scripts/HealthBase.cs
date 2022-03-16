using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Game
{
    public abstract class HealthBase<T> : MonoBehaviour
    {
        [SerializeField] protected T maxHealth;
        protected T currentHealth;
        protected bool isDead;
        public abstract void Take(T amount);
        public abstract void OnDeath();
    }
}