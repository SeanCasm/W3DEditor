using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;

namespace WEditor.Game
{
    public class Sensor : MonoBehaviour
    {
        [SerializeField] float slideTime;
        [SerializeField] float timeBeforeClose;
        [SerializeField] AudioClip openClip, closeClip;
        private bool playerAround = false;
        private bool enemyAround = false;
        private AudioSource audioSource;
        public float timeToClose { get => timeBeforeClose; set => timeBeforeClose = value; }
        public State doorState { get; private set; } = State.Close;
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
        private void OnEnable()
        {
            GameplayEvent.instance.onInteracted += OnInteracted;
        }
        private void OnDisable()
        {
            GameplayEvent.instance.onInteracted -= OnInteracted;
        }
        private void OnInteracted()
        {
            if (doorState == State.Close && playerAround)
            {
                doorState = State.Opening;
                audioSource.clip = openClip;
                audioSource.Play();
                StartCoroutine(nameof(Open));
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerAround = true;
            }
            else if (other.CompareTag("Enemy"))
            {
                enemyAround = true;
                if (doorState == State.Close)
                {
                    audioSource.Play();
                    StartCoroutine(nameof(Open));
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerAround = false;
            }
            else if (other.CompareTag("Enemy"))
            {
                enemyAround = false;
            }
        }

        IEnumerator Open()
        {
            float time = 0;
            float direction = 1;
            float speed = direction / slideTime;
            while (time <= slideTime)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
                time += Time.deltaTime;
                yield return null;
            }
            doorState = State.Open;

            yield return new WaitForSeconds(timeBeforeClose);
            yield return new WaitWhile(() => playerAround || enemyAround);

            if (timeBeforeClose > 0)
                StartCoroutine(nameof(Close));
        }
        IEnumerator Close()
        {
            doorState = State.Closing;
            float time = 0;
            float direction = -1;
            float speed = direction / slideTime;
            while (time <= slideTime)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
                time += Time.deltaTime;
                yield return null;
            }
            audioSource.clip = closeClip;
            audioSource.Play();
            doorState = State.Close;
        }
    }
    public enum State
    {
        Close, Closing, Open, Opening
    }
}