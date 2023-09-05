using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaptureGem.Shell
{
    public class GamePlayerCollider : MonoBehaviour
    {
        [SerializeField] private bool _isLocalPlayer = false;
        
        public Action<Vector3, GamePlayer> OnTrigger;
        
        public void SetLocalPlayer(bool bIsLocalPlayer)
        {
            _isLocalPlayer = bIsLocalPlayer;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(!_isLocalPlayer)
                return;
            
            Debug.Log($"[Avatar]: {other.GetComponent<GamePlayer>().playerName} has entered range");
            OnTrigger?.Invoke(transform.position, GetComponentInParent<GamePlayer>());
        }

        private void OnTriggerExit(Collider other)
        {
            if(!_isLocalPlayer)
                return;
            
            Debug.Log($"[Avatar]: {other.GetComponent<GamePlayer>().playerName} has exited range");
        }
    }
}
