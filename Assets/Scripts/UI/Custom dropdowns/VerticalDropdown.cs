using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace WEditor.Scenario
{
    public class VerticalDropdown : EditorDropdown
    {
        [SerializeField] VerticalSide dropdownSide;
        private new void Start()
        {
            base.Start();
            switch (dropdownSide)
            {
                case VerticalSide.Bottom:
                    dropdownDirection = new Vector2(0, 1);
                    break;
                case VerticalSide.Top:
                    dropdownDirection = new Vector2(0, -1);
                    break;
            }
            currentDir = dropdownDirection.y;
        }
        IEnumerator StartDropdown()
        {
            float time = 0;
            float posY = dropdownRect.anchoredPosition.y;
            while (time < dropdownTime)
            {
                dropdownRect.anchoredPosition = new Vector2(0, posY += speed * Time.deltaTime * currentDir);
                time += Time.deltaTime;
                yield return null;
            }
            currentDir *= -1;
            dropdownButton.interactable = true;
        }
    }
    public enum VerticalSide
    {
        Top, Bottom
    }
}
