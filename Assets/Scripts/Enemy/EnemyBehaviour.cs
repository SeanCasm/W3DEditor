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
        [SerializeField] LayerMask playerLayer;
        private Animator animator;
        private Rigidbody rigid;
        private bool isAttacking;
        private bool playerDetected;
        private float distanceToPlayer { get => Vector3.Distance(transform.position, PlayerGlobalReference.instance.position); }
        private Vector3 directionToPlayer { get => (transform.position - PlayerGlobalReference.instance.position).normalized; }
        private Vector3 position { get => transform.position; set => transform.position = value; }
        private TilemapPathFinding pathfinding;
        public Vector3 spawnPoint { get; set; }
        void Start()
        {
            rigid = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            position = spawnPoint;
            pathfinding = GetComponent<TilemapPathFinding>();
        }

        void Update()
        {
            Debug.DrawRay(position, PlayerGlobalReference.instance.position, Color.red);
            RaycastHit[] raycastHit = Physics.RaycastAll(position, directionToPlayer, checkDistance, playerLayer);
            foreach (var hit in raycastHit)
            {
                if (CompareTag(hit, "Ground"))
                {
                    break;
                }
                else
                if (CompareTag(hit, "Player") && distanceToPlayer <= checkDistance && !playerDetected)
                {
                    playerDetected = true;
                    pathfinding.Init(position);
                }
            }
        }
        private void LateUpdate()
        {
            animator.SetBool("isAttacking", isAttacking);
        }
        private bool CompareTag(RaycastHit hit, string tagToCompare)
        {
            return hit.collider.tag == tagToCompare;
        }
    }
}
