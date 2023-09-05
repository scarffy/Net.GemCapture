using System;
using System.Collections;
using System.Collections.Generic;
using CaptureGem.Network;
using CaptureGem.Data;
using CaptureGem.Shell;
using CaptureGem.UI;
using UnityEngine;

namespace CaptureGem.Core
{
    public class OfflineCoreAnchor : MonoBehaviour
    {
        [SerializeField] private RoomManager _roomManager;
        [SerializeField] private OfflineNetworkAnchor _networkAnchor;
        [SerializeField] private OfflineUIAnchor _uiAnchor;

        /// <summary>
        /// Initialize all anchors
        /// </summary>
        public void Initialize()
        {
            _networkAnchor.Initialize();
            _uiAnchor.Initialize();

            _uiAnchor.UIConnection.OnSetPlayerName += _networkAnchor.SetPlayerName;
            _uiAnchor.UIConnection.OnConnectToServer += _networkAnchor.ConnectToServer;
            
            _networkAnchor.SetNetworkAddress(ProjectBootstrapper.Instance.ProjectSettings.NetworkAddress);
        }

        /// <summary>
        /// Clean up to avoid memory leak
        /// </summary>
        public void OnDestroy()
        {
            _uiAnchor.UIConnection.OnSetPlayerName -= _networkAnchor.SetPlayerName;
            _uiAnchor.UIConnection.OnConnectToServer -= _networkAnchor.ConnectToServer;
        }

        public RoomManager RoomManager => _roomManager;
        public OfflineNetworkAnchor NetworkAnchor => _networkAnchor;
        public OfflineUIAnchor UIAnchor => _uiAnchor;
    }
}
