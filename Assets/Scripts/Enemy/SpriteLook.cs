using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Game.Enemy
{
    public class SpriteLook : MonoBehaviour
    {
        [SerializeField] List<Sprite> walkingSpritesFrontLeft, walkingSpritesFrontRight;
        [SerializeField] List<Sprite> walkingSpritesBackLeft, walkingSpritesBackRight, walkingSpritesBack;

        public void SwapSpriteWhenFollowingPlayer(float angle, Animator animator, SpriteRenderer spriteRenderer)
        {
            float time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
            int index = GetIndex(angle);

            switch (index)
            {
                case 0:
                    CheckCurrentSprite(time, true, spriteRenderer);
                    break;
                case 3:
                    CheckCurrentSprite(time, false, spriteRenderer);
                    break;

            }
        }
        private void CheckCurrentSprite(float animTime, bool frontLeft, SpriteRenderer spriteRenderer)
        {
            List<Sprite> currentSpriteList = frontLeft ? walkingSpritesFrontLeft : walkingSpritesFrontRight;
            if (animTime >= 0 && animTime < .25f)
                spriteRenderer.sprite = currentSpriteList[0];
            else if (animTime >= .25f && animTime < .75f)
                spriteRenderer.sprite = currentSpriteList[1];
            else if (animTime >= .75f && animTime < 1)
                spriteRenderer.sprite = currentSpriteList[2];
            else if (animTime >= 1)
                spriteRenderer.sprite = currentSpriteList[3];
        }
        private int GetIndex(float angle)
        {
            //front
            if (angle > 90 && angle < 135)
                return 0;
            if (angle > 135 && angle < 180)
                return 1;
            if (angle > -180 && angle < -135)
                return 2;
            if (angle > -135 && angle < -90)
                return 3;

            return 0;

            //back


        }
    }
}
