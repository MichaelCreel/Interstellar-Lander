using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject player, tutorialBox;

    private Rigidbody2D playerRB;

    [SerializeField] private TMP_Text text;

    public bool inTutorial = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator StartTutorial()
    {
        inTutorial = true;
        yield return new WaitForSeconds(0.1f);
        playerRB.linearVelocity = new Vector3(0, 0, 0);
        player.transform.position = new Vector3(0, 150, 0);
        playerRB.gravityScale = 0;
        tutorialBox.SetActive(true);
        float thrustForce = player.GetComponent<PlayerController>().SetThrustForce(0);
        text.text = "Welcome to the game. The goal of this game is to land on flat surfaces of the randomly generated terrain.\n\nPress SPACE to continue...";
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        Settings settings = FindObjectOfType<Settings>();
        text.text = "Your controls can be changed in settings on the main menu, or by pressing " +
            settings.keybinds[3] + " and choosing settings.\n\nPress SPACE to continue...";
        while (!Input.GetKeyUp(KeyCode.Space))
        {
            yield return null;
        }
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        text.text = "To rotate the lander clockwise, press " + settings.keybinds[0];
        while (!Input.GetKeyUp(settings.keybinds[0]))
        {
            yield return null;
        }
        text.text = "To rotate the lander counter-clockwise, press " + settings.keybinds[1];
        while (!Input.GetKeyUp(settings.keybinds[1]))
        {
            yield return null;
        }
        text.text = "To use the lander's thrust and move the lander, press " + settings.keybinds[2];
        while (!Input.GetKeyUp(settings.keybinds[2]))
        {
            yield return null;
        }
        text.text = "In to top left, you can see what direction your lander is facing. This helps at high altitudes when your lander is hard to see or when the camera hasn't caught up to the lander.\n\nPress SPACE to continue...";
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        text.text = "When the lander has a horizontal velocity, the camera will account for the velocity by moving to the left or right. If the lander is moving left, it will be on the right side of the screen. If the lander is moving right, it will be on the left side.\n\n Press SPACE to continue...";
        while (!Input.GetKeyUp(KeyCode.Space))
        {
            yield return null;
        }
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        text.text = "To have a successful landing, your lander must be angled less than 15 degrees in either direction and have a downward velocity less than 2 m/s (Spd and SpdY are measured in m/s). In normal gamemodes, you will have a speed of 10 m/s to either the left or right, but you start with no velocity for the tutorial.\n\nPress SPACE to continue...";
        while (!Input.GetKeyUp(KeyCode.Space))
        {
            yield return null;
        }
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        text.text = "When looking for spots to land, look for buildings on the ground. Landing in front of these buildings increases your score.\n\nPress SPACE to continue...";
        while (!Input.GetKeyUp(KeyCode.Space))
        {
            yield return null;
        }
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        text.text = "Now you will try landing. The lander will be reset every time the landing is failed.\n\nPress SPACE when ready...";
        while (!Input.GetKeyUp(KeyCode.Space))
        {
            yield return null;
        }
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        float unused = player.GetComponent<PlayerController>().SetThrustForce(thrustForce);
        playerRB.gravityScale = 1;
        tutorialBox.SetActive(false);   
    }
}
