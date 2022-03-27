using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WEditor.UI
{
    public class TextMessageHandler : MonoBehaviour
    {
        public static TextMessageHandler instance;
        [SerializeField] GameObject errorPanel;
        [SerializeField] TMPro.TextMeshProUGUI errorMessage;
        [SerializeField] float messageTime;
        [SerializeField] string errorsPath, messagesPath;
        private Dictionary<string, string> errors = new Dictionary<string, string>();
        private Dictionary<string, string> messages = new Dictionary<string, string>();
        private void Awake()
        {
            instance = this;
            ReadErrors();
            ReadMessages();
        }
        public void SetMessage(string id)
        {
            errorPanel.SetActive(true);
            errorMessage.text = messages[id];
            Invoke(nameof(ClearMessage), messageTime);
        }
        public void SetError(string id)
        {
            errorPanel.SetActive(true);
            errorMessage.text = errors[id];
            Invoke(nameof(ClearMessage), messageTime);
        }
        private void ClearMessage()
        {
            errorPanel.SetActive(false);
            errorMessage.text = "";
        }
        private void ReadMessages()
        {
            string[] lines = System.IO.File.ReadAllLines(messagesPath);
            foreach (var line in lines)
            {
                (string key, string value) = GetErrorTextKeyAndValue(line);
                messages.Add(key, value);
            }
        }
        private void ReadErrors()
        {
            string[] lines = System.IO.File.ReadAllLines(errorsPath);
            foreach (var line in lines)
            {
                (string key, string value) = GetErrorTextKeyAndValue(line);
                errors.Add(key, value);
            }
        }
        private Tuple<string, string> GetErrorTextKeyAndValue(string line)
        {
            string[] words = line.Split(':');
            return Tuple.Create(words[0], words[1]);
        }
    }
}
