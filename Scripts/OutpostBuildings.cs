using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutpostBuildings : MonoBehaviour
{
    public bool colliding = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CollisionCheckWait());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        } else
        {
            FindObjectOfType<ScoringSystem>().OutpostCheck(gameObject, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FindObjectOfType<ScoringSystem>().OutpostCheck(gameObject, false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        } else
        {
            FindObjectOfType<ScoringSystem>().OutpostCheck(gameObject, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        FindObjectOfType<ScoringSystem>().OutpostCheck(gameObject, false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator CollisionCheckWait()
    {
        yield return new WaitForSeconds(0.25f);
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
