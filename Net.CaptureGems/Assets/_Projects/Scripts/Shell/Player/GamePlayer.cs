using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

namespace CaptureGem.Shell
{
    public class GamePlayer : NetworkBehaviour
    {
        [SerializeField] private GamePlayerMovement _playerMovement;
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField] private GamePlayerCollider _playerCollider;

        [Space(10)]
        [SyncVar(hook = nameof(OnPlayerNameChanged))] public string playerName = "defaultName";

        [SyncVar] public bool _bHasCarry = false;
        [SyncVar] public int _playerScore;

        #region Private variable
        private readonly WaitForSeconds _stopMovementTime = new WaitForSeconds(1f);
        private bool _isCountDown = false;
        
        public Action<string> OnNameChanged;
        public Action<int> OnAddScore;
        #endregion

        /// <summary>
        /// There may be other way to validate player's authority
        /// This is the best and easiest way for other dev to understand
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            //! Set movement input to local player
            _playerMovement.enabled = true;
            
            //! Set camera to follow local player
            _cameraFollow.enabled = true;

            //! Initilize camere follow
            _cameraFollow.Initialize();
            
            //! Set player collider to local object
            _playerCollider.SetLocalPlayer(true);
        }

        [Command]
        public void CmdSetPlayerName(string value)
        {
            //! Tell server to set new player name
            playerName = value;
        }

        private void OnPlayerNameChanged(string oldValue, string newValue)
        {
            //! Tell other client we have new name
            playerName = newValue;
            
            //! Set name on UI
            OnNameChanged?.Invoke(newValue);
        }

        [ClientRpc]
        public void RpcReducePlayerSpeed(bool bIsCarryGem)
        {
            //! Reduce player speed when carry gem.
            _playerMovement.CarryGem(bIsCarryGem);
            
            //! Set player to carry gem
            _bHasCarry = bIsCarryGem;
        }

        /// <summary>
        /// Stop movement input for a while after player touch
        /// </summary>
        public void PlayerTouch()
        {
            //! Check if player carry gem
            if(!_bHasCarry)
                return;

            //! Check if player can move
            if(_isCountDown)
                return;

            //! Start count down
            _isCountDown = true;
            
            //! Start count down coroutine
            StartCoroutine(OnPlayerTouch());
        }

        private IEnumerator OnPlayerTouch()
        {
            //! Stop player movement input
            _playerMovement.enabled = false;

            //! Count down
            yield return _stopMovementTime;

            //! Enable player movement input
            _playerMovement.enabled = true;
            
            //! Reset player count down
            _isCountDown = false;
        }

        [ClientRpc]
        public void RpcAddPlayerScore()
        {
            //! Add player score to sync with server
            _playerScore++;
            
            //! Invoke score to set in ui
            OnAddScore?.Invoke(_playerScore);
            
            //! Set player not carry gem
            CmdSetCarryGem(false);
        }
        
        [Command]
        public void CmdSetCarryGem(bool hasGem)
        {
            _bHasCarry = hasGem;
        }
    }
}
