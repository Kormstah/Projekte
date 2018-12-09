using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float playerSpeed;
    public GameObject projectilePrefab;
    public GameObject projectilePrefab2;
    public GameObject explosionPrefab;

    // (P03,E03)
    public GameObject shield;
    private Material shieldMaterial;
    private Color shieldFull = new Color(0, 1, 0, 0.3f);
    private Color shieldHalf = new Color(1, 1, 0, 0.3f);
    // (P03,E03) end

    public static int score = 0;
    public static int lives = 3;
    public static Text playerStats;

    public static int killStreak = 0; // (P02,E01)

    public static int missed = 0;

    public float projectileOffset;
    public float projectileOffset2; // (P03,E01)

    private float shipInvisibleTime = 1.5f;
    private float shipMoveOnToScreenSpeed = 5f;
    private float blinkRate = 0.1f;
    private int numberOfTimesToBlink = 10;
    private int blinkCount;

    enum State
    {
        Playing,
        Explosion,
        Invincible
    };
    private State state = State.Playing;

    // Use this for initialization
    void Start()
    {
        playerStats = GameObject.Find("PlayerStats").GetComponent<Text>();
        UpdateStats();

        shieldMaterial = shield.GetComponent<Renderer>().material;
        shieldMaterial.color = shieldFull;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != State.Explosion)
        {
            // Move player depending on input
            float amtToMove = Input.GetAxisRaw("Horizontal") * playerSpeed * Time.deltaTime;

            // Präsenzaufgaben 01, Exercise 02 (P01,E02)
            if (amtToMove < 0) transform.rotation = Quaternion.Euler(new Vector3(0, 30, 0)); // set rotation when moving left
            else if (amtToMove > 0) transform.rotation = Quaternion.Euler(new Vector3(0, -30, 0)); // set rotation when moving right
            else transform.rotation = Quaternion.identity; // reset rotation to (0,0,0) when not moving horizontally
            // (P01,E02) end

            transform.Translate(Vector3.right * amtToMove, Space.World); // Space.World added in P01,E02

            // Präsenzaufgaben 01, Exercise 01 (P01,E01)
            amtToMove = Input.GetAxisRaw("Vertical") * playerSpeed * Time.deltaTime; // get "Vertical" input
            if (transform.position.y > 5 && amtToMove > 0) amtToMove = 0; // Prevent player from going out of the screen (top)
            if (transform.position.y < -3 && amtToMove < 0) amtToMove = 0; // Prevent player from going out of the screen (bottom)
            transform.Translate(Vector3.up * amtToMove); // move player vertically
            // (P01,E01) end

            // Screen Wrap
            if (transform.position.x < -7.4f)
            {
                transform.position = new Vector3(7.4f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 7.4f)
            {
                transform.position = new Vector3(-7.4f, transform.position.y, transform.position.z);
            }

            if (Input.GetKeyDown("space"))
            {
                // Set position
                Vector3 position = new Vector3(transform.position.x, transform.position.y + projectileOffset, transform.position.z);

                // Fire projectile
                Instantiate(projectilePrefab, position, Quaternion.identity);

                //Präsenzaufgaben 02, Exercise 01 (P02,E01)
                if (killStreak > 9)
                {
                    // Präsenzaufgaben 01, Exercise 03 (P01,E03)
                    position = new Vector3(transform.position.x - (0.55f * transform.localScale.x), transform.position.y + projectileOffset2, transform.position.z); // (P03,E01) projectileOffset2
                    Instantiate(projectilePrefab2, position, Quaternion.Euler(0, 0, 10));
                    position = new Vector3(transform.position.x + (0.55f * transform.localScale.x), transform.position.y + projectileOffset2, transform.position.z); // (P03,E01) projectileOffset2
                    Instantiate(projectilePrefab2, position, Quaternion.Euler(0, 0, -10));
                    // (P01,E03) end
                }
                // (P02,E01) end
            }
        }
    }

    public static void UpdateStats()
    {
        playerStats.text = "Score: " + score.ToString() + "\nLives: " + lives.ToString() + "\nMissed: " + missed.ToString();
    }

    void OnTriggerEnter(Collider otherObject)
    {
        if (otherObject.tag == "Enemy" && state == State.Playing)
        {
            // Set a new position and speed for the hit enemy
            Enemy enemy = (Enemy)otherObject.gameObject.GetComponent("Enemy");
            enemy.SetPositionAndSpeed();

            // Präsenzaufgaben 03, Exercise 03 (P03,E03)
            if (shieldMaterial.color == shieldFull && shield.activeInHierarchy)
            {
                shieldMaterial.color = shieldHalf;
                return;
            }
            if (shieldMaterial.color == shieldHalf && shield.activeInHierarchy)
            {
                shieldMaterial.color = shieldFull;
                shield.SetActive(false);
                return;
            }
            // (P03,E03) end

            //Decrease the player's life and make sure it is shown in the UI
            lives--;
            UpdateStats();

            killStreak = 0; // (P02,E01)

            StartCoroutine(DestroyShip()); // has replaced Instantiate (explosion...) in T04
        }
    }

    IEnumerator DestroyShip()
    {
        blinkCount = 0;
        state = State.Explosion;

        // Instantiate the Explosion (was moved from OnTriggerEnter in T04)
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);


        transform.position = new Vector3(0f, -5.5f, transform.position.z);
        yield return new WaitForSeconds(shipInvisibleTime);

        if (lives > 0)
        {
            while (transform.position.y < -2.2f)
            {
                // Move the ship up
                float amtToMove = shipMoveOnToScreenSpeed * Time.deltaTime;
                transform.position = new Vector3(0, transform.position.y + amtToMove, transform.position.z);
                yield return 0;
            }
            state = State.Invincible;
            shield.SetActive(true); // (P03,E03) 
            while (blinkCount < numberOfTimesToBlink)
            {
                gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
                if (gameObject.GetComponent<Renderer>().enabled)
                    blinkCount++;
                yield return new WaitForSeconds(blinkRate);
            }
            state = State.Playing;
        }
        else
            SceneManager.LoadScene("Lose");
    }
}
