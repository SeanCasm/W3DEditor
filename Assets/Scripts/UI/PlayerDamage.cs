using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.UI
{
    public class PlayerDamage : MonoBehaviour
    {
        public static PlayerDamage instance;
        [SerializeField] Animator animator;

        private void Start()
        {
            instance = this;
        }
        
        public void StartAnimation()
        {
            animator.SetTrigger("start");
        }
    }
}
