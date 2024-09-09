using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 120f;
    public float runSpeed = 170;
    public float initialJumpForce = 10f;
    public float sustainedJumpForce = 5f;
    public float maxJumpTime = 0.5f;
    public float jumpTimeCounter = 0;
    public bool isJumping;
    public bool isGrounded;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        isGrounded = true;
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        PlayerMovement();
    }

    void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }

    void PlayerMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        playerRb.AddForce(transform.forward * speed * verticalInput * Time.deltaTime, ForceMode.Impulse);
        playerRb.AddForce(transform.right * speed * horizontalInput * Time.deltaTime, ForceMode.Impulse);

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : speed;

        // Calculate movement direction
        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        // Apply velocity change for immediate movement
        Vector3 targetVelocity = moveDirection * currentSpeed;
        playerRb.velocity = new Vector3(targetVelocity.x, playerRb.velocity.y, targetVelocity.z);


        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            StartJump();
        }
        
        if(Input.GetKey(KeyCode.Space) && isJumping)
        {
            ContinueJump();
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            EndJump();
        }
    }

    void StartJump()
    {
        isJumping = true;
        playerRb.AddForce(Vector3.up * initialJumpForce, ForceMode.Impulse);
        jumpTimeCounter = 0;
    }

    void ContinueJump()
    {
        jumpTimeCounter += Time.deltaTime;
        if(jumpTimeCounter < maxJumpTime)
        {
            playerRb.AddForce(Vector3.up * sustainedJumpForce, ForceMode.Force);
        }
        else
        {
            EndJump();
        }
    }

    void EndJump()
    {
        isJumping = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    
}
