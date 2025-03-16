using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;

public class ScoringSystem : MonoBehaviour
{
    private DateTime startTime, endTime;

    private string path;

    private bool increaseScore = false;

    private List<GameObject> outposts = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        startTime = DateTime.Now;
        path = Application.persistentDataPath + "/Highscores.data";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CalculateScore();
        }
    }

    public float CalculateScore()
    {
        endTime = DateTime.Now;
        TimeSpan totalTime = endTime - startTime;
        double seconds = totalTime.TotalSeconds;
        float score = Mathf.Round((float)(FindObjectOfType<PlayerController>().GetFuel() * 7500 / seconds));
        Debug.Log("Score: " + score);
        if (increaseScore)
        {
            score *= 1.5f;
            score = MathF.Floor(score);
        }
        Debug.Log("Score After Increase: " + score);
        CheckHigh(score);
        return score;
    }

    private void CheckHigh(float score)
    {
        int difficulty = 0;
        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            using (StreamReader reader = new StreamReader(Application.persistentDataPath + "/Game.data"))
            {
                string line = reader.ReadLine();
                switch (line)
                {
                    case "Flat": difficulty = 0; break;
                    case "Easy": difficulty = 1; break;
                    case "Medium": difficulty = 2; break;
                    case "Hard": difficulty = 3; break;
                    case "Hardcore": difficulty = 4; break;
                    default: difficulty = 0; break;
                }
                reader.Close();
            }

            if (score > float.Parse(lines[difficulty]))
            {
                lines[difficulty] = score.ToString();
                File.WriteAllLines(path, lines);
            }
        } else
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                using (StreamReader reader = new StreamReader(Application.persistentDataPath + "/Game.data"))
                {
                    string line = reader.ReadLine();
                    switch (line)
                    {
                        case "Flat": difficulty = 0; break;
                        case "Easy": difficulty = 1; break;
                        case "Medium": difficulty = 2; break;
                        case "Hard": difficulty = 3; break;
                        case "Hardcore": difficulty = 4; break;
                        default: difficulty = 0; break;
                    }
                    reader.Close();
                }

                for (int i = 0; i < difficulty; i++)
                {
                    writer.WriteLine("0");
                }
                writer.WriteLine(score.ToString());
                for (int i = difficulty; i < 4; i++)
                {
                    writer.WriteLine("0");
                }
            }
        }
    }

    public void OutpostCheck(GameObject outpost, bool status)
    {
        if (status == true)
        {
            outposts.Add(outpost);
            increaseScore = true;
        } else
        {
            outposts.Remove(outpost);
            if (outposts.Count <= 0)
            {
                increaseScore = false;
            }
        }
    }
}
