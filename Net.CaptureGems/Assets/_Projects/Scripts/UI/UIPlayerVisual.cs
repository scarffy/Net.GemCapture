using System;
using System.Collections;
using System.Collections.Generic;
using CaptureGem.Shell;
using UnityEngine;
using TMPro;

namespace CaptureGem.UI
{
    using CaptureGem.Shell;
    public class UIPlayerVisual : MonoBehaviour
    {
        private Transform _camera;
        
        [SerializeField] private GamePlayer _gamePlayer;
        [SerializeField] private Transform _uiCanvas;

        [SerializeField] private TMP_Text _playerNameText;
        [SerializeField] private TMP_Text _playerScoreText;

        private void Start()
        {
            _camera = Camera.main.transform;

            SetPlayerScore($"Score: 0");
            
            _gamePlayer.OnNameChanged += SetPlayerName;
            _gamePlayer.OnAddScore += SetPlayerScore;
        }

        private void LateUpdate()
        {
            //! UI to always look at the camera
            _uiCanvas.transform.LookAt( _uiCanvas.position + _camera.rotation * Vector3.forward);
        }

        public void SetPlayerName(string value)
        {
            _playerNameText.SetText(value);
        }

        public void SetPlayerScore(string value)
        {
            _playerScoreText.SetText(value);
        }
        
        public void SetPlayerScore(int newScore)
        {
            _playerScoreText.SetText( $"Score: {newScore.ToString()}");
        }

        /// <summary>
        /// Clean up when destroy object to avoid memory leak
        /// </summary>
        private void OnDestroy()
        {
            _gamePlayer.OnNameChanged -= SetPlayerName;
            _gamePlayer.OnAddScore -= SetPlayerScore;
        }
    }
}
