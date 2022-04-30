using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Player;
namespace WEditor.Game.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Player detection")]
        [SerializeField] float speed;
        [SerializeField] float tileCheckDistance = .64f, distanceToAttack;
        private Animator animator;
        private float currentSpeed;
        private bool isDead, isInvisible;
        private Vector3 directionToPlayer { get => (PlayerGlobalReference.instance.position - localCenter).normalized; }
        private Vector3 playerPosition { get => PlayerGlobalReference.instance.position; }
        private Vector3 localCenter { get => spriteRenderer.bounds.center; }
        private Rigidbody rigid;
        private SpriteRenderer spriteRenderer;
        private PathFinding pathfinding;
        private SpriteLook spriteLook;
        int groundLayer = 6;
        int playerLayer = 7;

        private MovementBehaviour eBehaviour = MovementBehaviour.Patrolling;
        void Start()
        {
            currentSpeed = speed;
            spriteLook = GetComponent<SpriteLook>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            rigid = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            pathfinding = GetComponent<PathFinding>();
        }

        void Update()
        {

            if (!isDead)
            {
                CheckBehaviour();
                switch (eBehaviour)
                {
                    case MovementBehaviour.FollowPlayer:
                        currentSpeed = speed;
                        MoveBetweenPath();
                        break;
                    case MovementBehaviour.Attacking:
                        currentSpeed = 0;
                        rigid.velocity = Vector3.zero;
                        // FollowCamera();
                        break;
                }
            }
            else FollowCamera();
        }
        private void CheckBehaviour()
        {
            int layerMaskCombined = (1 << groundLayer) | (1 << playerLayer);

            RaycastHit[] raycastHit = Physics.RaycastAll(localCenter, directionToPlayer,
            Vector3.Distance(localCenter, playerPosition), layerMaskCombined);

            // Debug.DrawRay(localCenter, directionToPlayer * Vector3.Distance(localCenter, playerPosition), Color.cyan);
            switch (raycastHit[0].collider.tag)
            {
                case "Ground":
                    Debug.DrawRay(localCenter, directionToPlayer * tileCheckDistance, Color.red);

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
                    break;
                case "Player":
                    float playerDistance = Vector3.Distance(localCenter, playerPosition);
                    Debug.DrawRay(localCenter, directionToPlayer * playerDistance, Color.green);
                    if (playerDistance <= distanceToAttack)
                    {
                        eBehaviour = MovementBehaviour.Attacking;
                    }
                    else
                    if (playerDistance <= tileCheckDistance && playerDistance > distanceToAttack)
                    {
                        eBehaviour = MovementBehaviour.FollowPlayer;
                    }
                    break;
            }
        }
        private void HandleSpriteLook()
        {
            
        }
        private void FollowCamera()
        {
            if (!isInvisible)
            {
                transform.LookAt(playerPosition, Vector3.up);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
        }
        void MoveBetweenPath()
        {
            pathfinding.FindPath(transform.position, playerPosition);
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