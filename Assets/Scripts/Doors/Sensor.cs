using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Events;
using WEditor.Game.Scriptables;

namespace WEditor.Game
{
    public class Sensor : MonoBehaviour, IInteractable
    {
        [SerializeField] float slideTime;
        [SerializeField] AudioClip openClip, closeClip;
        public KeyDoorScriptable keyDoorScriptable { get; set; }
        private bool playerAround = false;
        private bool enemyAround = false;
        private AudioSource audioSource;
        public State doorState { get; private set; } = State.Close;
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            GetComponent<SpriteRenderer>().sprite = keyDoorScriptable.doorSprite;
        }
        private void OnEnable()
        {
            GameplayEvent.instance.onInteracted += OnInteracted;
        }
        private void OnDisable()
        {
            GameplayEvent.instance.onInteracted -= OnInteracted;
        }
        public void OnInteracted(List<KeyType> keyToOpen)
        {
            if (doorState == State.Close && playerAround
                && keyToOpen.Exists(x => x == keyDoorScriptable.keyType))
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

            yield return new WaitForSeconds(keyDoorScriptable.timeToClose);
            yield return new WaitWhile(() => playerAround || enemyAround);

            if (keyDoorScriptable.timeToClose > 0)
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
public interface IInteractable
{
    public void OnInteracted(List<KeyType> keyToOpen);
    public KeyDoorScriptable keyDoorScriptable { get; set; }

}