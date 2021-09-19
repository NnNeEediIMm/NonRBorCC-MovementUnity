using System.Collections;
using System;
using UnityEngine;

/*Non rigidbody and non-
 character controller
movement by NnNeEediIMm*/

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;
    
    //Input
    [Header("Input")]
    public KeyCode crouchI = KeyCode.LeftShift;
    public KeyCode sprintI = KeyCode.LeftControl;
    
    float x, z;

    //gravity
    [Header("Gravity")]
    public LayerMask ground;
    public Transform groundCheck;
    public float gravityScale = 0.1f;

    private float defaultGravityScale;
    private float distToGround = 0.01f;
    private bool isGrounded = false;

    //jumping
    bool jumping;
    bool canJump = true;
    [Header("Jumping")]
    public float jumpForce = 0.1f;

    //crouching
    [Header("Crouching")]
    public bool crouching = true;
    public float reducedSize = 0.5f;

    private float speedWhileCrouching;
    private float defaultSize;
    bool crouchingInput;
    bool crouchingInputUp;

    //sprinting
    [Header("Sprint")]
    public bool sprinting = true;
    public float sprintSpeed = 15f;

    private float defaultSpeed;
    bool sprint;
    bool sprintUp;


    private void Start()
    {
        //gravity
        defaultGravityScale = gravityScale;

        //Movement
        speedWhileCrouching = speed / 2;
        defaultSpeed = speed;

        //crouching
        defaultSize = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Input
        myInput();

        //other mechanics
        Crouching();
        sprinnting();
    }

    private void FixedUpdate()
    {
        //gravity
        gravity();

        //jumping
        jump();

        /*Main movement*/
        movement();
    }

    //Gravity
    public void gravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, distToGround, ground);
        if (!isGrounded)
        {
            gravityScale = defaultGravityScale;
            transform.Translate(0f, -gravityScale, 0f);
        }
        else
        {
            gravityScale = 0f;
        }
    }
    //end of the gravity

    public void myInput()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        jumping = Input.GetKeyDown(KeyCode.Space);

        crouchingInput = Input.GetKey(crouchI) && Input.GetKey(KeyCode.W);
        crouchingInputUp = Input.GetKeyUp(crouchI);

        sprint = Input.GetKey(sprintI) && Input.GetKey(KeyCode.W);
        sprintUp = Input.GetKeyUp(sprintI);
    }
    //jumping
    public void jump()
    {
        if (isGrounded)
        {
            if (jumping && canJump)
            {
                StartCoroutine(jumpNow());
                canJump = false;
            }
        }
    }
    IEnumerator jumpNow()
    {
        for (int jump = 0; jump < 8; jump++)
        {
            yield return new WaitForSeconds(0.000000000000000000000000001f);
            transform.Translate(0, 0.1f + jumpForce, 0);
        }
        yield return new WaitForSeconds(0.13f);
        canJump = true;
    }

    IEnumerator crouch()
    {
        yield return new WaitForSeconds(0.00000001f);
        crouching = false;
        yield return new WaitForSeconds(0.15f);
        crouching = true;
        StopCoroutine(crouch());
    }
    /// <summary>
    /// Other mechanics of the movement!!
    /// Crouching
    /// </summary>
    public void Crouching()
    {
        if (isGrounded && crouching)
        {
            if (crouchingInput)
            {
                transform.localScale = new Vector3(transform.localScale.x, reducedSize, transform.localScale.z);
                speed = speedWhileCrouching;
            }
            if (!crouchingInput)
            {
                StopCrouching();
                StartCoroutine(crouch());
                normalPositionAfterCrouching();
                speed = defaultSpeed;
            }
        }
    }
    public void StopCrouching()
    {
        transform.localScale = new Vector3(transform.localScale.x, defaultSize, transform.localScale.z);
    }
    public void normalPositionAfterCrouching()
    {
        if (crouchingInputUp || crouchingInput && transform.localScale.y > 0.51)
        {
            transform.Translate(Vector3.up * 2.5f);
        }
    }
    //sprinting 
    public void sprinnting()
    {
        if (isGrounded && sprinting)
        {
            if (sprint)
            {
                speed = sprintSpeed;
            }
            if (sprintUp)
            {
                normalSpeed();
            }
        }
    }
    public void normalSpeed()
    {
        speed = defaultSpeed;
    }

    /// <summary>
    /// Movement 
    /// for all script!
    /// </summary>
    public void movement() 
    {
        transform.Translate(Vector3.right * x * Time.fixedDeltaTime * speed);
        transform.Translate(Vector3.forward * z * Time.fixedDeltaTime * speed);
    }
}
