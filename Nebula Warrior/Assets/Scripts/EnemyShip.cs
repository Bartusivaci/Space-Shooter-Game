using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] float health = 100;
    [SerializeField] GameObject deathVFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0,1)]float deathSoundVolume = 0.75f;
    [SerializeField] int scoreValue = 2000;

    [Header("Laser")]
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject enemyLaserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] AudioClip laserSFX;
    [SerializeField] [Range(0, 1)] float laserVolume = 0.5f;

    float shotTimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        shotTimeCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotTimeCounter -= Time.deltaTime;
        if(shotTimeCounter <= 0f)
        {
            Fire();
            shotTimeCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(enemyLaserPrefab, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,-(projectileSpeed));
        AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, 1f);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
    }
}
