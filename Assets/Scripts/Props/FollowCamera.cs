using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Player;
namespace WEditor.Game
{

    public class FollowCamera : MonoBehaviour
    {
        bool isVisible;
        void Update()
        {
            if (isVisible)
            {
                transform.LookAt(PlayerGlobalReference.instance.Position, Vector3.up);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
        }
        private void OnBecameInvisible()
        {
            isVisible = false;
        }
        private void OnBecameVisible()
        {
            isVisible = true;
        }
    }
}
