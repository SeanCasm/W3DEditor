using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Game.Enemy
{
    public class SpriteLook : MonoBehaviour
    {
        [SerializeField] List<Sprite> walkingSpritesFrontLeft, walkingSpritesFrontRight;
        private Animator animator;
        private Vector3 playerPosition { get => PlayerGlobalReference.instance.position; }
        private Vector3 targetPos;
        private Vector3 targetDir;

        private SpriteRenderer spriteRenderer;

        private float angle;

        void Start()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            animator = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            // Get Target Position and Direction
            targetPos = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
            targetDir = targetPos - transform.position;

            // Get Angle
            angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

            //Flip Sprite if needed
            Vector3 tempScale = Vector3.one;
            if (angle > 0) { tempScale.x *= -1f; }
            spriteRenderer.transform.localScale = tempScale;

        }
        private void LateUpdate()
        {
            float time = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
            int index = GetIndex();

            switch (index)
            {
                case 5:
                    CheckCurrentSprite(time, true);
                    break;
                case 3:
                    CheckCurrentSprite(time, false);
                    break;
            }
        }
        private void CheckCurrentSprite(float animTime, bool frontLeft)
        {
            List<Sprite> currentSpriteList = frontLeft ? walkingSpritesFrontLeft : walkingSpritesFrontRight;
            if (animTime >= 0 && animTime < .25f)
            {
                print(frontLeft);
                spriteRenderer.sprite = currentSpriteList[0];
            }
            else if (animTime >= .25f && animTime < .75f)
            {
                spriteRenderer.sprite = currentSpriteList[1];
            }
            else if (animTime >= .75f && animTime < 1)
            {
                spriteRenderer.sprite = currentSpriteList[2];
            }
            else if (animTime >= 1)
            {
                spriteRenderer.sprite = currentSpriteList[3];
            }
        }
        public int GetIndex()
        {
            //front
            if (angle > -22.5f && angle < 22.6f)
                return 0;
            if (angle >= 22.5f && angle < 67.5f)
                return 7;
            if (angle >= 67.5f && angle < 112.5f)
                return 6;
            if (angle >= 112.5f && angle < 157.5f)
                return 5;


            //back
            if (angle <= -157.5 || angle >= 157.5f)
                return 4;
            if (angle >= -157.4f && angle < -112.5f)
                return 3;
            if (angle >= -112.5f && angle < -67.5f)
                return 2;
            if (angle >= -67.5f && angle <= -22.5f)
                return 1;

            return 0;
        }
    }
}
