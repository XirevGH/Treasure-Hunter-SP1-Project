using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingChain : MonoBehaviour
{
    private float vertical;
    private bool isChain;
    private bool isClimbing;

    [SerializeField] private float climbSpeed = 10f;
    [SerializeField] private Rigidbody2D rb;

    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        if(isChain && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * climbSpeed);
        }
        else
        {
            rb.gravityScale = 2f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Chain"))
        {
            isChain = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Chain"))
        {
            isChain = false;
            isClimbing = false;
        }
    }
}
