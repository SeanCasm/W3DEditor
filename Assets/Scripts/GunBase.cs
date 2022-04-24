using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.Game.Guns
{
    public class GunBase : MonoBehaviour
    {
        [SerializeField] protected float checkDistance;
        [SerializeField] protected LayerMask hitLayer;
        [SerializeField] protected float damage;
        [SerializeField] AudioClip shootClip;
        protected Transform shootPoint;
        private AudioSource audioSource;
        protected void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = shootClip;
            shootPoint = transform.GetChild(0);
        }
        public void PlayShootClip()
        {
            audioSource.Play();
        }
        public Tuple<bool, RaycastHit> ShootRay()
        {
            RaycastHit raycastHit;
            Ray ray = new Ray(shootPoint.position, transform.root.forward);

            bool hit = Physics.Raycast(ray, out raycastHit, checkDistance, hitLayer);
            Debug.DrawLine(shootPoint.position, transform.root.forward * checkDistance, Color.green, 1000);
            return Tuple.Create(hit, raycastHit);
        }
    }
}
