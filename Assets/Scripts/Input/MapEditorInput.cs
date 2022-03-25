using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WInput;

namespace WEditor.Input
{

    public class MapEditorInput : MonoBehaviour
    {
        public static MapEditorInput instance;
        private WInput wInput;
        private void Start()
        {
            instance = this;
            wInput = GameInput.instance.wInput;
        }

        public void EnableAndSetCallbacks(IMapEditorActions callbacks)
        {
            wInput.MapEditor.Enable();
            wInput.MapEditor.SetCallbacks(callbacks);
        }
        public void ChangeInputActiveState(bool enable)
        {
            if (enable) wInput.MapEditor.Enable();
            else wInput.MapEditor.Disable();
        }
        public void ChangeInputOnInventory(bool enable)
        {
            if (enable)
            {
                wInput.MapEditor.Aim.Enable();
                wInput.MapEditor.Click.Enable();
            }
            else
            {
                wInput.MapEditor.Aim.Disable();
                wInput.MapEditor.Click.Disable();
            }
        }
    }
}
