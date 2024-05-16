using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Base")]
    public float moveSpeed;
    public float walkSpeed;
    public float SprintSpeed;
    public KeyCode SprintKey = KeyCode.LeftShift;
    public float gravity;

    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    public Rigidbody rb;


    [Header("Check For Ground")]
    public float playerHeight;
    public float groundDrag;
    public bool grounded;
    public LayerMask whatIsGround;

    [Header("Check For Slope")]
    public float maxSlopeAngle;
    private RaycastHit SlopeHit;
    public bool exitSlope;


    [Header("Jump")]
    public float jumpForce;
    public float airMultiplier;
    public bool canJump;
    public KeyCode jumpKey = KeyCode.Space;
    public CheckSpawn spawn;


   



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        spawn = GameObject.Find("Spawner").GetComponent<CheckSpawn>();
        transform.position = spawn.CurrentSpawn.position;
    }

    // Update is called once per frame
    void Update()
    {
        //verifica daca e ceva sub player
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        

        GetInput();

        ControlSpeed();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        

    }

    private void FixedUpdate()
    {
        if(!OnSlope())
            rb.AddForce(Vector3.down * gravity,ForceMode.Acceleration);

        MovePlayer();
    }

    private void GetInput()
    {
        //primeste input de la player
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        //verifica daca alergi
        if (Input.GetKey(SprintKey))
        {
            moveSpeed = SprintSpeed;
        }
        else
            moveSpeed = walkSpeed;



        //check for jump
        if(Input.GetKey(jumpKey) && canJump && grounded)
        {
            canJump = false;

            Jump();
        }
    }

    private void MovePlayer()
    {
        //gaseste directia si aplica forta depinzand de input
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(OnSlope() && !exitSlope)
        {
            rb.AddForce(GetSlopeDirection() * moveSpeed * 20, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80, ForceMode.Force);
            
        }

        //pe pamant/in aer
        else if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);

        
    }

    private void ControlSpeed()
    {
        if(OnSlope() && !exitSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 Vel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            //limiteaza viteza daca e prea mare
            if (Vel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = Vel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

      
    }

    private void Jump()
    {
        exitSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter()
    {
        if (grounded)
        {
            canJump = true;

            exitSlope = false;
        }
           
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out SlopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, SlopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, SlopeHit.normal).normalized;
    }

}
