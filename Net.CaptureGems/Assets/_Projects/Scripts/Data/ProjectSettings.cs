using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaptureGem.Data
{
    [CreateAssetMenu(fileName = "ProjectSettings", menuName = "Scriptable Objects/Project Settings", order = 1)]
    public class ProjectSettings : ScriptableObject
    {
        [SerializeField] private bool _debugMode = false;

        [Header("Networking")] [SerializeField]
        private string _networkAddress = "127.0.0.1";

        public string NetworkAddress => _networkAddress;
    }
}
