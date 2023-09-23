using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject cannonBall;
    private float startTimeBetweenShots;

    private float timeBetweenShots;

    void Start()
    {
        startTimeBetweenShots = Random.Range(3f, 5f);
        timeBetweenShots = Random.Range(3f, 5f);
    }

    void Update()
    {
        if(timeBetweenShots <= 0)
        {
            Instantiate(cannonBall, firePoint.position, firePoint.rotation);
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }
}
