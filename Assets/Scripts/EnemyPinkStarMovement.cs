using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPinkStarMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private int damageGiven = 1;
    [SerializeField] private float giveKnocbackForceH = 200f;
    [SerializeField] private float giveKnockbackForceV = 100f;
    private SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        transform.Translate(new Vector2(moveSpeed, 0) * Time.deltaTime);

        if (moveSpeed > 0)
        {
            rend.flipX = true;
        }
        if(moveSpeed < 0)
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

            if(other.transform.position.x > transform.position.x)
            {
                other.gameObject.GetComponent<PlayerMovement>().TakeKnockBack(giveKnocbackForceH, giveKnockbackForceV);
            }
            else
            {
                other.gameObject.GetComponent<PlayerMovement>().TakeKnockBack(-giveKnocbackForceH, giveKnockbackForceV);
            }
        }
    }
}
