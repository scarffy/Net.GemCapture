using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.PlayerLoop;

namespace CaptureGem.Shell
{
    public class GemBehaviour : NetworkBehaviour
    {
        [SerializeField] private GamePlayer _triggeredPlayer;
        [SerializeField] private GamePlayer _cacheTriggerdPlayer;
        
        [SerializeField] private Vector3 _defaultGemPosition = new Vector3(0,3f,0);

        [SyncVar] public bool _isPickedUp = false;

        [SerializeField] private bool _isRespawn = false;
        private Vector3 _offSetPosition = new Vector3(0f, 3.5f, 0f);
        
        private void Update()
        {
            if(isClient)
                transform.RotateAround(transform.position, transform.up, Time.deltaTime * 75f);

            if (isServer && _isPickedUp)
                transform.position = _cacheTriggerdPlayer.transform.position + _offSetPosition;
            
        }

        private void OnTriggerEnter(Collider colliderObject)
        {
            if(!isServer)
                return;
            
            if(_isPickedUp)
                return;
            
            if(_isRespawn)
                return;
            
            if (colliderObject.CompareTag("Player"))
            {
                _triggeredPlayer = colliderObject.GetComponent<GamePlayer>();

                if (_cacheTriggerdPlayer == null)
                {
                    _triggeredPlayer.RpcReducePlayerSpeed(true);
                    _cacheTriggerdPlayer = _triggeredPlayer;
                    _isPickedUp = true;
                    _triggeredPlayer._bHasCarry = true;
                    return;
                }
                
                if (_triggeredPlayer == _cacheTriggerdPlayer)
                    return;
                else
                {
                    _cacheTriggerdPlayer.RpcReducePlayerSpeed(false);
                    _triggeredPlayer.RpcReducePlayerSpeed(true);

                    _cacheTriggerdPlayer = _triggeredPlayer;
                    _isPickedUp = true;
                    _triggeredPlayer._bHasCarry = false;
                    _cacheTriggerdPlayer._bHasCarry = true;
                    Debug.Log($"[Core(Gem)]: Player picked up gem");
                }
            }
        }

        [Server]
        public void DropGem()
        {
            _isPickedUp = false;
        }

        public IEnumerator GemRespawn()
        {
            //! Set isRespawn to true to avoid gem being pick up
            _isRespawn = true;
            
            //! Reset isPickup for players
            _isPickedUp = false;
            
            //! Wait for 2 second before continue the game
            yield return new WaitForSeconds(2f);
            
            //! Reset gem position
            transform.position = _defaultGemPosition;

            //! Set isRespawn to false for player able to pick up
            _isRespawn = false;

            _triggeredPlayer = null;
            _cacheTriggerdPlayer = null;
        }
    }
}
