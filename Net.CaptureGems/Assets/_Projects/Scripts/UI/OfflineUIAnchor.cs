using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CaptureGem.UI
{
    public class OfflineUIAnchor : MonoBehaviour
    {
        [SerializeField] private UIServerConnection _connectionCanvas;
        [SerializeField] private GameObject _serverConnectionCanvas;
        
        public void Initialize()
        {
            // Check if null, spawn connection canvas
            if (_connectionCanvas == null)
            {
                GameObject go = Instantiate(_serverConnectionCanvas,transform.position,Quaternion.identity);
                _connectionCanvas = go.GetComponent<UIServerConnection>();
            }
        }

        public UIServerConnection UIConnection => _connectionCanvas;
    }
}
