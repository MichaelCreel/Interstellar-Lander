using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] terrainPieces, outpostPieces;

    [SerializeField] private int maximumAngle = 75, minimumAngle = 30;

    private Vector2 rightPosition = new Vector2(0, 0), leftPosition = new Vector2(0, 0);

    private int flatNumberBottom = 11, flatNumberTop = 11;

    [SerializeField] private Camera cam;

    private bool generateAllowed = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FlatNumberWait());
    }

    // Update is called once per frame
    void Update()
    {
        if (generateAllowed)
        {
            if (leftPosition.x > cam.ViewportToWorldPoint(new Vector3(0, 1, 0)).x)
            {
                leftPosition = GenerateTerrianLeft(leftPosition);
            }
            if (rightPosition.x < cam.ViewportToWorldPoint(new Vector3(1, 1, 0)).x)
            {
                rightPosition = GenerateTerrianRight(rightPosition);
            }
        }
    }

    private Vector2 GenerateTerrianRight(Vector2 startPosition) //Pass in vector 2 with position to start terrain, and it will return next position that can be passed in
    {
        return Generation(startPosition, 0, 1);
    }

    private Vector2 GenerateTerrianLeft(Vector2 startPosition) //Pass in vector 2 with position to start terrain, and it will return next position that can be passed in
    {
        return Generation(startPosition, 180, -1);
    }

    private Vector2 Generation(Vector2 startPosition, int yRot, int posMult)
    {
        int angle = 0;
        int rand = Random.Range(0, 43);
        bool outpost = false;
        if (rand < flatNumberBottom)
        {
            angle = Random.Range(minimumAngle, maximumAngle + 1);
        } else if (rand > flatNumberTop)
        {
            angle = Random.Range(-maximumAngle, -minimumAngle + 1);
            if (startPosition.y < 1)
            {
                angle = -angle;
            }
        } else
        {
            angle = 0;
            int outpostChance = Random.Range(0, 20);
            if (outpostChance == 4)
            {
                outpost = true;
            }
        }
        float yPos = Mathf.Tan((float)angle * Mathf.PI / 180);
        GameObject terrain = Instantiate(terrainPieces[Random.Range(0, terrainPieces.Length)], startPosition + new Vector2((float)posMult * 0.5f, yPos / 2), Quaternion.Euler(0, yRot, angle));
        terrain.transform.localScale = new Vector2(1 / Mathf.Cos((float)angle * Mathf.PI / 180f) + Mathf.Cos((float)angle * Mathf.PI / 180f) * 0.05f, 0.2f);
        terrain.transform.parent = transform;
        Vector2 newPos = startPosition + new Vector2(posMult * 1, yPos);

        if (outpost)
        {
            GameObject newOutpost = Instantiate(outpostPieces[Random.Range(0, outpostPieces.Length)], startPosition + new Vector2(0.4761905f * posMult, 0.675f), Quaternion.identity, terrain.transform);
            newOutpost.transform.localScale = new Vector3(7, 35, 1);
        }

        return newPos;
    }

    public void RightTrigger()
    {
        rightPosition = GenerateTerrianRight(rightPosition);
    }

    public void LeftTrigger()
    {
        leftPosition = GenerateTerrianLeft(leftPosition);
    }

    private IEnumerator FlatNumberWait()
    {
        yield return new WaitForSeconds(0.05f);
        using (StreamReader reader = new StreamReader(Application.persistentDataPath + "/Game.data"))
        {
            switch (reader.ReadLine())
            {
                case "Flat": flatNumberBottom = 0; flatNumberTop = 42; break;
                case "Easy": flatNumberBottom = 10; flatNumberTop = 32; break;
                case "Medium": flatNumberBottom = 19; flatNumberTop = 25; break;
                case "Hard": flatNumberBottom = 21; flatNumberTop = 23; break;
                case "Hardcore": flatNumberBottom = 22; flatNumberTop = 22; break;
                case "Tutorial": flatNumberBottom = 0; flatNumberTop = 42; StartCoroutine(FindObjectOfType<Tutorial>().StartTutorial()); break;
            }
            reader.Close();
        }
        generateAllowed = true;
    }
}
