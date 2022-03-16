using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace WEditor.Game.Collectibles
{
    public abstract class CollectibleBase : MonoBehaviour
    {
        [SerializeField] protected int amount;
        [SerializeField] AudioClip sound;
        [SerializeField]
        protected abstract void AddTo();
        protected abstract void OnTriggerEnter(Collider other);
    }
    public enum CollectibleType
    {
        ammo, health, points
    }
}