using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class JSON_Reader : EditorWindow {

    public LocalizationData localizationData;
    private Vector2 scrollPos;
    private string fileNameOpened;

    [MenuItem("Tools/JSON Reader (Language)")]
    public static void ShowWindow() {
        GetWindow(typeof(JSON_Reader),false, "JSON Reader");
    }

    private void OnGUI() {
        if(localizationData != null) {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos); //scroll view
            {
                SerializedObject serializedObject = new SerializedObject(this);
                SerializedProperty serializedProperty = serializedObject.FindProperty("localizationData");
                EditorGUILayout.PropertyField(serializedProperty, true);
                serializedObject.ApplyModifiedProperties();
            }
            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("SAVE")){
                SaveData();
            }
        }

        if (GUILayout.Button("LOAD")) {
            LoadData();
        }

        if (GUILayout.Button("CREATE")) {
            CreateNewData();
        }
    }

    private void CreateNewData() {
        localizationData = new LocalizationData();
        GetWindow(typeof(JSON_Reader)).titleContent = new GUIContent("JSON Reader");
    }

    private void LoadData() {
        string path = EditorUtility.OpenFilePanel("Selecciona el archivo", Application.streamingAssetsPath + "/lan", "json");
        string[] s = path.Split(new char[] { '/', '.' });
        fileNameOpened = s[s.Length - 2];

        if (!string.IsNullOrEmpty(path)) {
            string jsonData = File.ReadAllText(path);
            localizationData = JsonUtility.FromJson<LocalizationData>(jsonData);
            GetWindow(typeof(JSON_Reader)).titleContent = new GUIContent(fileNameOpened + ".json");
        }
    }

    private void SaveData() {
        string path = EditorUtility.SaveFilePanel("Guardar", Application.streamingAssetsPath + "/lan", "", "json");
        string[] s = path.Split(new char[] { '/', '.' });
        fileNameOpened = s[s.Length - 2];

        if (!string.IsNullOrEmpty(path)) {
            string jsonData = JsonUtility.ToJson(localizationData);
            File.WriteAllText(path, jsonData);
            GetWindow(typeof(JSON_Reader)).titleContent = new GUIContent(fileNameOpened + ".json");
        }
    }
}
