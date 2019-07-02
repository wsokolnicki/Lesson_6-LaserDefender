using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
 
    [Header("Enemy info")]
    [SerializeField] float health = 100;
    [SerializeField] AudioClip deathSFX;
    [Range(0, 1f)] [SerializeField] float enemyDeathVolume = 0.5f;
    [SerializeField] int scoreValue = 120;

    [Header("Projectile info")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] AudioClip laserSFX;
    [Range(0, 1f)] [SerializeField] float enemyLaserVolume = 0.5f;

    [Header("Game objects")]
    [SerializeField] GameObject enemyLaserPrefab;
    [SerializeField] GameObject explosionPrefab;

	// Use this for initialization
	void Start ()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
	}
	
	// Update is called once per frame
	void Update ()
    {
        CountDownAndShoot();
	}

    private void CountDownAndShoot()
    {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                Fire();
                shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
     }

    private void Fire()
    {
        GameObject enemyLaser = Instantiate(enemyLaserPrefab, transform.position, Quaternion.identity) as GameObject;
        enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, enemyLaserVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer demageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!demageDealer) { return; }
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
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, 1f);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, enemyDeathVolume);
    }
}
