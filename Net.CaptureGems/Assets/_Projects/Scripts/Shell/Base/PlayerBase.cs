using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace CaptureGem.Shell
{
    public class PlayerBase : NetworkBehaviour
    {
        [SerializeField] private GamePlayer _registerPlayer;
        [SerializeField] private bool _hasRegister;
        
        public Action<GamePlayer> OnPlayerTrigger;
        
        private void OnTriggerEnter(Collider colliderObject)
        {
            if(!isServer)
                return;
            Debug.Log($"[Core(Trigger)]: Is server trigger");
            
            if (colliderObject.CompareTag("Player"))
            {
                //! Register player for first time entry
                if (!_hasRegister)
                {
                    GamePlayer go = colliderObject.GetComponent<GamePlayer>();
                    _registerPlayer = go;
                    
                    _hasRegister = true;
                    Debug.Log($"[Core(Trigger)]: Is server trigger. Registered player");
                }
                else
                {
                    //! Check if player is the registered player
                    GamePlayer go = colliderObject.GetComponent<GamePlayer>();
                    
                    if(_registerPlayer != go)
                        return;

                    //! Check if player carry gem
                    if(!_registerPlayer._bHasCarry)
                        return;
                    
                    //! If player carry gem, invoke game system
                    OnPlayerTrigger?.Invoke(_registerPlayer);
                }
            }
        }
    }
}
