using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WEditor
{
    public class GameInput : MonoBehaviour
    {
        public static GameInput instance;
        public WInput wInput { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
                wInput = new WInput();
            }
            else
            {
                Destroy(this);
            }
        }
    }
}
