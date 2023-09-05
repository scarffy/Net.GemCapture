using System;
using System.Collections;
using System.Collections.Generic;
using CaptureGem.Network;
using CaptureGem.Shell;
using Mirror;
using UnityEngine;

namespace CaptureGem.Core
{
    /// <summary>
    /// This serve as game system. Ideally, we would want to have different script name for game system
    /// </summary>
    public class CoreAnchor : NetworkBehaviour
    {
        public static CoreAnchor Instance;
        [SerializeField] private GamePlayer[] _players;
        [SerializeField] private GemBehaviour _gemBehaviour;

        [SerializeField] private PlayerBase[] _playerBases;

        public void Initialize()
        {
            if (Instance == null)
                Instance = this;
            
            StartCoroutine(RegisterPlayers());
            RegisterPlayerBases();

            Debug.Log(($"[Core]: Core anchor initialized!"));
        }
        
        public override void OnStartServer()
        {
            base.OnStartServer();
            Debug.Log($"[Core]: OnStartServer");
        }

        private IEnumerator RegisterPlayers()
        {
            //! TODO: Revisit this coroutine.
            yield return new WaitForSeconds(.5f);
            
            _players = FindObjectsOfType<GamePlayer>();

            foreach (var player in _players)
            {
                if (player.isLocalPlayer)
                {
                    //! Set player's name
                    player.CmdSetPlayerName(OfflineNetworkAnchor.Instance.GetPlayerName());
                    player.GetComponentInChildren<GamePlayerCollider>().OnTrigger += DropGem;
                }
            }
        }

        private void RegisterPlayerBases()
        {
            foreach (var playerBase in _playerBases)
            {
                playerBase.OnPlayerTrigger += CheckPlayerPoint;
            }
        }

        private void CheckPlayerPoint(GamePlayer player)
        {
            //! Add player's score
            if (!isServer)
            {
                Debug.Log($"[Core]: Checking player is not a server. Returning");
                return;
            }
            
            //! Add player score
            player.RpcAddPlayerScore();

            //! Respawn Gem
            StartCoroutine(_gemBehaviour.GemRespawn());
        }

        public void DropGem(Vector3 vGemPosition, GamePlayer player)
        {
            //! Set gem location to pick up by other player
            //! Player dropped can't move for a while

            player.PlayerTouch();
            Vector3 gemOffset = new Vector3(0, 2.5f, 0);
            _gemBehaviour.transform.position = vGemPosition + gemOffset;

            CmdDropGem();
        }

        [Command(requiresAuthority = false)]
        private void CmdDropGem()
        {
            _gemBehaviour.DropGem();
        }

        public override void OnStopServer()
        {
            Debug.Log($"[Core]: OnStopServer");
            base.OnStopClient();
            foreach (var player in _players)
            {
                if (player.isLocalPlayer)
                {
                    player.GetComponent<GamePlayerCollider>().OnTrigger -= DropGem;
                }
            }
        }
    }
}
