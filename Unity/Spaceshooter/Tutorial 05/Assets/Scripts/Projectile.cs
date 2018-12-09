using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour 
{
    public float projectileSpeed;
    public GameObject explosionPrefab;

    private Enemy enemy;

	// Use this for initialization
	void Start () 
    {
        enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        float amtToMove = projectileSpeed * Time.deltaTime;
        transform.Translate(Vector3.up * amtToMove);

        if (transform.position.y > 6.4f)
        {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter(Collider otherObject)
    {
        Debug.Log("We hit: " + otherObject.name);
        if (otherObject.tag == "Enemy")
        {
            Player.score += 100;
            Player.UpdateStats();
            if (Player.score >= 2000)
                SceneManager.LoadScene("Win");

            Player.killStreak++; // (P02,E01)
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            enemy.minSpeed += 0.5f;
            enemy.maxSpeed += 1f;
            enemy.SetPositionAndSpeed();
            // Destroy projectile
            Destroy(gameObject);
        }
    }
}
