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
    private Touch theTouch;
    private Vector2 touchStart, touchEnd;
    private string touchDirection;
    private bool isTouching = false;
    

    void Start()
    {
        charControl = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update(){
        #if UNITY_ANDROID || UNITY_IOS
            HandleMobileControls(); 
        if (isTouching){
            MoveForward();
        }
        #else
            HandleDesktopControls();  
        #endif
        //ensure gravity applies
        HandleVerticalMovement(); 
    }
    private void HandleDesktopControls()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = (horizontal * transform.right) + (vertical * transform.forward);
        direction.y = 0; 

        TriggerRunAnimation(horizontal, vertical);

        charControl.Move(direction * speed * Time.deltaTime);
    }   

    private void HandleMobileControls()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began)
            {
                isTouching = true; 
                touchStart = theTouch.position;  
            }
            else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Stationary)
            {
                
                touchEnd = theTouch.position;  

                float x = touchEnd.x - touchStart.x;
                float y = touchEnd.y - touchStart.y;

         
                if (Mathf.Abs(x) > Mathf.Abs(y))  // Horizontal swipe
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
                    TriggerRunAnimation(1f, 0f);  
                }
                else  
                {
                    touchDirection = "tapped"; //move it forward when tapped 
                    MoveForward();
                    TriggerRunAnimation(1f, 0f);  
                }
            }
            else if (theTouch.phase == TouchPhase.Ended || theTouch.phase == TouchPhase.Canceled)
            {
        
                isTouching = false;
                TriggerRunAnimation(0f, 0f);  
            }

            Debug.Log("Mobile Input: " + touchDirection);  // Debug touch direction
        }
    }


    private void MoveForward()
    {
        Vector3 forwardDirection = transform.forward ;
        forwardDirection.y = 0;
        charControl.Move(forwardDirection * speed * Time.deltaTime);  
                 
    }

       private void MoveRight()
    {
        Vector3 rightDirection = transform.right * speed * Time.deltaTime;
        charControl.Move(rightDirection);                   
    }

    private void MoveLeft()
    {
        Vector3 leftDirection = -transform.right * speed * Time.deltaTime;
        charControl.Move(leftDirection);                   
    }

    private void HandleVerticalMovement()
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

    private void TriggerRunAnimation(float horizontal, float vertical)
    
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
