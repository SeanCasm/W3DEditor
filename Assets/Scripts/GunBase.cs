using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Enemy;

namespace WEditor.Game
{
    public class GunBase<T> : MonoBehaviour
    {
        [SerializeField] protected float checkDistance;
        [SerializeField] protected LayerMask hitLayer;
        [SerializeField] protected T damage;
        [SerializeField] AudioClip shootClip;
        protected Transform shootPoint;
        protected AudioSource audioSource;
        protected void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = shootClip;
        }
    }
}
