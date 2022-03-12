using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WEditor
{
    public class SaveSlotHandler : MonoBehaviour
    {
        [SerializeField] Transform slotsParent;
        private int totalSlots = 0;
        private void Start()
        {
            if (PlayerPrefs.HasKey("slots"))
            {
                totalSlots = PlayerPrefs.GetInt("slots");
            }
            else
            {
                
            }
        }
    }
}