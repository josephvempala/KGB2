using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GitHub.Unity
{
    [InitializeOnLoad]
    public class ExtensionLoader : ScriptableSingleton<ExtensionLoader>
    {
        private const string sourceModePath = "Assets/Editor/build/";
        private const string realPath = "Assets/Plugins/GitHub/Editor/";

        private const string GITHUB_UNITY_DISABLE = "GITHUB_UNITY_DISABLE";

        private static bool inSourceMode;

        private static readonly string[] assemblies20 =
        {
            "System.Threading.dll", "AsyncBridge.Net35.dll", "ReadOnlyCollectionsInterfaces.dll", "GitHub.Api.dll",
            "GitHub.Unity.dll"
        };

        private static readonly string[] assemblies45 = {"GitHub.Api.45.dll", "GitHub.Unity.45.dll"};
        [SerializeField] private bool initialized = true;

        static ExtensionLoader()
        {
            if (IsDisabled) return;
            EditorApplication.update += Initialize;
        }

        public bool Initialized
        {
            get => initialized;
            set
            {
                initialized = value;
                Save(true);
            }
        }

        private static bool IsDisabled => Environment.GetEnvironmentVariable(GITHUB_UNITY_DISABLE) == "1";

        private static void Initialize()
        {
            EditorApplication.update -= Initialize;

            // we're always doing this right now because if the plugin gets updated all the meta files will be disabled and we need to re-enable them
            // we should probably detect if our assets change and re-run this instead of doing it every time
            //if (!ExtensionLoader.instance.Initialized)
            {
                var scriptPath = Path.Combine(Application.dataPath,
                    "Editor" + Path.DirectorySeparatorChar + "GitHub.Unity" + Path.DirectorySeparatorChar +
                    "EntryPoint.cs");
                inSourceMode = File.Exists(scriptPath);
                ToggleAssemblies();
                //ExtensionLoader.instance.Initialized = true;
                AssetDatabase.SaveAssets();
            }
        }

        private static void ToggleAssemblies()
        {
            var path = inSourceMode ? sourceModePath : realPath;
#if NET_4_6
            ToggleAssemblies(path, assemblies20, false);
            ToggleAssemblies(path, assemblies45, true);
#else
            ToggleAssemblies(path, assemblies45, false);
            ToggleAssemblies(path, assemblies20, true);
#endif
        }

        private static void ToggleAssemblies(string path, string[] assemblies, bool enable)
        {
            foreach (var file in assemblies)
            {
                var filepath = path + file;
                var importer = AssetImporter.GetAtPath(filepath) as PluginImporter;
                if (importer == null)
                {
                    Debug.LogFormat("GitHub for Unity: Could not find importer for {0}. Some functionality may fail.",
                        filepath);
                    continue;
                }

                if (importer.GetCompatibleWithEditor() != enable)
                {
                    importer.SetCompatibleWithEditor(enable);
                    importer.SaveAndReimport();
                }
            }
        }
    }
}