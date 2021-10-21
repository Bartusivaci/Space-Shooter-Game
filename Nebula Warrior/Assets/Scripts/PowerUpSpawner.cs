using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{

    [SerializeField] GameObject healOrb;
    [SerializeField] float minTime = 15f;
    [SerializeField] float maxTime = 30f;

    float randomTime;

    // Start is called before the first frame update
    void Start()
    {
        SetRandomTime();
    }

    private void SetRandomTime()
    {
        randomTime = UnityEngine.Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        randomTime -= Time.deltaTime;
        if(randomTime <= 0f)
        {
            Spawn();
            SetRandomTime();
        }
    }

    private void Spawn()
    {
        float spawnY = UnityEngine.Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        float spawnX = UnityEngine.Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
        Vector2 spawnPos = new Vector2(spawnX, spawnY);
        Instantiate(healOrb, spawnPos, Quaternion.identity);
    }
}
