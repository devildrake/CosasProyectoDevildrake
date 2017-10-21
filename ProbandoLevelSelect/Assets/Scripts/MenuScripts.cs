using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScripts : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LOADLEVEL1() {
        SceneManager.LoadScene(1);

    }



    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
}
