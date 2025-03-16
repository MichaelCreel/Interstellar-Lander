using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class HighScoreLoader : MonoBehaviour
{
    [SerializeField] private TMP_Text flatHi, easyHi, mediumHi, hardHi, hardcoreHi;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadHighscores()
    {
        if (File.Exists(Application.persistentDataPath + "/Highscores.data")) {
            string[] lines = File.ReadAllLines(Application.persistentDataPath + "/Highscores.data");
            flatHi.text = "HI: " + lines[0];
            easyHi.text = "HI: " + lines[1];
            mediumHi.text = "HI: " + lines[2];
            hardHi.text = "HI: " + lines[3];
            hardcoreHi.text = "HI: " + lines[4];
        } else
        {
            flatHi.text = "HI: 0";
            easyHi.text = "HI: 0";
            mediumHi.text = "HI: 0";
            hardHi.text = "HI: 0";
            hardcoreHi.text = "HI: 0";
        }
    }
}
