using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WInput;
namespace WEditor
{
    public class ConsoleInput : MonoBehaviour
    {
        public static ConsoleInput instance;
        private WInput wInput;
        private void Start()
        {
            instance = this;
            wInput = GameInput.instance.wInput;
        }
        public void EnableAndSetCallbacks(ICommandConsoleActions callbacks)
        {
            wInput.CommandConsole.Enable();
            wInput.CommandConsole.SetCallbacks(callbacks);
        }
    }
}
