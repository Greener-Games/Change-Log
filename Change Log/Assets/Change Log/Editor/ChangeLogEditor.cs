using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace GG.ChangeLog.Editor
{
/// <summary>
///     Editor script giving the option to create and edit versions of the current project.
///     Each version has a list of changes
/// </summary>
public class ChangeLogEditor : EditorWindow
{
    public const float optionAreaWidth = 150;

    /// <summary>
    ///     The editor window
    /// </summary>
    static ChangeLogEditor _window;

    /// <summary>
    ///     The changelog of this project
    /// </summary>
    static ChangeLogData changeLogData;

    int currentIndex = -1;
    Vector2 dataScroll = Vector2.zero;

    Vector2 optionsScroll = Vector2.zero;

    static ChangeLogEditor Window
    {
        get
        {
            if (_window == null)
            {
                _window = GetWindow<ChangeLogEditor>(true, "Change Log", true);
                _window.minSize = new Vector2(1000, 400);
            }

            return _window;
        }
    }

    [MenuItem("Developer/Change Log/Open", false, 50)]
    static void OpenChangeLog()
    {
        Window.Show();
        changeLogData = ChangeLogData.LoadAsset();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical("box", GUILayout.Width(optionAreaWidth));
            {
                DrawOptionsArea();

                if (GUILayout.Button("Add New"))
                {
                    ChangeLogEntry changeLogVersion = new ChangeLogEntry();
                    changeLogData.versions.Add(changeLogVersion);
                }

                if (GUILayout.Button("Save"))
                {
                    EditorUtility.SetDirty(changeLogData);
                }

                if (GUILayout.Button("Export As Json"))
                {
                    string json = JsonConvert.SerializeObject(changeLogData, Formatting.Indented);
                    using (StreamWriter writer = new StreamWriter(Path.Combine(Application.streamingAssetsPath, "ChangeLog.json")))
                    {
                        writer.Write(json);
                    } 
                    Debug.Log(json);
                }
                
                if (GUILayout.Button("Import Json"))
                {
                    string path = EditorUtility.OpenFilePanel("Load Json Text file", "", "txt");
                    if (path.Length != 0)
                    {
                        string fileContent = File.ReadAllText(path);
                        ChangeLogData data = JsonConvert.DeserializeObject<ChangeLogData>(fileContent);
                        changeLogData.versions = data.versions;
                    }
                }
            }
            GUILayout.EndVertical();

            GUILayout.Space(10);
            GUILayout.BeginVertical("box");
            {
                DrawMainPanel();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
    }

    void DrawOptionsArea()
    {
        optionsScroll = GUILayout.BeginScrollView(optionsScroll);
        {
            int lastVersion = currentIndex;
            currentIndex = GUILayout.SelectionGrid(currentIndex, changeLogData.versions.Select(x => x.versionId + " (" + x.versionName + ")").ToArray(), 1);

            if (lastVersion != currentIndex)
            {
                GUI.FocusControl(null);
            }
        }
        GUILayout.EndScrollView();
    }

    void DrawMainPanel()
    {
        if (currentIndex < 0)
        {
            return;
        }
        
        int versionIndexToRemove = -1;
        int indexToRemove = -1;
        dataScroll = GUILayout.BeginScrollView(dataScroll);
        {
            ChangeLogEntry log = changeLogData.versions[currentIndex];

            log.versionId = EditorGUILayout.TextField("Version Number", log.versionId);
            log.versionName = EditorGUILayout.TextField("Version Name", log.versionName);

            if (GUILayout.Button("Add"))
            {
                log.changes.Add("");
            }


            for (int index = 0; index < log.changes.Count; index++)
            {
                GUILayout.BeginHorizontal();
                {
                    log.changes[index] = EditorGUILayout.TextField(log.changes[index]);

                    if (GUILayout.Button("X", GUILayout.Width(50)))
                    {
                        indexToRemove = index;
                    }
                }
                GUILayout.EndHorizontal();
            }

            if (indexToRemove >= 0)
            {
                log.changes.RemoveAt(indexToRemove);
            }
        }
        GUILayout.EndScrollView();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Delete Entry"))
        {
            versionIndexToRemove = currentIndex;
        }

        if (versionIndexToRemove != -1)
        {
            changeLogData.versions.RemoveAt(versionIndexToRemove);
            currentIndex = -1;
        }
    }
}
}
