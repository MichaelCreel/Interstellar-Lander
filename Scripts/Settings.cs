    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class Settings : MonoBehaviour
{
    string path = null;

    public KeyCode[] keybinds = new KeyCode[4];

    private KeyCode[] defaults = new KeyCode[] { KeyCode.D, KeyCode.A, KeyCode.Space, KeyCode.Escape };

    public int volume = 50;

    private enum Controls
    {
        CW,
        CCW,
        Thrust,
        Pause
    }

    // Start is called before the first frame update
    void Start()
    {
        path = Application.persistentDataPath + "/Keybinds.controls";
        Debug.Log(path);
        LoadKeybinds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveKeybinds()
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            for (int i = 0; i < keybinds.Length; i++)
            {
                if (keybinds[i] != KeyCode.None)
                {
                    writer.WriteLine(keybinds[i].ToString());
                } else
                {
                    writer.WriteLine(defaults[i].ToString());
                }
            }
            writer.WriteLine(volume.ToString());
            writer.Close();
            PlayerController playerController = FindObjectOfType<PlayerController>();
            if (playerController != null)
            {
                playerController.HotloadKeybinds();
            }
        }
    }

    public void LoadKeybinds()
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                for (int i = 0; i < keybinds.Length; i++)
                {
                    string line = reader.ReadLine();
                    if (line != "" && line != string.Empty && line != null)
                    {
                        keybinds[i] = (KeyCode)System.Enum.Parse(typeof(KeyCode), line);
                    } else
                    {
                        keybinds[i] = defaults[i];
                    }
                }
                string line2 = reader.ReadLine();
                if (line2 == "" && line2 == string.Empty && line2 == null)
                {
                    volume = 50;
                } else
                {
                    int vol = 50;
                    if (int.TryParse(line2, out vol))
                    {
                        volume = vol;
                    } else
                    {
                        volume = 50;
                    }
                }
                reader.Close();
            }
        } else
        {
            SaveKeybinds();
            LoadKeybinds();
        }
    }
}
