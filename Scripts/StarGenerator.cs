using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGenerator : MonoBehaviour
{
    [SerializeField] private GameObject star;

    [SerializeField] private Camera cam;

    [SerializeField] private int starcount = 100;

    private List<GameObject> stars = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GenerateStars();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateStars()
    {
        for (int i = 0; i < starcount; i++)
        {
            GameObject newStar = Instantiate(star, new Vector3(Random.Range(cam.ViewportToWorldPoint(new Vector3(0, 1, 0)).x, cam.ViewportToWorldPoint(new Vector3(1, 1, 0)).x), Random.Range(cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).y, cam.ViewportToWorldPoint(new Vector3(1, 1, 0)).y), 10), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            newStar.transform.parent = transform;
            float size = Random.Range(0.05f, 0.1f);
            newStar.transform.localScale = new Vector3(size, size, size);
            stars.Add(newStar);
        }

        foreach (var star in stars)
        {
            if (star.transform.position.x < cam.ViewportToWorldPoint(new Vector3(0, 1, 0)).x)
            {
                stars.Remove(star);
                Destroy(star);
            }
            if (star.transform.position.x > cam.ViewportToWorldPoint(new Vector3(1, 1, 0)).x)
            {
                stars.Remove(star);
                Destroy(star);
            }
        }
    }
}
