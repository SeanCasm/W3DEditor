using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WEditor.Input;
using UnityEngine.UI;
using System.IO;

namespace WEditor.UI
{
    public class MessageHandler : MonoBehaviour
    {
        public static MessageHandler instance;
        [SerializeField] GameObject errorPanel;
        [SerializeField] TMPro.TextMeshProUGUI errorMessage;
        [SerializeField] float messageTime = 3;
        [SerializeField] string errorsPath = "Texts/errors", messagesPath = "Texts/messages";
        [Header("Level save settings")]
        [SerializeField] GameObject messagePopUp;
        [SerializeField] Button accept, cancel;
        [SerializeField] TextMeshProUGUI textMessage;
        private Dictionary<string, string> errors = new();
        private Dictionary<string, string> messages = new();
        private void Awake()
        {
            instance = this;
            ReadErrors();
            ReadMessages();
        }
        public void SetPopUpMessage(string id, string levelName, Action callback)
        {
            string message = messages[id].Replace('_'.ToString(), $"'{levelName}'");
            messagePopUp.SetActive(true);

            textMessage.text = message;
            accept.onClick.AddListener(() =>
            {
                callback();
                EditorCameraInput.instance.Enable();
                messagePopUp.SetActive(false);

            });
            cancel.onClick.AddListener(() =>
            {
                EditorCameraInput.instance.Enable();
                messagePopUp.SetActive(false);
            });

            EditorCameraInput.instance.Disable();
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
            TextAsset fileText = Resources.Load<TextAsset>(messagesPath);
            using StringReader reader = new(fileText.text);
            List<string> lines = new();
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                (string key, string value) = GetErrorTextKeyAndValue(line);
                messages.Add(key, value);
            }

        }
        private void ReadErrors()
        {
            TextAsset fileText = Resources.Load<TextAsset>(errorsPath);
            using StringReader reader = new(fileText.text);
            List<string> lines = new();
            string line;

            while ((line = reader.ReadLine()) != null)
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
