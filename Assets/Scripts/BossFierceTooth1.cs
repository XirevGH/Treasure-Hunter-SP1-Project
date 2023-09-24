using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossFierceTooth1 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float bounciness = 100f;
    [SerializeField] private int damageGiven = 2;
    [SerializeField] private int damageTaken = 1;
    [SerializeField] private float giveKnocbackForceH = 400f;
    [SerializeField] private float giveKnockbackForceV = 200f;
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private BoxCollider2D boxCollider1, boxCollider2;
    [SerializeField] private AudioClip[] hitSounds;
    private SpriteRenderer rend;
    private Animator anim;
    private Rigidbody2D rb;
    private bool canMove = true;
    private bool hasBoostedSpeed = false;
    private bool isRunning = false;
    private int currentHealth;

    private AudioSource audioSource;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = startingHealth;
    }

    void FixedUpdate()
    {
        if (!canMove || FindAnyObjectByType<Drawbridge>().GetComponent<Drawbridge>().bossCanMove1 == false)
            return;

        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, playerTransform.position) > 5f && hasBoostedSpeed == false)
        {
            moveSpeed = moveSpeed * 5;
            hasBoostedSpeed = true;
            isRunning = true;

        }
        if(Vector2.Distance(transform.position, playerTransform.position) < 0.5 && hasBoostedSpeed == true)
        {
            moveSpeed = moveSpeed / 5;
            hasBoostedSpeed = false;
            isRunning = false;
        }

        if (moveSpeed > 0)
        {
            rend.flipX = true;
        }
        if (moveSpeed < 0)
        {
            rend.flipX = false;
        }

        anim.SetBool("isRunning", isRunning);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerProperties>().TakeDamage(damageGiven);

            if (other.transform.position.x > transform.position.x)
            {
                other.gameObject.GetComponent<PlayerProperties>().TakeKnockBack(giveKnocbackForceH, giveKnockbackForceV);
            }
            else
            {
                other.gameObject.GetComponent<PlayerProperties>().TakeKnockBack(-giveKnocbackForceH, giveKnockbackForceV);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && currentHealth > 0)
        {
            currentHealth -= damageTaken;
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(other.GetComponent<Rigidbody2D>().velocity.x, 0);
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, bounciness));
            anim.SetTrigger("Hit");
            int RandomValue = Random.Range(0, hitSounds.Length);
            audioSource.PlayOneShot(hitSounds[RandomValue], 0.5f);
        }

        if (other.CompareTag("Player") && currentHealth <= 0)
        {
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(other.GetComponent<Rigidbody2D>().velocity.x, 0);
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, bounciness));
            anim.SetTrigger("Defeated");
            boxCollider1.enabled = false;
            boxCollider2.enabled = false;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            Invoke("EscapeFlight", 2);
            canMove = false;
            int RandomValue = Random.Range(0, hitSounds.Length);
            audioSource.PlayOneShot(hitSounds[RandomValue], 0.5f);
            Destroy(gameObject, 8f);
        }
    }

    private void EscapeFlight()
    {
        rb.velocity = new Vector2(0, 2);
    }
}
