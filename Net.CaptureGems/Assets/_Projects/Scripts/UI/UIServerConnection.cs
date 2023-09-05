using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CaptureGem.UI
{
    public class UIServerConnection : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private TMP_InputField _ipInputField;
        [SerializeField] private Button _connectToServerButton;
        
        public Action<string> OnConnectToServer;
        public Action<string> OnSetPlayerName;
        
        private void Start()
        {
            _connectToServerButton.onClick.AddListener(ConnectToServer);
        }

        /// <summary>
        /// Removing listener to avoid memory leak
        /// </summary>
        private void OnDestroy()
        {
            _connectToServerButton.onClick.RemoveAllListeners();
        }
        
        /// <summary>
        /// Invoke connect to server with IP address in test field
        /// Invoke player name that is set in the beginning
        /// </summary>
        private void ConnectToServer()
        {
            OnConnectToServer?.Invoke(_ipInputField.text);
            OnSetPlayerName?.Invoke(_nameInputField.text);
        }
    }
}
