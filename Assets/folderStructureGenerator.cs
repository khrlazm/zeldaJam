// -------------------------
// FOLDER STRUCTURE SETUP SCRIPT (Editor)
// -------------------------
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


public static class folderStructureGenerator
{
    [MenuItem("Tools/Generate Folder Structure")]
    public static void Generate()
    {
        string[] folders = new string[]
        {
"Assets/Art",
"Assets/Art/Characters",
"Assets/Art/Environment",
"Assets/Art/Props",


"Assets/Audio",
"Assets/Audio/SFX",
"Assets/Audio/Music",


"Assets/Materials",
"Assets/Prefabs",
"Assets/Scenes",
"Assets/Scripts",
"Assets/Scripts/Player",
"Assets/Scripts/Enemies",
"Assets/Scripts/Systems",
"Assets/Scripts/Interaction",


"Assets/Animations",
"Assets/Animations/Player",
"Assets/Animations/Enemies",


"Assets/VFX",
"Assets/UI",
"Assets/UI/Icons",
"Assets/UI/Prefabs",


"Assets/Settings",
"Assets/Settings/Input",
"Assets/Settings/Rendering"
        };


        foreach (string folder in folders)
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                string parent = System.IO.Path.GetDirectoryName(folder).Replace("\\", "/");
                string newFolder = System.IO.Path.GetFileName(folder);
                AssetDatabase.CreateFolder(parent, newFolder);
            }
        }


        AssetDatabase.Refresh();
        Debug.Log("Folder structure generated successfully.");
    }
}
#endif

