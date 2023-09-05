using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace CaptureGem.Shell
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook _cinemachine;
        
        public void Initialize()
        {
            if (_cinemachine == null)
                _cinemachine = FindObjectOfType<CinemachineFreeLook>();
            
            //! Assign camera to follow local player position and rotation
            _cinemachine.Follow = transform;
            _cinemachine.LookAt = transform;
        }
    }
}
        