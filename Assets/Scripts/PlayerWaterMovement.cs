using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaterMovement : MonoBehaviour
{
    private float vertical;
    private float horizontal;
    private bool isSwimming;

    [SerializeField] private float SwimSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
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
            isSwimming = true;
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
