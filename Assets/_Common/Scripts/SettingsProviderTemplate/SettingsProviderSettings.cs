using UnityEngine;
using Sirenix.OdinInspector;


#if UNITY_EDITOR
using UnityEditor;
[GameSettings(typeof(SettingsProviderSettings))]
#endif

public class SettingsProviderSettings : SingletonSCO<SettingsProviderSettings>
{
    [Header("SettingsProvider Settings")]
    public string nameSpace = "_WorldGame";

    [Header("SettingsProvider Generator")]
    public string settingsName = "Exemple";

    [Button]
    public void CreateProvider()
    {
#if UNITY_EDITOR
        // Validate input and build class name
        if (string.IsNullOrWhiteSpace(settingsName))
        {
            EditorUtility.DisplayDialog("Settings Generator", "Please provide a valid settings name.", "OK");
            return;
        }

        string trimmedName = settingsName.Trim();
        string className = trimmedName + "Settings";

        // Ask for file path for the ScriptableObject script
        string scriptAssetPath = EditorUtility.SaveFilePanelInProject(
            "Create Settings Script",
            className + ".cs",
            "cs",
            "Choose where to create the settings script"
        );
        if (string.IsNullOrEmpty(scriptAssetPath))
        {
            return; // User cancelled
        }

        // Ensure the folder exists
        string scriptFolder = System.IO.Path.GetDirectoryName(scriptAssetPath).Replace('\\', '/');
        if (!AssetDatabase.IsValidFolder(scriptFolder))
        {
            // Create the folder hierarchy if it doesn't exist
            string[] folders = scriptFolder.Split('/');
            string current = "Assets";
            for (int i = 1; i < folders.Length; i++)
            {
                string next = current + "/" + folders[i];
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, folders[i]);
                current = next;
            }
        }

        // Create the settings ScriptableObject script
        string soScript =
$@"using UnityEngine;

#if UNITY_EDITOR
[GameSettings(typeof({className}))]
#endif
public class {className} : SingletonSCO<{className}>
{{
    [field: SerializeField] public int ExampleValue {{ get; private set; }} = 1;
}}
";
        System.IO.File.WriteAllText(scriptAssetPath, soScript);
        AssetDatabase.ImportAsset(scriptAssetPath);

        // Ensure an Editor folder exists next to the script
        string editorFolderPath = scriptFolder + "/Editor";
        if (!AssetDatabase.IsValidFolder(editorFolderPath))
        {
            AssetDatabase.CreateFolder(scriptFolder, "Editor");
        }

        // Create the SettingsProvider script inside the Editor folder
        string providerClassName = className + "Provider";
        string providerAssetPath = editorFolderPath + "/" + providerClassName + ".cs";
        string providerScript = $@"
#if UNITY_EDITOR
using UnityEditor;

public class {providerClassName} : SettingProviderBase<{className}>
{{
    public {providerClassName}(string path, SettingsScope scope) : base(path, scope) {{ }}

    [SettingsProvider]
    public static SettingsProvider GetSettingsProvider() => CreateProviderForProjectSettings();
}}
#endif
";
        System.IO.File.WriteAllText(providerAssetPath, providerScript);
        AssetDatabase.ImportAsset(providerAssetPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Highlight created files
        var soAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(scriptAssetPath);
        if (soAsset != null)
        {
            EditorGUIUtility.PingObject(soAsset);
        }
        var providerAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(providerAssetPath);
        if (providerAsset != null)
        {
            EditorGUIUtility.PingObject(providerAsset);
        }
#endif
    }
}

