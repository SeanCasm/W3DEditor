using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Utils;
using System.Linq;
namespace WEditor.Game.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Player detection")]
        [SerializeField] AudioClip alertedClip;
        [SerializeField] float speed;
        [SerializeField] float tileCheckDistance = .64f, distanceToAttack;
        private Animator animator;
        private AudioSource audioSource;
        private float currentSpeed;
        private bool isVisible;
        private Vector3 playerDirection { get => (PlayerGlobalReference.instance.position - localCenter).normalized; }
        private Vector3 playerPosition { get => PlayerGlobalReference.instance.position; }
        private Vector3 localCenter { get => spriteRenderer.bounds.center; }
        private Rigidbody rigid;
        private SpriteRenderer spriteRenderer;
        private PathFinding pathfinding;
        int groundLayer = 6, playerLayer = 7;
        private float angle;
        private List<Vector3Int> movements = new List<Vector3Int>();

        private MovementBehaviour eBehaviour = MovementBehaviour.Patrol;
        void Start()
        {
            currentSpeed = speed;
            audioSource = GetComponentInChildren<AudioSource>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            rigid = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            pathfinding = GetComponent<PathFinding>();
            // GenerateMovementPattern();
        }

        void Update()
        {
            if (eBehaviour != MovementBehaviour.Death)
            {
                angle = Vector3.SignedAngle(playerDirection, transform.forward, Vector3.up);
                CheckBehaviour();
                switch (eBehaviour)
                {
                    case MovementBehaviour.Follow:
                        // StopCoroutine(nameof(PatrollingMovement));
                        currentSpeed = speed;
                        spriteRenderer.transform.eulerAngles = Vector3.zero;
                        FollowCamera(playerPosition);
                        MoveBetweenPath(playerPosition);
                        break;
                    case MovementBehaviour.Attack:
                        // StopCoroutine(nameof(PatrollingMovement));
                        currentSpeed = 0;
                        rigid.velocity = Vector3.zero;
                        FollowCamera(playerPosition);
                        break;
                    // case MovementBehaviour.Patrol:
                    //     currentSpeed = speed;
                    //     FollowCamera(playerPosition);
                    //     MoveBetweenPath(playerPosition);
                    //     // if (!isPatrolling) StartCoroutine(nameof(PatrollingMovement));
                    //     break;
                    case MovementBehaviour.Idle:
                        currentSpeed = 0;
                        break;
                }
            }
            else FollowCamera(playerPosition);
        }
        public void FollowCamera(Vector3 playerPosition)
        {
            if (isVisible)
            {
                spriteRenderer.transform.LookAt(playerPosition, Vector3.up);
                spriteRenderer.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
        }
        private void GenerateMovementPattern()
        {
            Vector3Int transformInt = Vector3Int.FloorToInt(transform.position);
            for (int i = 0; i < 5; i++)
            {
                Vector3Int move = new Vector3Int(
                    Random.Range(transformInt.x - 2, transformInt.x + 3),
                    Random.Range(transformInt.z - 2, transformInt.z + 3),
                    0
                );
                movements.Add(move);
            }
        }
        private void CheckBehaviour()
        {
            int layerMaskCombined = (1 << groundLayer) | (1 << playerLayer);

            RaycastHit[] raycastHit = Physics.RaycastAll(localCenter, playerDirection,
            Vector3.Distance(localCenter, playerPosition), layerMaskCombined);

            if (raycastHit.Length == 0)
                return;

            Debug.DrawRay(localCenter, playerDirection * Vector3.Distance(localCenter, playerPosition), Color.cyan);
            switch (raycastHit[0].collider.tag)
            {
                case "Ground":
                    Debug.DrawRay(localCenter, playerDirection * tileCheckDistance, Color.red);

                    if (raycastHit[0].transform.gameObject.name.Contains("Door"))
                    {
                        Sensor doorSensor = raycastHit[0].collider.GetComponent<Sensor>();
                        if (doorSensor.doorState != State.Open)
                        {
                            eBehaviour = MovementBehaviour.Idle;
                        }
                    }
                    else
                    {
                        eBehaviour = MovementBehaviour.Patrol;
                    }
                    break;
                case "Player":
                    if (eBehaviour != MovementBehaviour.Alert)
                    {
                        audioSource.clip = alertedClip;
                        audioSource.Play();
                        eBehaviour = MovementBehaviour.Alert;
                    }
                    float playerDistance = Vector3.Distance(localCenter, playerPosition);
                    Debug.DrawRay(localCenter, playerDirection * playerDistance, Color.green);
                    if (playerDistance <= distanceToAttack)
                    {
                        eBehaviour = MovementBehaviour.Attack;
                    }
                    else
                    if (playerDistance <= tileCheckDistance && playerDistance > distanceToAttack)
                    {
                        eBehaviour = MovementBehaviour.Follow;
                    }
                    break;
            }
        }
        // IEnumerator PatrollingMovement()
        // {
        //     int j = 0;
        //     while (eBehaviour == MovementBehaviour.Patrolling)
        //     {
        //         var target = movements[j];
        //         pathfinding.FindPath(transform.position, new Vector3(target.x, target.y, 0));
        //         for (int i = 0; i < pathfinding.finalPath.Count; i++)
        //         {
        //             Vector3Int currentTarget = Vector3Int.FloorToInt(pathfinding.finalPath[i].gridPosition);
        //             Vector3Int positionGrid = Vector3Int.FloorToInt(transform.position).SwapZToY().ZEqualZero();
        //             while (Vector3Int.FloorToInt(currentTarget) != positionGrid)
        //             {
        //                 transform.position = Vector3.MoveTowards(
        //                     transform.position,
        //                     new Vector3(currentTarget.x, transform.position.y, currentTarget.y),
        //                     currentSpeed * Time.deltaTime
        //                 );
        //                 yield return null;
        //             }
        //         }
        //         j++;
        //         if (j >= 5) j = 0;
        //     }
        // }
        void MoveBetweenPath(Vector3 target)
        {
            pathfinding.FindPath(transform.position, target);
            for (int i = 0; i < pathfinding.finalPath.Count; i++)
            {
                Vector3 currentTarget = pathfinding.finalPath[i].gridPositionCenter;
                transform.position = Vector3.MoveTowards(transform.position, currentTarget, currentSpeed * Time.deltaTime);
            }
        }
        public void OnDeath()
        {
            eBehaviour = MovementBehaviour.Death;
            animator.SetBool("Attack", false);
            animator.SetBool("Idle", false);
            animator.SetBool("Patrol", false);
            animator.SetBool("Alert", false);
            animator.SetTrigger("death");
            rigid.velocity = Vector3.zero;
            StopAllCoroutines();
            Destroy(rigid);
        }
        private void LateUpdate()
        {
            if (eBehaviour != MovementBehaviour.Death)
            {
                // if (eBehaviour == MovementBehaviour.Attacking || eBehaviour == MovementBehaviour.FollowingPlayer)
                //     spriteLook.SwapSpriteWhenFollowingPlayer(angle, animator, isVisible);

                animator.SetBool("Attack", eBehaviour == MovementBehaviour.Attack);
                animator.SetBool("Idle", eBehaviour == MovementBehaviour.Idle);
                animator.SetBool("Patrol", eBehaviour == MovementBehaviour.Patrol || eBehaviour == MovementBehaviour.Follow);
                animator.SetBool("Alert", eBehaviour == MovementBehaviour.Alert);
            }
        }
        private void OnBecameVisible()
        {
            isVisible = true;
        }
        private void OnBecameInvisible()
        {
            isVisible = false;
        }
    }
    public enum MovementBehaviour
    {
        Idle, Patrol, Follow, Attack, Alert, Death
    }
}