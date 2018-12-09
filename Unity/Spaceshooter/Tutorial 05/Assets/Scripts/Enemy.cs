using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    #region Fields
    public float currentSpeed;
    public float minSpeed;
    public float maxSpeed;
    private float x, y, z;

    private float MinRotateSpeed = 60f;
    private float MaxRotateSpeed = 120f;
    private float MinScale = 0.8f;
    private float MaxScale = 2f;
    private float currentRotationSpeed;
    private float currentScaleX;
    private float currentScaleY;
    private float currentScaleZ;
    #endregion

    // Use this for initialization
    void Start()
    {
        // Set starting position and speed
        SetPositionAndSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        float rotationSpeed = currentRotationSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(-1, 0, 0) * rotationSpeed);

        // Move Enemy
        float amtToMove = currentSpeed * Time.deltaTime;
        transform.Translate(Vector3.down * amtToMove, Space.World);  // Note: adding Space.World in Tutorial 05 makes the solution of (P01,E04) invalid. How could this be done differently?

        // Set new position and speed when enemy has reached the bottom of the screen
        if (transform.position.y <= -5)
        {
            SetPositionAndSpeed();
            Player.missed++;
            Player.UpdateStats();
        }
    }

    public void SetPositionAndSpeed()
    {
        // Set new speed
        currentSpeed = Random.Range(minSpeed, maxSpeed);

        // Set new Rotation Speed and Scale
        currentRotationSpeed = Random.Range(MinRotateSpeed, MaxRotateSpeed);
        currentScaleX = Random.Range(MinScale, MaxScale);
        currentScaleY = Random.Range(MinScale, MaxScale);
        currentScaleZ = Random.Range(MinScale, MaxScale);

        // Set new position
        x = Random.Range(-6f, 6f);
        y = 7f;
        z = 0f;
        transform.position = new Vector3(x, y, z);

        // Präsenzaufgaben 01, Exercise 04 (P01,E04)
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(-30f, 30f));
        // (P01,E04) end

        transform.localScale = new Vector3(currentScaleX, currentScaleY, currentScaleZ);
    }
}
