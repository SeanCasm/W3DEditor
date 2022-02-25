using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WEditor.Scenario
{
    public class EditorDropdown : MonoBehaviour
    {
        [SerializeField] GameObject dropdownReference;
        [SerializeField] protected float dropdownTime;
        [SerializeField] protected float dropdownDistance;
        [SerializeField] protected Button dropdownButton;
        protected Vector2 dropdownDirection; // to appears
        protected RectTransform dropdownRect;
        protected float speed, currentDir;
        protected void Start()
        {
            dropdownRect = dropdownReference.GetComponent<RectTransform>();
            speed = dropdownDistance / dropdownTime;
        }
        public void Button_StartDropdown()
        {
            StartCoroutine("StartDropdown");
            dropdownButton.interactable = false;
        }
    }
}
