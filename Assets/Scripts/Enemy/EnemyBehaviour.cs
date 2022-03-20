using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Player;
namespace WEditor.Game.Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        [Header("Player detection")]
        [SerializeField] float checkDistance;
        private Animator animator;
        private Rigidbody rigid;
        private bool isAttacking, isAlerted;
        private bool playerDetected;
        private float distanceToPlayer { get => Vector3.Distance(transform.position, PlayerGlobalReference.instance.playerPosition); }
        void Start()
        {
            rigid = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            Debug.DrawRay(transform.position, PlayerGlobalReference.instance.playerPosition, Color.red);
            if (distanceToPlayer <= checkDistance && !playerDetected)
            {
                isAttacking = true;
                isAlerted = true;
            }
        }
        private void LateUpdate()
        {
            animator.SetBool("isAttacking", isAttacking);
        }
    }
}
