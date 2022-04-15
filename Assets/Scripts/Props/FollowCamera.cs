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
            transform.LookAt(PlayerGlobalReference.instance.position,Vector3.up);
            transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
        }
    }
}
