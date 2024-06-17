using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public bool canMove = true; 
    public bool canInteract = true; 

    [SerializeField] private int speed = 5;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnMovement(InputValue value)
    {
        if (canMove)
        {
            movement = value.Get<Vector2>();
        }
        else
        {
            movement = Vector2.zero; 
        }

        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("X", movement.x);
            animator.SetFloat("Y", movement.y);

            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        }
    }


    public void StartConversation()
    {
        canMove = false;
        canInteract = false;
    }
    public void EndConversation()
    {
        canMove = true;
        canInteract = true;
    }
}
