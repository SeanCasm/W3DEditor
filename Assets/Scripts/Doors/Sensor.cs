using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Game.Doors
{
    public class Sensor : MonoBehaviour
    {
        [SerializeField] float slideTime;
        [SerializeField] float timeBeforeClose;
        // [SerializeField] LayerMask playerLayer, enemyLayer;
        private bool isOpen = false;
        private bool playerAround = false;
        private bool enemyAround = false;
        private bool isOpening = false, isClosing;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerAround = true;
                if (!isOpening && !isClosing)
                {
                    StartCoroutine(nameof(Open));
                }
            }
            else if (other.CompareTag("Enemy"))
            {
                enemyAround = true;
                if (!isOpening && !isClosing)
                {
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
            isOpening = true;
            float time = 0;
            float direction = 1;
            float speed = direction / slideTime;
            while (time <= slideTime)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
                time += Time.deltaTime;
                yield return null;
            }
            isOpen = true;

            yield return new WaitForSeconds(timeBeforeClose);
            yield return new WaitWhile(() => playerAround || enemyAround);
            isOpening = false;
            StartCoroutine(nameof(Close));
        }
        IEnumerator Close()
        {
            isClosing = true;
            float time = 0;
            float direction = -1;
            float speed = direction / slideTime;
            while (time <= slideTime)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
                time += Time.deltaTime;
                yield return null;
            }
            isClosing = isOpen = false;
        }
    }
}