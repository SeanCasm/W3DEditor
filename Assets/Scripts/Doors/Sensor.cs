using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Game.Doors
{

    public class Sensor : MonoBehaviour
    {
        [SerializeField] float slideTime;
        // [SerializeField] LayerMask playerLayer, enemyLayer;
        private bool isOpen = false;
        void Start()
        {
        }
        private void OnTriggerEnter(Collider other)
        {
            if ((other.CompareTag("Player") || other.CompareTag("Enemy")) && !isOpen)
            {
                print(other);
                StartCoroutine(nameof(Slide));
            }
        }
        IEnumerator Slide(bool open)
        {
            float time = 0;
            float side = open ? 1 : -1;
            while (time <= slideTime)
            {
                transform.Translate(Vector3.right * side * Time.deltaTime, Space.World);
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
