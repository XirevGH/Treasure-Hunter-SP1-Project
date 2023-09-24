using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossFierceTooth : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float bounciness = 100f;
    [SerializeField] private int damageGiven = 2;
    [SerializeField] private int damageTaken = 1;
    [SerializeField] private float giveKnocbackForceH = 400f;
    [SerializeField] private float giveKnockbackForceV = 200f;
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private BoxCollider2D boxCollider1, boxCollider2;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private int levelToLoad;

    [SerializeField] private GameObject completedTheGame;
    [SerializeField] private GameObject lostTheGame;

    private SpriteRenderer rend;
    private Animator anim;
    private Rigidbody2D rb;
    private bool canMove;
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

    private void Update()
    {
        canMove = FindAnyObjectByType<PlayerProperties>().GetComponent<PlayerProperties>().bossCanMove;
    }

    void FixedUpdate()
    {
        if (!canMove)
            return;

        if (currentHealth > 0)
        {
            anim.SetTrigger("BossActivated");

            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

            if (transform.position.x < playerTransform.position.x)
            {
                rend.flipX = true;
            }
            if (transform.position.x > playerTransform.position.x)
            {
                rend.flipX = false;
            }
        }
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
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
            Invoke("HeavenlyFlight", 2f);
            int RandomValue = Random.Range(0, hitSounds.Length);
            audioSource.PlayOneShot(hitSounds[RandomValue], 0.5f);
            completedTheGame.SetActive(true);
            Invoke("displayText", 3f);
            Invoke("LoadNextLevel", 5f);
            Destroy(gameObject, 10f);
        }
    }

    private void HeavenlyFlight()
    {
        rb.velocity = new Vector2(0, 2);
    }
    private void displayText()
    {
        lostTheGame.SetActive(true);
    }
    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
