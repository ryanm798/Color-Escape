using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;
    public bool allowCrouch = true;
    
    public float climbSpeed = 40f;
    bool onLadder = false;
    float verticalMove = 0f;

    Rigidbody2D rb2d;
    float defaultGravityScale;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    bool movementEnabled = true;

    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb2d.gravityScale;
    }

    void Update()
    {
        if (movementEnabled)
        {
            UpdateHorizontalMove(Input.GetAxisRaw("Horizontal") * runSpeed);
            if (Input.GetButtonUp("Horizontal") && controller.IsGrounded())
            {
                StopHorizontal();
            }

            if (!onLadder && Input.GetButtonDown("Jump"))
                jump = true;
            
            verticalMove = 0f;
            if (onLadder && Input.GetButton("Jump"))
                verticalMove = climbSpeed;
            else if (onLadder && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
                verticalMove = -1 * climbSpeed;

            if (Input.GetButtonDown("Crouch"))
            {
                crouch = true;
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                crouch = false;
            }
        }
        else
        {
            UpdateHorizontalMove(0f);
        }

        if (!controller.IsGrounded())
        {
            animator.SetBool("IsJumping", true);
            if (rb2d.velocity.y > 0)
            {
                animator.SetBool("IsFalling", false);
            }
            else if (rb2d.velocity.y < 0)
            {
                animator.SetBool("IsFalling", true);
            }
        }
    }

    private void UpdateHorizontalMove(float value)
    {
        horizontalMove = value;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", false);
        StopHorizontal();
    }

    // FixedUpdate called at fixed intervals (independent of frame rate, best for physics)
    void FixedUpdate()
    {
        // Move the character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch && allowCrouch, jump);
        jump = false;

        if (onLadder)
        {
            controller.ClimbLadder(verticalMove * Time.fixedDeltaTime);
        }
    }

    public void OnLadder(bool on)
    {
        onLadder = on;
        if (on)
            rb2d.gravityScale = 0;
        else
            rb2d.gravityScale = defaultGravityScale;
    }

    public void StopHorizontal()
    {
        if (horizontalMove == 0)
            rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
    }

    public void DisableMovement()
    {
        movementEnabled = false;
        crouch = false;
    }

    public void EnableMovement()
    {
        movementEnabled = true;
    }

    public void TryJump()
    {
        jump = true;
    }
}
