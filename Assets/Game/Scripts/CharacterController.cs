using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    private CharacterController charControl;

    public float speed = 5f;
    public float gravity = -9.81f; 
    public float groundDistance = 0.4f; 
    public LayerMask groundMask; 

    private Vector3 velocity;
    private bool isGrounded;
    private Animator animator;

    public Transform groundCheck; 
    
    void Start()
    {
        charControl = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = (horizontal * transform.right) + (vertical * transform.forward);
        direction.y = 0; 

        charControl.Move(direction * speed * Time.deltaTime);

        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        charControl.Move(velocity * Time.deltaTime);

        TriggerAnimation(horizontal, vertical);
    }
        void TriggerAnimation(float horizontal, float vertical)
    {
        if (horizontal != 0 || vertical != 0)
        {
    
            animator.SetTrigger("StartRun");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }
}
