using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace CaptureGem.Editor
{
    public static class BuildTool
    {
        public static string RelativeBuildFolder = "/Build";
        
        private static string[] _includedScenes =
        {
            "Assets/_Projects/Scenes/OfflineScene.unity",
            "Assets/_Projects/Scenes/PlayScene.unity",
            "Assets/_Projects/Scenes/RoomScene.unity"
        };
        
        private static StandaloneBuildSubtarget _cachedSubtarget;
        
        [MenuItem("Capture GEM/Tools/Build/Build All")]
        public static void BuildAll()
        {
            _cachedSubtarget = CurrentSubtarget;
            //! Build server first if already as server, saves time for recompiling
            if (CurrentSubtarget == StandaloneBuildSubtarget.Server)
            {
                BuildServer();
                BuildClient();
            }
            else
            {
                BuildClient();
                BuildServer();
            }

            if (CurrentSubtarget != _cachedSubtarget)
                EditorUserBuildSettings.standaloneBuildSubtarget = _cachedSubtarget;

            OpenBuildFolder();
        }
        
        [MenuItem("Capture GEM/Tools/Build/Build Server Only")]
        public static void BuildServerOnly()
        {
            _cachedSubtarget = CurrentSubtarget;
            BuildServer();
            if (CurrentSubtarget != _cachedSubtarget)
                EditorUserBuildSettings.standaloneBuildSubtarget = _cachedSubtarget;
            OpenBuildFolder("/Server");
        }
        
        [MenuItem("Capture GEM/Tools/Build/Build Client Only")]
        public static void BuildClientOnly()
        {
            _cachedSubtarget = CurrentSubtarget;
            BuildClient();
            if (CurrentSubtarget != _cachedSubtarget)
                EditorUserBuildSettings.standaloneBuildSubtarget = _cachedSubtarget;
            OpenBuildFolder("/Client");
        }

        private static void BuildServer()
        {
            if (CurrentSubtarget != StandaloneBuildSubtarget.Server)
                EditorUserBuildSettings.standaloneBuildSubtarget = StandaloneBuildSubtarget.Server;

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = _includedScenes;
            buildPlayerOptions.locationPathName = FullBuildPath + "/Server/Server.exe";
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
#pragma warning disable 0618
            //! Not obsolete as Unity wants it to be
            buildPlayerOptions.options = BuildOptions.EnableHeadlessMode;
#pragma warning restore 0618


            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            switch (summary.result)
            {
                case BuildResult.Succeeded:
                    Debug.Log($"Build Server succeded: {summary.totalSize / 1000000} MB at {summary.outputPath} with {summary.totalWarnings} warnings. " +
                              $"Time taken: {summary.totalTime.TotalSeconds.ToString("F2")}s.");
                    break;

                case BuildResult.Failed:
                    Debug.LogError($"Build Server failed: {summary.totalErrors} errors and {summary.totalWarnings} warnings.");
                    break;

                case BuildResult.Cancelled:
                    Debug.Log($"Build Server cancelled.");
                    break;
            }
        }

        private static void BuildClient()
        {
            if (CurrentSubtarget != StandaloneBuildSubtarget.Player)
                EditorUserBuildSettings.standaloneBuildSubtarget = StandaloneBuildSubtarget.Player;
            
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = _includedScenes;
            buildPlayerOptions.locationPathName = FullBuildPath + "/Client/Client.exe";
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            
            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            switch (summary.result)
            {
                case BuildResult.Succeeded:
                    Debug.Log($"Build Client succeded: {summary.totalSize / 1000000} MB at {summary.outputPath} with {summary.totalWarnings} warnings. " +
                              $"Time taken: {summary.totalTime.TotalSeconds.ToString("F2")}s.");
                    break;

                case BuildResult.Failed:
                    Debug.LogError($"Build Client failed: {summary.totalErrors} errors and {summary.totalWarnings} warnings.");
                    break;

                case BuildResult.Cancelled:
                    Debug.Log($"Build Client cancelled.");
                    break;
            }
        }
        
        public static void OpenBuildFolder(string extraPath = "")
        {
            var sanitizedPath = FullBuildPath.Replace(@"/", @"\");
            EditorUtility.RevealInFinder(sanitizedPath);
        }
        
        public static StandaloneBuildSubtarget CurrentSubtarget => EditorUserBuildSettings.standaloneBuildSubtarget;
        public static string FullBuildPath => Application.dataPath + "/.." + RelativeBuildFolder;
    }
}
