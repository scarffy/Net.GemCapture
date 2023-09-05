using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaptureGem.Network
{
    public class OfflineNetworkAnchor : MonoBehaviour
    {
        public static OfflineNetworkAnchor Instance;
        
        [SerializeField] private RoomManager _roomManager;
        
        private string _playerName;

        public void Initialize()
        {
            if (Instance == null)
                Instance = this;
            
            Debug.Log($"[Network] Network anchor initialized!");
        }

        public void SetNetworkAddress(string networkAddress)
        {
            _roomManager.networkAddress = networkAddress;
        }
        
        public void ConnectToServer(string networkAddress)
        {
            if (networkAddress == "" || string.IsNullOrEmpty(networkAddress))
            {
                Debug.Log($"[Network]: Call Connect to Server ip address is empty. Connecting to default instead");
                
                //! TODO: Connect to project setting's default
                _roomManager.StartClient();
                
                return;
            }
            
            Debug.Log($"[Network]: Call Connect to Server ip address {networkAddress}");
            _roomManager.networkAddress = networkAddress;
            _roomManager.StartClient();
        }

        public void SetPlayerName(string playerName)
        {
            //! Update player name
            _playerName = playerName;
        }

        public string GetPlayerName() => _playerName;
        public RoomManager RoomManager => _roomManager;
    }
}
