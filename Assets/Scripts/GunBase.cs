using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.Game.Guns
{
    public class GunBase : MonoBehaviour
    {
        [SerializeField] protected Transform shootPoint;
        [SerializeField] protected float bulletDistance;
        [SerializeField] protected LayerMask hitLayer;
        [SerializeField] protected float damage;
        public Tuple<bool, RaycastHit> ShootRay()
        {
            RaycastHit raycastHit;
            Ray ray = new Ray(shootPoint.position, transform.forward);

            bool hit = Physics.Raycast(ray, out raycastHit, bulletDistance, hitLayer);

            return Tuple.Create(hit, raycastHit);
        }
    }
}
