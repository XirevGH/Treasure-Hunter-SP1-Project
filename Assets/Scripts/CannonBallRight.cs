using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallRight : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CannonBallDestroyer"))
        {
            Destroy(gameObject);
        }
    }
}
