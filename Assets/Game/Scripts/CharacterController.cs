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
    public float swipeSpeed = 3f; 
    public float gravity = -9.81f; 
    public float groundDistance = 0.4f; 
    public LayerMask groundMask; 

    private Vector3 velocity;
    private bool isGrounded;
    private Animator animator;
    public Transform groundCheck; 
    private Touch theTouch;
    private Vector2 touchStart, touchEnd;
    private string touchDirection;
    

    void Start()
    {
        charControl = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update(){
        #if UNITY_ANDROID || UNITY_IOS
            HandleMobileControls();  
        #else
            HandleDesktopControls();  
        #endif
        //ensure gravity applies
        HandleMovement(); 
    }
    private void HandleDesktopControls()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = (horizontal * transform.right) + (vertical * transform.forward);
        direction.y = 0; 

        
        if (horizontal != 0 || vertical != 0)
        {
            TriggerRunAnimation();
        }

        charControl.Move(direction * speed * Time.deltaTime);
    }   

    private void HandleMobileControls()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began)
            {
                touchStart = theTouch.position;  // Record touch start position
            }
            else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
            {
                touchEnd = theTouch.position;   // Record touch end position
                float x = touchEnd.x - touchStart.x;
                float y = touchEnd.y - touchStart.y;

                if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                {
                    touchDirection = "tapped";   
                    MoveForward();               
                }
                else if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    touchDirection = x > 0 ? "right" : "left";  
                    if (touchDirection == "right")
                    {
                        MoveRight();    
                    }
                    else 
                    {
                        MoveLeft();   
                    }
                }

                Debug.Log("Mobile Input: " + touchDirection);   // Debug touch direction
            }
        }
    }
      private void MoveForward()
    {
        Vector3 forwardDirection = transform.forward * speed * Time.deltaTime;
        charControl.Move(forwardDirection);  
        TriggerRunAnimation();               
    }

       private void MoveRight()
    {
        Vector3 rightDirection = transform.right * swipeSpeed * Time.deltaTime;
        charControl.Move(rightDirection);    
        TriggerRunAnimation();               
    }

    private void MoveLeft()
    {
        Vector3 leftDirection = -transform.right * swipeSpeed * Time.deltaTime;
        charControl.Move(leftDirection);     
        TriggerRunAnimation();               
    }

    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  
        }

        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        charControl.Move(velocity * Time.deltaTime); 
    }

    private void TriggerRunAnimation()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("StartRun"))
        {
            animator.SetTrigger("StartRun");
        }
    }

}
