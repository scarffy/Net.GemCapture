using System;
using UnityEngine;
using CaptureGem.UI;
using Mirror;

namespace CaptureGem.Network
{
    public class RoomPlayer : NetworkRoomPlayer
    {
        [SyncVar(hook = nameof(HandleNameChange))] public string _playerName;
        
        public override void OnStartLocalPlayer()
        {
            if(OfflineNetworkAnchor.Instance != null)
                CmdSetPlayerName(OfflineNetworkAnchor.Instance.GetPlayerName());
            else
            {
                Debug.Log($"[Network]: Network Anchor is null");
            }
        }
        
        [Command]
        public void CmdSetPlayerName(string value)
        {
            _playerName = value;
        }
        
        public void HandleNameChange(string oldValue, string newValue)
        {
            _playerName = newValue;
        }
    }
}
