using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaterMovement : MonoBehaviour
{
    private float vertical;
    private float horizontal;
    private bool isSwimming;

    private float futureTime;
    private float currentTime;
    private float intervalTime = 0.4f;

    [SerializeField] private float SwimSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioClip waterSplash;
    [SerializeField] private AudioClip swimmingSound;

    private AudioSource audioSource;
    private Animator anim;

    private void Start()
    {
        futureTime = Time.time + intervalTime;
        currentTime = Time.time;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        anim.SetBool("IsSwimming", isSwimming);
        anim.SetFloat("DirectionHorizontal", horizontal);
        anim.SetFloat("DirectionVertical", vertical);
    }

    private void FixedUpdate()
    {
        if (isSwimming)
        {
            rb.velocity = new Vector2(horizontal * SwimSpeed, vertical * SwimSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            audioSource.PlayOneShot(waterSplash, 0.2f);
            isSwimming = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            currentTime = Time.time;
            if (currentTime >= futureTime)
            {
                futureTime = Time.time + intervalTime;
                audioSource.PlayOneShot(swimmingSound, 1f);
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isSwimming = false;
        }
    }
}
