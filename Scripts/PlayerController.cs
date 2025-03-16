using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Burst.CompilerServices;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode CW = KeyCode.D, CCW = KeyCode.A, Thrust = KeyCode.Space, Pause = KeyCode.Escape;

    [SerializeField] private float rotationForce = 150, thrustForce = 6500;

    [SerializeField] private Camera cam, landerCam;

    [SerializeField] TMP_Text landerText, fuelText, scoreText;

    [SerializeField] private GameObject gameUI, pauseUI, finishMenu, successText, failedText;

    private int landingAngle = 15, landingVelocity = 2;

    private float previousVelocity = 0, previousVelocityY = 0;

    private bool applyDownward = false, thrusting = false, paused = false, decided = false, thrustAllowed = true, crashed = false;

    [SerializeField] private int fuel = 250;

    private Rigidbody2D playerRB;

    [SerializeField] private ParticleSystem explosion, thrust;

    [SerializeField] private AudioSource thrustAudio, explosionAudio;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        StartFuelWait();
        StartCoroutine(SettingsWait());
        Time.timeScale = 0;
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            playerRB.linearVelocity = new Vector3(10, 0, 0);
            transform.position = new Vector3(0, 150, 0);
        }
        else
        {
            playerRB.linearVelocity = new Vector3(-10, 0, 0);
            transform.position = new Vector3(0, 150, 0);
        }
        cam.transform.position = new Vector3(transform.position.x, transform.position.y/2, -10);
        cam.orthographicSize = (transform.position.y + 2f) / 2f;
    }

    private void Update()
    {
        float angle = transform.eulerAngles.z;
        if (angle > 180)
        {
            angle -= 360;
        }

        if (!crashed)
        {
            landerText.text = "Deg: " + Mathf.Round(angle * 10) / 10 + "\nSpd: " + Mathf.Round(previousVelocity * 10) / 10 + "\nSpd Y: " + Mathf.Round(previousVelocityY * 10) / 10;
            fuelText.text = fuel + " :Fuel";
        } else
        {
            landerText.text = "Deg: >Error<" + "\nSpd: >Error<" + "\nSpd Y: >Error<";
            fuelText.text = ">Error< :Fuel";
        }

        if (Input.GetKeyDown(Pause))
        {
            if (!paused)
            {
                Time.timeScale = 0;
                gameUI.SetActive(false);
                pauseUI.SetActive(true);
                paused = true;
            }
            else
            {
                gameUI.SetActive(true);
                pauseUI.SetActive(false);
                Time.timeScale = 1;
                paused = false;
            }
        }

        //Debugging Control
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Tab) && Input.GetKey(KeyCode.E))
        {
            playerRB.gravityScale = 0;
            playerRB.linearVelocity = new Vector3(0, 0, 0);
        }

        EmitCheck(thrusting);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!decided)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
            if (hit)
            {
                Vector3 target = new Vector3(transform.position.x, (transform.position.y - hit.point.y)/2 + hit.point.y, -10);

                cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, (transform.position.y - target.y) + 2f, Time.deltaTime * (1f + Mathf.Abs(cam.orthographicSize - ((transform.position.y - target.y) + 2f))/10f * Mathf.Abs(playerRB.linearVelocity.y)/2.5f));

                cam.transform.position = Vector3.MoveTowards(cam.transform.position, target, Time.deltaTime * (1f + Mathf.Abs(playerRB.linearVelocity.y)/4f * Mathf.Abs(cam.transform.position.y - (transform.position.y - target.y)) / 8.4375f));
            }
            cam.transform.position = new Vector3(transform.position.x + playerRB.linearVelocity.x / 10 * cam.orthographicSize / 4f, cam.transform.position.y, -10);

            landerCam.transform.position = transform.position + new Vector3(0, 0, -10);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(CW))
        {
            playerRB.AddTorque(-rotationForce, ForceMode2D.Force);
        }
        if (Input.GetKey(CCW))
        {
            playerRB.AddTorque(rotationForce, ForceMode2D.Force);
        }

        if (thrustAllowed)
        {
            if (Input.GetKey(Thrust))
            {
                playerRB.AddRelativeForce(Vector2.up * thrustForce, ForceMode2D.Force);
                thrusting = true;
                var thrustEmission = thrust.emission;
                thrustEmission.enabled = true;
                //StartCoroutine(AudioWait());
                ThrustAudioCheck(true);
            }
            else
            {
                thrusting = false;
                var thrustEmission = thrust.emission;
                thrustEmission.enabled = false;
                ThrustAudioCheck(false);
            }
        } else
        {
            thrusting = false;
            var thrustEmission = thrust.emission;
            thrustEmission.enabled = false;
            ThrustAudioCheck(false);

        }
        previousVelocity = playerRB.linearVelocity.magnitude;
        previousVelocityY = Mathf.Abs(playerRB.linearVelocity.y);

        if (applyDownward)
        {
            playerRB.AddForce(Vector2.down * 1000, ForceMode2D.Force);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Outpost")
        {
            float angle = transform.eulerAngles.z;
            if (angle > 180)
            {
                angle = 360 - angle;
            }
            rotationForce = 0;
            thrustForce = 0;
            if (previousVelocityY < landingVelocity && angle < landingAngle && transform.eulerAngles.z > -landingAngle)
            {
                applyDownward = true;
                thrustAllowed = false;
            }
            else
            {
                if (FindObjectOfType<Tutorial>().inTutorial)
                {
                    playerRB.linearVelocity = new Vector3(0, 0, 0);
                    playerRB.angularVelocity = 0;
                    transform.position = new Vector3(0, 150, 0);
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    cam.transform.position = new Vector3(transform.position.x, transform.position.y / 2, -10);
                    cam.orthographicSize = (transform.position.y + 2f) / 2f;
                    rotationForce = 150;
                    thrustForce = 6500;
                    fuel = 250;
                }
                else
                {
                    applyDownward = false;
                    decided = true;
                    thrustAllowed = false;
                    finishMenu.SetActive(true);
                    successText.SetActive(false);
                    failedText.SetActive(true);
                    crashed = true;
                    scoreText.text = "Score: 0";
                    explosion.transform.position = transform.position;
                    explosion.Play();
                    explosionAudio.Play();
                    Destroy(GetComponent<SpriteRenderer>());
                    Destroy(GetComponent<BoxCollider2D>());
                    playerRB.gravityScale = 0;
                    playerRB.linearVelocity = new Vector2(0, 0);
                    playerRB.angularVelocity = 0;
                }
            }
            if (FindObjectOfType<Tutorial>().inTutorial)
            {
                StartCoroutine(TutorialDecisionWait());
            }
            else
            {
                StartCoroutine(DecisionWait());
            }
        }
    }

    private IEnumerator DecisionWait()
    {
        yield return new WaitForSeconds(2);
        if (!decided)
        {
            bool rayHit = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
            if (hit)
            {
                if (transform.position.y - hit.point.y < 0.65f)
                {
                    rayHit = true;
                }
            }

            float angle = transform.eulerAngles.z;
            if (angle > 180)
            {
                angle = 360 - angle;
            }
            if (previousVelocityY < landingVelocity && angle < landingAngle && transform.eulerAngles.z > -landingAngle && rayHit)
            {
                successText.SetActive(true);
                failedText.SetActive(false);
                scoreText.text = "Score: " + FindObjectOfType<ScoringSystem>().CalculateScore().ToString();
            }
            else
            {

                successText.SetActive(false);
                failedText.SetActive(true);
                scoreText.text = "Score: 0";
            }
            decided = true;
            finishMenu.SetActive(true);
        }
    }

    private IEnumerator TutorialDecisionWait()
    {
        yield return new WaitForSeconds(2);
        if (!decided)
        {
            bool rayHit = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
            if (hit)
            {
                if (transform.position.y - hit.point.y < 0.65f)
                {
                    rayHit = true;
                }
            }

            float angle = transform.eulerAngles.z;
            if (angle > 180)
            {
                angle = 360 - angle;
            }
            if (previousVelocityY < landingVelocity && angle < landingAngle && transform.eulerAngles.z > -landingAngle && rayHit)
            {
                successText.SetActive(true);
                failedText.SetActive(false);
                scoreText.text = "Score: Unscored";
                decided = true;
                finishMenu.SetActive(true);
            }
        }
    }

    private IEnumerator FuelWait()
    {
        yield return new WaitForSeconds(0.25f);
        if (thrusting && thrustForce > 0)
        {
            fuel--;
        }
        if (fuel <= 0)
        {
            fuel = 0;
            thrustForce = 0;
            thrustAllowed = false;
        }
        StartFuelWait();
    }

    private void StartFuelWait()
    {
        StartCoroutine(FuelWait());
    }

    public void ChangePauseState(bool state)
    {
        paused = state;
    }

    public void HotloadKeybinds()
    {
        Settings settings = FindObjectOfType<Settings>();
        settings.LoadKeybinds();
        CW = settings.keybinds[0];
        CCW = settings.keybinds[1];
        Thrust = settings.keybinds[2];
        Pause = settings.keybinds[3];
        thrustAudio.volume = (float)settings.volume/100f;
        explosionAudio.volume = (float)settings.volume/100f;
    }

    private IEnumerator SettingsWait()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        Time.timeScale = 1;
        HotloadKeybinds();
    }

    public int GetFuel()
    {
        return fuel;
    }

    private void EmitCheck(bool pass)
    {
        var thrustEmission = thrust.emission;
        if (pass)
        {
            if (!thrustEmission.enabled)
            {
                thrustEmission.enabled = true;
            }
        } else
        {
            if (thrustEmission.enabled)
            {
                thrustEmission.enabled = false;
            }
        }
    }

    private void ThrustAudioCheck(bool play)
    {
        if (thrustAudio.isPlaying && !play)
        {
            thrustAudio.Stop();
        } else if (!thrustAudio.isPlaying && play)
        {
            thrustAudio.Play();
        }
    }

    public float SetThrustForce(float newForce)
    {
        float temp = thrustForce;
        thrustForce = newForce;
        return temp;
    }
}