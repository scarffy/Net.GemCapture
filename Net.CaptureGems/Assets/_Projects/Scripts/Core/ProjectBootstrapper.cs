using System.Collections;
using System.Collections.Generic;
using CaptureGem.Data;
using UnityEngine;

namespace CaptureGem.Core
{
    /// <summary>
    /// This serves as starting point for the project
	/// </summary>
    public class ProjectBootstrapper : MonoBehaviour
    {
        public static ProjectBootstrapper Instance;
        
        [Header("Project Settings")]
        [SerializeField] private ProjectSettings _projectSettings;
        
        [Header("Core Anchors")]
        [SerializeField] private OfflineCoreAnchor _offlineCoreAnchor;

        [SerializeField] private CoreAnchor _coreAnchor;

        private void Start()
        {
            if (Instance == null)
                Instance = this;
            
            //! Initialize all anchors
            if(_offlineCoreAnchor != null)
                _offlineCoreAnchor.Initialize();

            if (_coreAnchor != null)
                _coreAnchor.Initialize();
        }

        public ProjectSettings ProjectSettings => _projectSettings;
    }
}
