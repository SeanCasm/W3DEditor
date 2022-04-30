using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WEditor.Events
{
    public class GameEditorEvent : MonoBehaviour
    {
        [SerializeField] UnityEvent onEditorEnter;
        public static GameEditorEvent instance;
        private void Start()
        {
            instance = this;
        }
        public void OnEditorEnter()
        {
            if (onEditorEnter != null)
            {
                onEditorEnter.Invoke();
            }
        }
    }
}


