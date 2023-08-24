using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor.Game.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Player detection")]
        [SerializeField] AudioClip alertedClip;
        [SerializeField] float speed;
        [SerializeField] float tileCheckDistance = 1.28f, distanceToAttack;
        private Animator animator;
        private AudioSource audioSource;
        private float currentSpeed;
        private Vector3 PlayerDirection { get => (PlayerGlobalReference.instance.Position - LocalCenter).normalized; }
        private Vector3 PlayerPosition { get => PlayerGlobalReference.instance.Position; }
        private Vector3 LocalCenter { get => spriteRenderer.bounds.center; }
        private Rigidbody rigid;
        private SpriteRenderer spriteRenderer;
        private PathFinding pathfinding;
        private Coroutine moveToPosition;
        private bool IsFollowing => eBehaviour == MovementBehaviour.Follow;
        private bool IsIdle => eBehaviour == MovementBehaviour.Idle;
        private bool IsAttacking => eBehaviour == MovementBehaviour.Attack;
        private int layerMaskCombined = (1 << 6) | (1 << 7);
        private MovementBehaviour eBehaviour = MovementBehaviour.Idle;
        void Start()
        {
            currentSpeed = speed;
            audioSource = GetComponentInChildren<AudioSource>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            rigid = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            pathfinding = GetComponent<PathFinding>();
        }

        void Update()
        {
            if (eBehaviour != MovementBehaviour.Death)
            {
                CheckBehaviour();
                switch (eBehaviour)
                {
                    case MovementBehaviour.Follow:
                        currentSpeed = speed;
                        break;
                    case MovementBehaviour.Attack:
                        currentSpeed = 0;
                        break;
                    case MovementBehaviour.Idle:
                        currentSpeed = 0;
                        break;
                    case MovementBehaviour.Alert:
                        currentSpeed = 0;
                        break;
                }
            }
        }
        void FixedUpdate()
        {
            switch (eBehaviour)
            {
                case MovementBehaviour.Attack:
                    rigid.velocity = Vector3.zero;
                    break;
                case MovementBehaviour.Follow:
                    if (moveToPosition == null)
                        FollowPlayerPath(PlayerPosition);
                    break;
            }
        }
        private void LateUpdate()
        {
            if (eBehaviour != MovementBehaviour.Death)
            {
                animator.SetBool("Idle", eBehaviour == MovementBehaviour.Idle);
                animator.SetBool("Walk", eBehaviour == MovementBehaviour.Follow);
            }
        }
        public RaycastHit[] DrawRaycast()
        {
            return Physics.RaycastAll(LocalCenter, PlayerDirection,
            Vector3.Distance(LocalCenter, PlayerPosition), layerMaskCombined);
        }
        private void CheckBehaviour()
        {

            var raycastHit = DrawRaycast();

            if (raycastHit.Length == 0)
                return;
#if UNITY_EDITOR
            Debug.DrawRay(LocalCenter, PlayerDirection * Vector3.Distance(LocalCenter, PlayerPosition), Color.cyan);
#endif
            switch (raycastHit[0].collider.tag)
            {
                case "Ground":
#if UNITY_EDITOR
                    Debug.DrawRay(LocalCenter, PlayerDirection * tileCheckDistance, Color.red);
#endif

                    if (raycastHit[0].transform.gameObject.name.Contains("Door"))
                    {
                        Sensor doorSensor = raycastHit[0].collider.GetComponent<Sensor>();
                        if (doorSensor.doorState != State.Open)
                        {
                            eBehaviour = MovementBehaviour.Idle;
                        }
                    }
                    if (IsAttacking)
                        eBehaviour = MovementBehaviour.Follow;
                    break;
                case "Player":
                    float playerDistance = Vector3.Distance(LocalCenter, PlayerPosition);
#if UNITY_EDITOR
                    Debug.DrawRay(LocalCenter, PlayerDirection * playerDistance, Color.green);
#endif
                    if (playerDistance <= distanceToAttack && (IsFollowing || IsIdle))
                    {
                        if (!audioSource.isPlaying)
                            audioSource.PlayOneShot(alertedClip);
                        animator.SetTrigger("Attack");
                        eBehaviour = MovementBehaviour.Alert;
                    }
                    else
                    if (playerDistance <= tileCheckDistance && playerDistance > distanceToAttack && (IsIdle || IsAttacking))
                    {
                        eBehaviour = MovementBehaviour.Follow;
                    }
                    break;
            }
        }
        void FollowPlayerPath(Vector3 target)
        {
            moveToPosition = StartCoroutine(MoveToPosition(target));
        }
        public void HurtBehaviour()
        {
            if (IsIdle)
                eBehaviour = MovementBehaviour.Follow;
        }
        IEnumerator MoveToPosition(Vector3 target)
        {
            var finalPath = pathfinding.FindPath(transform.position, target);
            bool xAprox;
            bool yAprox;
            int index = 0;
            do
            {
                target = finalPath[index].gridPositionCenter;
                Vector3 newPos = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.deltaTime);
                rigid.MovePosition(newPos);
                yield return new WaitForFixedUpdate();
                xAprox = Mathf.Approximately(target.x, transform.position.x);
                yAprox = Mathf.Approximately(target.z, transform.position.z);
                if (xAprox && yAprox)
                    index++;
            } while (index < finalPath.Count);
            moveToPosition = null;
        }
        public void StartAttack()
        {
            eBehaviour = MovementBehaviour.Attack;
        }
        public void OnDeath()
        {
            eBehaviour = MovementBehaviour.Death;
            animator.SetBool("Idle", false);
            animator.SetTrigger("death");
            rigid.velocity = Vector3.zero;
            StopAllCoroutines();
            Destroy(rigid);
        }

    }
    public enum MovementBehaviour
    {
        Idle, Follow, Attack, Alert, Death
    }
}