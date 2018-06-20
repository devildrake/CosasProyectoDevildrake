using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class JSON_Reader : EditorWindow {

    private string[] str;
    private int items = 1;
    private string path;

    class JSON_Info {
        public string key;
        public string value;

        public JSON_Info(string key, string value) {
            this.key = key;
            this.value = value;
        }
    }

    [MenuItem("Window/JSON")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(JSON_Reader));
    }

    private void OnGUI() {
        path = Application.streamingAssetsPath;
        str = new string[items];
        GUILayout.Label("JSON items", EditorStyles.boldLabel);
        //items = EditorGUILayout.IntField("Items", items);

        //for(int i = 0; i<items; i++) {
        //    str[i] = EditorGUILayout.TextField("hey", str[i]);
        //}

        //JSON_Info i = JsonUtility.FromJson<JSON_Info>();
    }
}
