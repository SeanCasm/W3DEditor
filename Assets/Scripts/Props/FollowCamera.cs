using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WEditor.Game.Player;
namespace WEditor.Game
{

    public class FollowCamera : MonoBehaviour
    {
        void Update()
        {
            transform.LookAt(PlayerGlobalReference.instance.position);
        }
    }
}
