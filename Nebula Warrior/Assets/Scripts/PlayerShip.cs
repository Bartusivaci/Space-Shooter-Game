using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float shipSpeed = 12f;
    [SerializeField] float health = 1000;
    [SerializeField] GameObject deathVFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 1f;

    [Header("Player Laser")]
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] GameObject playerLaserPrefab;
    [SerializeField] AudioClip laserSFX;
    [SerializeField] [Range(0,1)] float laserVolume = 0.25f;

    float xMin;
    float xMax; 

    float yMin;
    float yMax;

    Coroutine firingCoroutine;

    bool isFiring = false;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        int temp = damageDealer.GetDamage();
        if(temp < 0)
        {
            if(health - temp > 1000)
            {
                health += 1000 - health;
            }
            else
            {
                health -= damageDealer.GetDamage();
            }
        }
        else
        {
            health -= damageDealer.GetDamage();
        }
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, 1f);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSoundVolume);
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isFiring)
            {
                isFiring = true;
                firingCoroutine = StartCoroutine(FireContinuously());
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
            isFiring = false;
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(playerLaserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        var deltaPosX = Input.GetAxis("Horizontal") * Time.deltaTime * shipSpeed;
        var deltaPosY = Input.GetAxis("Vertical") * Time.deltaTime * shipSpeed;

        var newPosX = Mathf.Clamp(transform.position.x + deltaPosX, xMin, xMax);
        var newPosY = Mathf.Clamp(transform.position.y + deltaPosY, yMin, yMax);
        transform.position = new Vector2(newPosX, newPosY);
    }

    public float GetHealth()
    {
        return health;
    }
}
