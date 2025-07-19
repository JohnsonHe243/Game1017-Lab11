using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Idling,
    Running,
    Rolling,
    Jumping
}
public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Transform groundDetect;
    [SerializeField] private bool isGrounded; // Just so we can see in Editor.
    [SerializeField] private float moveForce;
    [SerializeField] private float jumpForce;
    public LayerMask groundLayer;
    private float groundCheckWidth = 2f;
    private float groundCheckHeight = 0.25f;
    private Animator an;
    private Rigidbody2D rb;
    private CapsuleCollider2D cc;

    private CharacterState currentState;
    private bool jumpStarted;

    void Start()
    {
        an = GetComponentInChildren<Animator>();
        isGrounded = false; // Always start in air.
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        StartJump();
    }

    void Update()
    {
        if (!jumpStarted)
        {
            GroundedCheck();
        }
        
        switch (currentState)
        {
            case CharacterState.Idling:
                HandleIdlingState(); 
                break;
            case CharacterState.Running:
                HandleRunningState();
                break;
            case CharacterState.Rolling:
                HandleRollingState();
                break;
            case CharacterState.Jumping:
                HandleJumpingState();
                break;
        }
    }

    private void HandleIdlingState()
    {
        // State logic here.
        transform.Translate(new Vector3(-4f * Time.deltaTime, 0f, 0f));

        if (isGrounded && (Input.GetAxis("Horizontal") != 0)) // To Running.
        {
            an.SetBool("isMoving", true);
            currentState = CharacterState.Running;
        }
        else if (isGrounded && Input.GetKeyDown(KeyCode.S)) // To Rolling.
        {
            cc.offset = new Vector2(0.33f, -1f);
            cc.size = new Vector2(2f, 2f);
            Game.Instance.SOMA.PlayLoopedSound("Roll");
            an.SetBool("isRolling", true);
            currentState = CharacterState.Rolling;
        }
        else if (isGrounded && Input.GetButtonDown("Jump")) // To Jumping.
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
            Game.Instance.SOMA.PlaySound("Jump");
            StartJump();
        }
    }

    private void HandleRunningState()
    {
        // State logic here.
        MoveCharacter();
        // Transistions.
        if (isGrounded && (Input.GetAxis("Horizontal") == 0)) // To Idling.
        {
            an.SetBool("isMoving", false);
            currentState = CharacterState.Idling;
        }
        else if (isGrounded && Input.GetKeyDown(KeyCode.S)) // To Rolling.
        {
            cc.offset = new Vector2(0.33f, -1f);
            cc.size = new Vector2(2f, 2f);
            Game.Instance.SOMA.PlayLoopedSound("Roll");
            an.SetBool("isRolling", true);
            currentState = CharacterState.Rolling;
        }
        else if (isGrounded && Input.GetButtonDown("Jump")) // To Jumping.
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
            Game.Instance.SOMA.PlaySound("Jump");
            StartJump();
        }
    }
    private void HandleRollingState()
    {
        // State logic here.
        MoveCharacter();
        // Transistions.
        if (Input.GetKeyUp(KeyCode.S))
        {
            cc.offset = new Vector2(0.33f, -0.25f);
            cc.size = new Vector2(2f, 3.5f);
            Game.Instance.SOMA.StopLoopedSound();
            an.SetBool("isRolling", false);
            currentState = CharacterState.Idling;
        }

    }
    private void HandleJumpingState()
    {
        // Jumping state logic here.
        MoveCharacter();
        // Transitions.
        if (isGrounded) // First frame where isGrounded is true, we transition to idling.
        {
            an.SetBool("isJumping", false);
            currentState = CharacterState.Idling;
        }
    }
    private void MoveCharacter()
    {
        // Horizontal movement.
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveForce * Time.fixedDeltaTime, rb.velocity.y);
    }

    private void GroundedCheck()
    {
        isGrounded = Physics2D.OverlapBox(groundDetect.position, 
            new Vector2(groundCheckWidth, groundCheckHeight), 0f, groundLayer);
        an.SetBool("isJumping", !isGrounded);
    }

    private void StartJump()
    {
        jumpStarted = true;
        isGrounded = false;

        an.SetBool("isJumping", true);
        currentState = CharacterState.Jumping;
        Invoke("ResetJumpStarted", 0.5f);
    }

    private void ResetJumpStarted()
    {
        jumpStarted = false;
    }
}
