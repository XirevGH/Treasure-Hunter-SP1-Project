using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCrabbyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float bounciness = 100f;
    [SerializeField] private int damageGiven = 1;
    [SerializeField] private float giveKnocbackForceH = 200f;
    [SerializeField] private float giveKnockbackForceV = 100f;
    [SerializeField] private BoxCollider2D boxCollider1, boxCollider2;
    private SpriteRenderer rend;
    private bool canMove = true;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!canMove)
            return;

        transform.Translate(new Vector2(moveSpeed, 0) * Time.deltaTime);

        if (moveSpeed > 0)
        {
            rend.flipX = true;
        }
        if (moveSpeed < 0)
        {
            rend.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyBlock"))
        {
            moveSpeed = -moveSpeed;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            moveSpeed = -moveSpeed;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().TakeDamage(damageGiven);

            if (other.transform.position.x > transform.position.x)
            {
                other.gameObject.GetComponent<PlayerMovement>().TakeKnockBack(giveKnocbackForceH, giveKnockbackForceV);
            }
            else
            {
                other.gameObject.GetComponent<PlayerMovement>().TakeKnockBack(-giveKnocbackForceH, giveKnockbackForceV);
            }
        }


    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(other.GetComponent<Rigidbody2D>().velocity.x, 0);
            other.GetComponent<Rigidbody2D>(). AddForce(new Vector2(0, bounciness));
            GetComponent<Animator>().SetTrigger("Hit");
            boxCollider1.enabled = false;
            boxCollider2.enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            canMove = false;
            Destroy(gameObject, 0.4f);
        }
    }
}
