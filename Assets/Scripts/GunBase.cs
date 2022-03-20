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
        protected Transform shootPoint;
        protected void Start()
        {
            shootPoint = transform.GetChild(0);
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
