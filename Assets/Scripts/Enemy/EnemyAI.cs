using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Player;
namespace WEditor.Game.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Player detection")]
        [SerializeField] LayerMask playerLayer;
        [SerializeField] float tileCheckDistance = .64f, distanceToAttack;
        [SerializeField] float speed;
        private Animator animator;
        private float distanceToPlayer { get => Vector3.Distance(transform.position, PlayerGlobalReference.instance.position); }
        private float currentSpeed;
        private bool isDead;
        private Vector3 directionToPlayer { get => (PlayerGlobalReference.instance.position - transform.position).normalized; }
        private Vector3 playerPosition { get => PlayerGlobalReference.instance.position; }
        private Rigidbody rigid;
        private PathFinding pathfinding;
        private EnemyBehaviour eBehaviour = EnemyBehaviour.Patrolling;
        public Vector3 spawnPoint { get; set; }
        void Start()
        {
            currentSpeed = speed;
            rigid = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            // position = spawnPoint;
            pathfinding = GetComponent<PathFinding>();

        }

        void Update()
        {
            if (isDead)
            {
                transform.LookAt(playerPosition, Vector3.up);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
            else
            {
                PathBehaviour();
                if (eBehaviour == EnemyBehaviour.FollowPlayer)
                {
                    pathfinding.FindPath(transform.position, playerPosition);
                    MoveBetweenPath();
                }
            }
        }
        private void PathBehaviour()
        {
            RaycastHit[] raycastHit = Physics.RaycastAll(transform.position, directionToPlayer, tileCheckDistance, playerLayer);
            Debug.DrawRay(transform.position, directionToPlayer * tileCheckDistance, Color.red);
            Debug.DrawRay(transform.position, directionToPlayer * distanceToAttack, Color.black);

            foreach (var hit in raycastHit)
            {
                switch (hit.collider.tag)
                {
                    case "Ground":
                        break;
                    case "Player":
                        Vector3 playerPosition = hit.collider.transform.position;
                        float playerDistance = Vector3.Distance(transform.position, playerPosition);

                        if (playerDistance <= distanceToAttack)
                        {
                            currentSpeed = 0;
                            rigid.velocity = Vector3.zero;
                            eBehaviour = EnemyBehaviour.Attacking;
                            transform.LookAt(playerPosition, Vector3.up);
                            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                        }
                        else if (playerDistance <= tileCheckDistance && playerDistance > distanceToAttack)
                        {
                            currentSpeed = speed;
                            eBehaviour = EnemyBehaviour.FollowPlayer;
                        }
                        break;
                }
            }
        }
        void MoveBetweenPath()
        {
            for (int i = 0; i < pathfinding.finalPath.Count - 1; i++)
            {
                Vector3 currentTarget = pathfinding.finalPath[i].gridPosition;
                transform.position = Vector3.MoveTowards(transform.position, currentTarget, currentSpeed * Time.deltaTime);
            }
        }
        public void OnDeath()
        {
            isDead = true;
            eBehaviour = EnemyBehaviour.None;
            animator.SetBool("isAttacking", false);
            animator.SetBool("idle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isPatrolling", false);
            animator.SetTrigger("death");
            Destroy(rigid);
        }
        private void LateUpdate()
        {
            if (!isDead)
            {
                animator.SetBool("isAttacking", eBehaviour == EnemyBehaviour.Attacking);
                // animator.SetBool("idle", eBehaviour == EnemyBehaviour.Idle);
                animator.SetBool("isWalking", eBehaviour == EnemyBehaviour.FollowPlayer);
                animator.SetBool("isPatrolling", eBehaviour == EnemyBehaviour.Patrolling);
            }
        }
        private bool CompareTag(RaycastHit hit, string tagToCompare)
        {
            return hit.collider.tag == tagToCompare;
        }
    }
    public enum EnemyBehaviour
    {
        Idle, Patrolling, FollowPlayer, Attacking, None
    }
}