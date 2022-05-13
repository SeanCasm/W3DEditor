using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Game.Enemy
{
    public class SpriteLook : MonoBehaviour
    {
        [SerializeField] List<Sprite> walkingSpritesFront, walkingSpritesBack;
        [SerializeField] List<Sprite> walkingSpritesFrontLeft, walkingSpritesFrontRight;
        [SerializeField] List<Sprite> walkingSpritesLeft, walkingSpritesRight;
        [SerializeField] List<Sprite> walkingSpritesFrontAngleLeft, walkingSpritesFrontAngleRight;
        [SerializeField] List<Sprite> walkingSpritesBackAngleLeft, walkingSpritesBackAngleRight;
        [SerializeField] SpriteRenderer spriteRenderer;
        public void SwapSpriteWhenFollowingPlayer(float angle, Animator animator, bool isVisible)
        {
            if (!isVisible) return;
            float time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
            SetDirection(time, angle);
            FollowCamera();
        }
        public void FollowCamera()
        {
            spriteRenderer.transform.LookAt(PlayerGlobalReference.instance.position, Vector3.up);
            spriteRenderer.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        private void SetDirection(float animTime, float angle)
        {
            List<Sprite> currentSpriteList = new List<Sprite>();
            // print(angle);
            //front
            if (angle > 90 && angle < 135)
                currentSpriteList = walkingSpritesFrontAngleLeft;
            if (angle > 135 && angle < 180)
                currentSpriteList = walkingSpritesFront;
            if (angle > -180 && angle < -135)
                currentSpriteList = walkingSpritesFront;
            if (angle > -135 && angle < -90)
                currentSpriteList = walkingSpritesFrontAngleRight;

            //back

            // if (angle < 90 && angle > 45)
            //     currentSpriteList = walkingSpritesBackAngleLeft;
            // if (angle < 45 && angle > 0)
            //     currentSpriteList = walkingSpritesBack;
            // if (angle > -180 && angle < -135)
            //     currentSpriteList = walkingSpritesBack;
            // if (angle > -135 && angle < -90)
            //     currentSpriteList = walkingSpritesBackAngleRight;



            if (animTime >= 0 && animTime < .25f)
                spriteRenderer.sprite = currentSpriteList[0];
            else if (animTime >= .25f && animTime < .75f)
                spriteRenderer.sprite = currentSpriteList[1];
            else if (animTime >= .75f && animTime < 1)
                spriteRenderer.sprite = currentSpriteList[2];
            else if (animTime >= 1)
                spriteRenderer.sprite = currentSpriteList[3];

        }
    }
}
