using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class JSON_Reader : EditorWindow {

    public LocalizationData localizationData;
    private Vector2 scrollPos;

    [MenuItem("Window/JSON")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(JSON_Reader));
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
    }

    private void LoadData() {
        string path = EditorUtility.OpenFilePanel("Selecciona el archivo", Application.streamingAssetsPath, "json");

        if (!string.IsNullOrEmpty(path)) {
            string jsonData = File.ReadAllText(path);
            localizationData = JsonUtility.FromJson<LocalizationData>(jsonData);
        }
    }

    private void SaveData() {
        string path = EditorUtility.SaveFilePanel("Guardar", Application.streamingAssetsPath, "", "json");

        if (!string.IsNullOrEmpty(path)) {
            string jsonData = JsonUtility.ToJson(localizationData);
            File.WriteAllText(path, jsonData);
        }
    }
}

/*
[System.Serializable]
public class LocalizationData {
    public LocalizationItem[] items;
}

[System.Serializable]
public class LocalizationItem {
    public string key;
    public string value;
}
*/
