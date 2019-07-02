using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //configuration parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] int health = 200;

    [Header("Explosion")]
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip deathSFX;
    [Range(0, 1f)] [SerializeField] float playerDeathVolume = 0.5f;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] AudioClip laserSFX;
    [Range(0, 1f)] [SerializeField] float playerLaserVolume = 0.5f;

    Coroutine firingCorouting;

    float xMin, xMax, yMin, yMax;
    float playerWidth;
    float playerHeight;
    SpriteRenderer playerSpriteRenderer;

    void Start()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        playerWidth = playerSpriteRenderer.bounds.size.x /2;
        playerHeight = playerSpriteRenderer.bounds.size.y /2;
        SetUpMoveBoundries();
    }

    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCorouting = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCorouting);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, playerLaserVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
       
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundries()
        {
            Camera gameCamera = Camera.main;
            xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + playerWidth;
            xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - playerWidth;
            yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + playerHeight;
            yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - playerHeight;
        }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer demageDealer = other.gameObject.GetComponent<DamageDealer>();
        if(!demageDealer) { return; }
        ProcessHit(demageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Death();
        }
    }


    private void Death()
    {
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, playerDeathVolume);

        FindObjectOfType<Level>().LoadGameOver();
    }

    public int GetHealth()
    {
        return health;
    }
}
