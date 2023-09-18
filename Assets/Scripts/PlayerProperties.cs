using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class PlayerProperties : MonoBehaviour
{
    [SerializeField] private float climbSpeed = 10f;
    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Transform leftFoot, rightFoot, leftHand, rightHand, leftSwordWall, rightSwordWall;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsChain;
    [SerializeField] private AudioClip coinPickup, healthPickup;
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private GameObject coinParticles, jumpParticles, keyParticles;

    //WallJump
    private bool wallCanJumpLeft;
    private bool wallCanJumpRight;
    public bool isWallSliding;
    [SerializeField] private float wallSlideSpeed = 0.5f;
    [SerializeField] private float wallHJumpForce = 200f;
    [SerializeField] private float wallVJumpForce = 200f;
    [SerializeField] private LayerMask whatIsWall;

    //Canvas
    [SerializeField] private UnityEngine.UI.Slider healthSlider;
    [SerializeField] private UnityEngine.UI.Image healthBarColor;
    [SerializeField] private Color greenHealth, redHealth;
    [SerializeField] private TMP_Text goldCoinsText;




    private bool isClimbing;

    private float verticalValue;
    private float horizontalValue;
    public bool isGrounded;
    public bool isOnChain;
    private bool canMove;
    private float rayDistance = 0.25f;
    public int startingHealth = 5;
    public int currentHealth = 0;
    private int goldCoinsCollected = 0;
    public int keysCollected = 0;

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private AudioSource audioSource;

    void Start()
    {
        canMove = true;
        currentHealth = startingHealth;
        goldCoinsText.text = "" + goldCoinsCollected;

        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        horizontalValue = Input.GetAxis("Horizontal");

        if (horizontalValue < 0)
        {
            FlipSprite(true);
        }

        if (horizontalValue > 0)
        {
            FlipSprite(false);
        }

        OnWallDoNotFlipSprite();

        if (Input.GetButtonDown("Jump") && CheckIfGrounded() == true)
        {
            Jump();
        }

        if(Input.GetButtonDown("Jump") && CheckIfOnChain() && verticalValue == 0)
        {
            rigidBody.gravityScale = 2f;
            isClimbing = false;
            Jump();

        }
        if(Input.GetButtonDown("Jump") && CheckIfOnWall() && horizontalValue < 0 && wallCanJumpLeft == true)
        {
            WallJumpLeft();
        }

        if (Input.GetButtonDown("Jump") && CheckIfOnWall() && horizontalValue > 0 && wallCanJumpRight == true)
        {
            WallJumpRight();
        }

        anim.SetFloat("MoveSpeed", Mathf.Abs(rigidBody.velocity.x));
        anim.SetFloat("VerticalSpeed", rigidBody.velocity.y);
        anim.SetBool("IsGrounded", CheckIfGrounded());
        anim.SetBool("IsOnChain", CheckIfOnChain());
        anim.SetBool("IsOnWall", CheckIfOnWall());
    }

    private void FixedUpdate()
    {
        if(!canMove)
        {
            return;
        }

        rigidBody.velocity = new Vector2(horizontalValue * moveSpeed * Time.deltaTime, rigidBody.velocity.y);

        verticalValue = Input.GetAxis("Vertical");

        if (CheckIfOnWall())
        {
            rigidBody.gravityScale = wallSlideSpeed;
            rigidBody.velocity = new Vector2(0, -rigidBody.gravityScale);
            rigidBody.freezeRotation = true;
        }

        if (CheckIfOnChain())
        {
            rigidBody.gravityScale = 0f;
            rigidBody.velocity = new Vector2(0, verticalValue * climbSpeed);
        }
        else
        {
            rigidBody.gravityScale = 2f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GoldCoin"))
        {
            Destroy(other.gameObject);
            goldCoinsCollected++;
            goldCoinsText.text = "" + goldCoinsCollected;
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(coinPickup, 0.25f);
            Instantiate(coinParticles, other.transform.position, Quaternion.identity);
        }

        if (other.CompareTag("HealthPotion"))
        {
            RestoreHealth(other.gameObject);
        }

        if (other.CompareTag("Key"))
        {
            Destroy(other.gameObject);
            keysCollected++;
            Instantiate(keyParticles, other.transform.position, Quaternion.identity);
        }
    }

    private void FlipSprite(bool direction)
    {
        spriteRenderer.flipX = direction;
    }

    private void Jump()
    {
        rigidBody.AddForce(new Vector2(0, jumpForce));
        int RandomValue = Random.Range(0, jumpSounds.Length);
        audioSource.PlayOneShot(jumpSounds[RandomValue], 0.25f);
        Instantiate(jumpParticles, transform.position, jumpParticles.transform.localRotation);
    }

    private void ChainJump()
    {
        if (Input.GetButton("Horizontal"))
        {
            isClimbing = false;
            rigidBody.AddForce(new Vector2(0, jumpForce));
        }

    }
    private void WallJumpRight()
    {
        rigidBody.AddForce(new Vector2(wallHJumpForce, wallVJumpForce));
    }

    private void WallJumpLeft()
    {
        rigidBody.AddForce(new Vector2(-wallHJumpForce, wallVJumpForce));
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    public void TakeKnockBack(float horizontalknockbackForce, float verticalKnockbackForce)
    {
        canMove = false;
        rigidBody.AddForce(new Vector2(horizontalknockbackForce, verticalKnockbackForce));
        Invoke("CanMoveAgain", 0.25f);
    }

    private void CanMoveAgain()
    {
        canMove = true;
    }

    private void Respawn()
    {
        currentHealth = startingHealth;
        UpdateHealthBar();
        transform.position = spawnPosition.position;
        rigidBody.velocity = Vector2.zero;
    }

    private void RestoreHealth(GameObject healthPotionPickup)
    {
        if (currentHealth >= startingHealth)
        {
            return;
        }
        else
        {
            int healthToRestore = healthPotionPickup.GetComponent<HealthPotionPickup>().healthAmount;
            currentHealth += healthToRestore;
            UpdateHealthBar();
            Destroy(healthPotionPickup);
            audioSource.PlayOneShot(healthPickup, 0.25f);

            if (currentHealth >= startingHealth)
            {
                currentHealth = startingHealth;
            }
        }
    }

    public void UpdateHealthBar()
    {
        healthSlider.value = currentHealth;

        if(currentHealth >= 2)
        {
            healthBarColor.color = greenHealth;
        }
        else
        {
            healthBarColor.color = redHealth;
        }
    }

    private bool CheckIfGrounded()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(leftFoot.position, Vector2.down, rayDistance, whatIsGround);
        RaycastHit2D rightHit = Physics2D.Raycast(rightFoot.position, Vector2.down, rayDistance, whatIsGround);

        if (leftHit.collider != null && leftHit.collider.CompareTag("Ground") || rightHit.collider != null && rightHit.collider.CompareTag("Ground"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool CheckIfOnChain()
    {
       if (CheckIfChain() && Mathf.Abs(verticalValue) > 0f)
        {
            isClimbing = true;
            return true;
        }
        else if (CheckIfChain() && isClimbing)
        {
            return true;
        }
        else if (!CheckIfChain() && isClimbing)
        {
            Input.ResetInputAxes();
            isClimbing = false;
            return false;
        }
        else 
        {
            isClimbing = false;
            return false; 
        }
    }
    private bool CheckIfChain()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(leftHand.position, Vector2.right, rayDistance, whatIsChain);
        RaycastHit2D rightHit = Physics2D.Raycast(rightHand.position, Vector2.left, rayDistance, whatIsChain);
        if (leftHit.collider != null && leftHit.collider.CompareTag("Chain") && rightHit.collider != null && rightHit.collider.CompareTag("Chain"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckIfOnWall()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(leftSwordWall.position, Vector2.left, rayDistance, whatIsWall);
        RaycastHit2D rightHit = Physics2D.Raycast(rightSwordWall.position, Vector2.right, rayDistance, whatIsWall);

        if (leftHit.collider != null && leftHit.collider.CompareTag("Wall") || rightHit.collider !=null && rightHit.collider.CompareTag("Wall"))
        {
            isWallSliding = true;
            return true;
        }
        else
        {
            isWallSliding = false;
            return false;
        }
    }

    private void OnWallDoNotFlipSprite()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(leftSwordWall.position, Vector2.left, rayDistance, whatIsWall);
        RaycastHit2D rightHit = Physics2D.Raycast(rightSwordWall.position, Vector2.right, rayDistance, whatIsWall);

        if (leftHit.collider != null && leftHit.collider.CompareTag("Wall"))
        {
            spriteRenderer.flipX = true;
            wallCanJumpRight = true;
            wallCanJumpLeft = false;
        }
        if (rightHit.collider != null && rightHit.collider.CompareTag("Wall"))
        {
            spriteRenderer.flipX = false;
            wallCanJumpLeft = true;
            wallCanJumpRight = false;
        }
    }
}
