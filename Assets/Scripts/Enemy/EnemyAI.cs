using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Player;
namespace WEditor.Game.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Player detection")]
        // [SerializeField] LayerMask playerLayer;
        [SerializeField] float speed;
        [SerializeField] float tileCheckDistance = .64f, distanceToAttack;
        private Animator animator;
        private float currentSpeed;
        private bool isDead, isInvisible;
        private Vector3 directionToPlayer { get => (PlayerGlobalReference.instance.position - transform.position).normalized; }
        private Vector3 playerPosition { get => PlayerGlobalReference.instance.position; }
        private Rigidbody rigid;
        private PathFinding pathfinding;
        private MovementBehaviour eBehaviour = MovementBehaviour.Patrolling;
        public Vector3 spawnPoint { get; set; }
        void Start()
        {
            currentSpeed = speed;
            rigid = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            // transform.position = spawnPoint;
            pathfinding = GetComponent<PathFinding>();
        }

        void Update()
        {
            if (isDead)
            {
                if (!isInvisible)
                {
                    transform.LookAt(playerPosition, Vector3.up);
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                }
            }
            else
            {
                if (eBehaviour != MovementBehaviour.WaitingDoor)
                {
                    PathBehaviour();
                }
            }
        }
        private void PathBehaviour()
        {
            RaycastHit[] raycastHit = Physics.RaycastAll(transform.position, directionToPlayer, tileCheckDistance);
            if (raycastHit[0].collider.tag == "Ground")
            {
                Debug.DrawRay(transform.position, directionToPlayer * tileCheckDistance, Color.red);

                if (raycastHit[0].transform.gameObject.name.Contains("door"))
                {
                    Sensor doorSensor = raycastHit[0].collider.GetComponent<Sensor>();
                    if (doorSensor.doorState != State.Open)
                    {
                        eBehaviour = MovementBehaviour.WaitingDoor;
                    }
                }
                else
                {
                    eBehaviour = MovementBehaviour.Patrolling;
                }
            }
            else
            {
                Debug.DrawRay(transform.position, directionToPlayer * distanceToAttack, Color.black);

                float playerDistance = Vector3.Distance(transform.position, playerPosition);
                if (playerDistance <= distanceToAttack)
                {
                    currentSpeed = 0;
                    rigid.velocity = Vector3.zero;
                    eBehaviour = MovementBehaviour.Attacking;
                    transform.LookAt(playerPosition, Vector3.up);
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                }
                else if (playerDistance <= tileCheckDistance && playerDistance > distanceToAttack)
                {
                    currentSpeed = speed;
                    eBehaviour = MovementBehaviour.FollowPlayer;
                    pathfinding.FindPath(transform.position, playerPosition);
                    MoveBetweenPath();
                }
            }
        }
        void MoveBetweenPath()
        {
            for (int i = 0; i < pathfinding.finalPath.Count; i++)
            {
                Vector3 currentTarget = pathfinding.finalPath[i].gridPosition;
                transform.position = Vector3.MoveTowards(transform.position, currentTarget, currentSpeed * Time.deltaTime);
            }
        }
        public void OnDeath()
        {
            isDead = true;
            eBehaviour = MovementBehaviour.None;
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
                animator.SetBool("isAttacking", eBehaviour == MovementBehaviour.Attacking);
                // animator.SetBool("idle", eBehaviour == EnemyBehaviour.Idle);
                animator.SetBool("isWalking", eBehaviour == MovementBehaviour.FollowPlayer);
                animator.SetBool("isPatrolling", eBehaviour == MovementBehaviour.Patrolling);
            }
        }
        private void OnBecameVisible()
        {
            isInvisible = false;
        }
        private void OnBecameInvisible()
        {
            isInvisible = true;
        }
    }
    public enum MovementBehaviour
    {
        Idle, WaitingDoor, Patrolling, FollowPlayer, Attacking, None
    }
}