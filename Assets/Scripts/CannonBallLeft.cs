using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CannonBallLeft : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = transform.right * -speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CannonBallDestroyer"))
        {
            Destroy(gameObject);
        }
    }
}
