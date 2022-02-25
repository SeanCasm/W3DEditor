using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.Scenario
{
    public class HorizotalDropdown : EditorDropdown
    {
        [SerializeField] HorizontalSide dropdownSide;
        private new void Start()
        {
            base.Start();
            switch (dropdownSide)
            {
                case HorizontalSide.Left:
                    dropdownDirection = new Vector2(1, 0);
                    break;
                case HorizontalSide.Right:
                    dropdownDirection = new Vector2(-1, 0);
                    break;
            }
            currentDir = dropdownDirection.x;
        }
        IEnumerator StartDropdown()
        {
            float time = 0;
            float posX = dropdownRect.anchoredPosition.x;
            while (time < dropdownTime)
            {
                dropdownRect.anchoredPosition = new Vector2(posX += speed * Time.deltaTime * currentDir, 0);
                time += Time.deltaTime;
                yield return null;
            }
            currentDir *= -1;
            dropdownButton.interactable = true;
        }

    }
    public enum HorizontalSide
    {
        Left, Right
    }
}
