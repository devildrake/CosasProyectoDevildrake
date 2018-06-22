using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Main : MonoBehaviour {

    public enum lan {en, es};
    public lan l = lan.es;
    public Dictionary<string, string> language;
    private LanguageChange[] languageElements;

	// Use this for initialization
	void Awake () {
        language = new Dictionary<string, string>();
        LoadFile("es");
        languageElements = FindObjectsOfType<LanguageChange>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool LoadFile(string filename) {
        language.Clear();
        string path = Path.Combine(Path.Combine(Application.streamingAssetsPath, "lan"), filename+".json");
        if (File.Exists(path)) {
            
            string json = File.ReadAllText(path);
            LocalizationData jsonItems = JsonUtility.FromJson<LocalizationData>(json);
            
            for(int i = 0; i<jsonItems.items.Length; i++) {
                language.Add(jsonItems.items[i].key, jsonItems.items[i].value);
            }
            Debug.Log("Language loaded correctly");
            return true;
        }
        else {
            Debug.LogError("Language file not found!");
            return false;
        }
    }

    public void ChangeLanguage(string lan) {
        if (LoadFile(lan)) {
            foreach (LanguageChange l in languageElements) {
                l.Change();
            }
        }
    }
}

[System.Serializable]
public class LocalizationData {
    public LocalizationItem[] items;
}

[System.Serializable]
public class LocalizationItem {
    public string key;
    public string value;
}
